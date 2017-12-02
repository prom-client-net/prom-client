using System;
using System.Collections.Generic;
using System.Linq;
using Prometheus.Client.Collectors;
using Prometheus.Client.Contracts;
using Prometheus.Client.SummaryImpl;

namespace Prometheus.Client
{
    /// <summary>
    ///     Summary metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#summary
    ///     </remarks>
    /// </summary>
    public interface ISummary
    {
        void Observe(double val);
    }

    /// <summary>
    ///     Summary metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#summary
    ///     </remarks>
    /// </summary>
    public class Summary : Collector<Summary.ThisChild>, ISummary
    {
        // Label that defines the quantile in a summary.
        private const string _quantileLabel = "quantile";

        // Default number of buckets used to calculate the age of observations
        private const int _defAgeBuckets = 5;

        // Standard buffer size for collecting Summary observations
        private const int _defBufCap = 500;

        // Default Summary quantile values.
        public static readonly IReadOnlyCollection<QuantileEpsilonPair> DefObjectives = new List<QuantileEpsilonPair>
        {
            new QuantileEpsilonPair(0.5, 0.05),
            new QuantileEpsilonPair(0.9, 0.01),
            new QuantileEpsilonPair(0.99, 0.001)
        };

        // Default duration for which observations stay relevant
        private static readonly TimeSpan _defMaxAge = TimeSpan.FromMinutes(10);

        private readonly int _ageBuckets;
        private readonly int _bufCap;
        private readonly TimeSpan _maxAge;

        private readonly IList<QuantileEpsilonPair> _objectives;

        protected override MetricType Type => MetricType.Summary;

        internal Summary(
            string name,
            string help,
            string[] labelNames,
            IList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null)
            : base(name, help, labelNames)
        {
            _objectives = objectives ?? DefObjectives.ToList();
            _maxAge = maxAge ?? _defMaxAge;
            _ageBuckets = ageBuckets ?? _defAgeBuckets;
            _bufCap = bufCap ?? _defBufCap;

            if (_objectives.Count == 0)
                _objectives = DefObjectives.ToList();

            if (_maxAge < TimeSpan.Zero)
                throw new ArgumentException($"Illegal max age {_maxAge}");

            if (_ageBuckets == 0)
                _ageBuckets = _defAgeBuckets;

            if (_bufCap == 0)
                _bufCap = _defBufCap;

            if (labelNames.Any(_ => _ == _quantileLabel))
                throw new ArgumentException($"{_quantileLabel} is a reserved label name");
        }

        public void Observe(double val)
        {
            Unlabelled.Observe(val);
        }

        public class ThisChild : Child, ISummary
        {
            // Protects hotBuf and hotBufExpTime.
            private readonly object _bufLock = new object();

            // Protects every other moving part.
            // Lock bufMtx before mtx if both are needed.
            private readonly object _lock = new object();

            private readonly QuantileComparer _quantileComparer = new QuantileComparer();

            // AgeBuckets is the number of buckets used to exclude observations that
            // are older than MaxAge from the summary. A higher number has a
            // resource penalty, so only increase it if the higher resolution is
            // really required. For very high observation rates, you might want to
            // reduce the number of age buckets. With only one age bucket, you will
            // effectively see a complete reset of the summary each time MaxAge has
            // passed. The default value is DefAgeBuckets.
            private int _ageBuckets;

            // BufCap defines the default sample stream buffer size.  The default
            // value of DefBufCap should suffice for most uses. If there is a need
            // to increase the value, a multiple of 500 is recommended (because that
            // is the internal buffer size of the underlying package
            // "github.com/bmizerany/perks/quantile").      
            private int _bufCap;

            private SampleBuffer _coldBuf;
            private uint _count;
            private QuantileStream _headStream;
            private DateTime _headStreamExpTime;
            private int _headStreamIdx;
            private SampleBuffer _hotBuf;
            private DateTime _hotBufExpTime;

            // MaxAge defines the duration for which an observation stays relevant
            // for the summary. Must be positive. The default value is DefMaxAge.
            private TimeSpan _maxAge;

            // Objectives defines the quantile rank estimates with their respective
            // absolute error. If Objectives[q] = e, then the value reported
            // for q will be the φ-quantile value for some φ between q-e and q+e.
            // The default value is DefObjectives.
            private IList<QuantileEpsilonPair> _objectives = new List<QuantileEpsilonPair>();

            private double[] _sortedObjectives;
            private TimeSpan _streamDuration;
            private QuantileStream[] _streams;
            private double _sum;

            private Contracts.Summary _wireMetric;

            public void Observe(double val)
            {
                Observe(val, DateTime.UtcNow);
            }

            internal override void Init(ICollector parent, LabelValues labelValues)
            {
                Init(parent, labelValues, DateTime.UtcNow);
            }

            internal void Init(ICollector parent, LabelValues labelValues, DateTime now)
            {
                base.Init(parent, labelValues);

                _objectives = ((Summary)parent)._objectives;
                _maxAge = ((Summary)parent)._maxAge;
                _ageBuckets = ((Summary)parent)._ageBuckets;
                _bufCap = ((Summary)parent)._bufCap;

                _sortedObjectives = new double[_objectives.Count];
                _hotBuf = new SampleBuffer(_bufCap);
                _coldBuf = new SampleBuffer(_bufCap);
                _streamDuration = new TimeSpan(_maxAge.Ticks / _ageBuckets);
                _headStreamExpTime = now.Add(_streamDuration);
                _hotBufExpTime = _headStreamExpTime;

                _streams = new QuantileStream[_ageBuckets];
                for (var i = 0; i < _ageBuckets; i++)
                    _streams[i] = QuantileStream.NewTargeted(_objectives);

                _headStream = _streams[0];

                for (var i = 0; i < _objectives.Count; i++)
                    _sortedObjectives[i] = _objectives[i].Quantile;

                Array.Sort(_sortedObjectives);

                _wireMetric = new Contracts.Summary();

                foreach (var quantileEpsilonPair in _objectives)
                    _wireMetric.Quantiles.Add(new Quantile
                    {
                        quantile = quantileEpsilonPair.Quantile
                    });
            }

            protected override void Populate(Metric metric)
            {
                Populate(metric, DateTime.UtcNow);
            }

            internal void Populate(Metric metric, DateTime now)
            {
                var summary = new Contracts.Summary();
                var quantiles = new Quantile[_objectives.Count];

                lock (_bufLock)
                {
                    lock (_lock)
                    {
                        // Swap bufs even if hotBuf is empty to set new hotBufExpTime.
                        SwapBufs(now);
                        FlushColdBuf();
                        summary.SampleCount = _count;
                        summary.SampleSum = _sum;

                        for (var idx = 0; idx < _sortedObjectives.Length; idx++)
                        {
                            var rank = _sortedObjectives[idx];
                            var q = _headStream.Count == 0 ? double.NaN : _headStream.Query(rank);

                            quantiles[idx] = new Quantile
                            {
                                quantile = rank,
                                Value = q
                            };
                        }
                    }
                }

                if (quantiles.Length > 0)
                    Array.Sort(quantiles, _quantileComparer);

                foreach (var quantile in quantiles)
                    summary.Quantiles.Add(quantile);

                metric.Summary = summary;
            }

            /// <summary>
            ///     For unit tests only
            /// </summary>
            internal void Observe(double val, DateTime now)
            {
                lock (_bufLock)
                {
                    if (now > _hotBufExpTime)
                        Flush(now);

                    _hotBuf.Append(val);

                    if (_hotBuf.IsFull)
                        Flush(now);
                }
            }

            // Flush needs bufMtx locked.
            private void Flush(DateTime now)
            {
                lock (_lock)
                {
                    SwapBufs(now);

                    // Go version flushes on a separate goroutine, but doing this on another
                    // thread actually makes the benchmark tests slower in .net
                    FlushColdBuf();
                }
            }

            // SwapBufs needs mtx AND bufMtx locked, coldBuf must be empty.
            private void SwapBufs(DateTime now)
            {
                if (!_coldBuf.IsEmpty)
                    throw new InvalidOperationException("coldBuf is not empty");

                var temp = _hotBuf;
                _hotBuf = _coldBuf;
                _coldBuf = temp;

                // hotBuf is now empty and gets new expiration set.
                while (now > _hotBufExpTime)
                    _hotBufExpTime = _hotBufExpTime.Add(_streamDuration);
            }

            // FlushColdBuf needs mtx locked. 
            private void FlushColdBuf()
            {
                for (var bufIdx = 0; bufIdx < _coldBuf.Position; bufIdx++)
                {
                    var value = _coldBuf[bufIdx];

                    foreach (var quantileStream in _streams)
                        quantileStream.Insert(value);

                    _count++;
                    _sum += value;
                }

                _coldBuf.Reset();
                MaybeRotateStreams();
            }

            // MaybeRotateStreams needs mtx AND bufMtx locked.
            private void MaybeRotateStreams()
            {
                while (!_hotBufExpTime.Equals(_headStreamExpTime))
                {
                    _headStream.Reset();
                    _headStreamIdx++;

                    if (_headStreamIdx >= _streams.Length)
                        _headStreamIdx = 0;

                    _headStream = _streams[_headStreamIdx];
                    _headStreamExpTime = _headStreamExpTime.Add(_streamDuration);
                }
            }
        }
    }
}