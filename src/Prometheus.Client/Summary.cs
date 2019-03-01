using System;
using System.Collections.Generic;
using System.Linq;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.SummaryImpl;

namespace Prometheus.Client
{
    /// <inheritdoc cref="ISummary" />
    public class Summary : Collector<Summary.LabelledSummary, Summary.SummaryConfiguration>, ISummary
    {
        internal Summary(SummaryConfiguration configuration)
            : base(configuration)
        {
        }

        protected override MetricType Type => MetricType.Summary;

        public void Observe(double val)
        {
            Unlabelled.Observe(val);
        }

        public class LabelledSummary : Labelled<SummaryConfiguration>, ISummary
        {
            // Protects hotBuf and hotBufExpTime.
            private readonly object _bufLock = new object();

            // Protects every other moving part.
            // Lock bufMtx before mtx if both are needed.
            private readonly object _lock = new object();

            private SampleBuffer _coldBuf;
            private uint _count;
            private QuantileStream _headStream;
            private DateTime _headStreamExpTime;
            private int _headStreamIdx;
            private SampleBuffer _hotBuf;
            private DateTime _hotBufExpTime;

            private double[] _sortedObjectives;
            private TimeSpan _streamDuration;
            private QuantileStream[] _streams;
            private double _sum;

            public void Observe(double val)
            {
                Observe(val, DateTime.UtcNow);
            }

            protected internal override void Init(LabelValues labelValues, SummaryConfiguration configuration)
            {
                Init(labelValues, configuration, DateTime.UtcNow);
            }

            internal void Init(LabelValues labelValues, SummaryConfiguration configuration, DateTime now)
            {
                base.Init(labelValues, configuration);

                _sortedObjectives = new double[Configuration.Objectives.Count];
                _hotBuf = new SampleBuffer(Configuration.BufCap);
                _coldBuf = new SampleBuffer(Configuration.BufCap);
                _streamDuration = new TimeSpan(Configuration.MaxAge.Ticks / Configuration.AgeBuckets);
                _headStreamExpTime = now.Add(_streamDuration);
                _hotBufExpTime = _headStreamExpTime;

                _streams = new QuantileStream[Configuration.AgeBuckets];
                for (int i = 0; i < Configuration.AgeBuckets; i++)
                    _streams[i] = QuantileStream.NewTargeted(Configuration.Objectives);

                _headStream = _streams[0];

                for (int i = 0; i < Configuration.Objectives.Count; i++)
                    _sortedObjectives[i] = Configuration.Objectives[i].Quantile;

                Array.Sort(_sortedObjectives);
            }

            internal SummaryState ForkState(DateTime now)
            {
                var values = new KeyValuePair<double, double>[Configuration.Objectives.Count];

                lock (_bufLock)
                {
                    lock (_lock)
                    {
                        // Swap bufs even if hotBuf is empty to set new hotBufExpTime.
                        SwapBufs(now);
                        FlushColdBuf();

                        for (int i = 0; i < _sortedObjectives.Length; i++)
                        {
                            double rank = _sortedObjectives[i];
                            double value = _headStream.Count == 0 ? double.NaN : _headStream.Query(rank);

                            values[i] = new KeyValuePair<double, double>(rank, value);
                        }

                        return new SummaryState
                        {
                            Values = values,
                            Count = _count,
                            Sum = _sum
                        };
                    }
                }
            }

            protected internal override void Collect(IMetricsWriter writer)
            {
                var state = ForkState(DateTime.UtcNow);

                for (int i = 0; i < state.Values.Length; i++)
                {
                    var bucketSample = writer.StartSample();
                    var labelWriter = bucketSample.StartLabels();
                    labelWriter.WriteLabels(Labels);
                    labelWriter.WriteLabel("quantile", state.Values[i].Key.ToString());
                    labelWriter.EndLabels();

                    bucketSample.WriteValue(state.Values[i].Value);
                    if (Configuration.IncludeTimestamp && Timestamp.HasValue)
                        bucketSample.WriteTimestamp(Timestamp.Value);
                }

                writer.WriteSample(state.Sum, "_sum", Labels, Timestamp);
                writer.WriteSample(state.Count, "_count", Labels, Timestamp);
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

                    TimestampIfRequired();
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
                    throw new ArgumentException("coldBuf is not empty");

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
                for (int bufIdx = 0; bufIdx < _coldBuf.Position; bufIdx++)
                {
                    double value = _coldBuf[bufIdx];

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

            internal struct SummaryState
            {
                public double Sum { get; set; }

                public uint Count { get; set; }

                public KeyValuePair<double, double>[] Values { get; set; }
            }
        }

        public class SummaryConfiguration : MetricConfiguration
        {
            // Label that defines the quantile in a summary.
            private const string _quantileLabel = "quantile";

            // Default number of buckets used to calculate the age of observations
            private const int _defaultAgeBuckets = 5;

            // Standard buffer size for collecting Summary observations
            private const int _defaultBufCap = 500;

            // Default Summary quantile values.
            public static readonly IReadOnlyList<QuantileEpsilonPair> DefaultObjectives = new List<QuantileEpsilonPair>
            {
                new QuantileEpsilonPair(0.5, 0.05),
                new QuantileEpsilonPair(0.9, 0.01),
                new QuantileEpsilonPair(0.99, 0.001)
            };

            // Default duration for which observations stay relevant
            private static readonly TimeSpan _defaultMaxAge = TimeSpan.FromMinutes(10);

            public SummaryConfiguration(
                string name,
                string help,
                bool includeTimestamp,
                string[] labelNames,
                IReadOnlyList<QuantileEpsilonPair> objectives = null,
                TimeSpan? maxAge = null,
                int? ageBuckets = null,
                int? bufCap = null)
            : base(name, help, includeTimestamp, labelNames)
            {
                Objectives = objectives;
                if (Objectives == null || Objectives.Count == 0)
                {
                    Objectives = DefaultObjectives;
                }

                MaxAge = maxAge ?? _defaultMaxAge;
                if (MaxAge < TimeSpan.Zero)
                    throw new ArgumentException($"Illegal max age {MaxAge}");

                AgeBuckets = ageBuckets ?? _defaultAgeBuckets;
                if (AgeBuckets == 0)
                    AgeBuckets = _defaultAgeBuckets;

                BufCap = bufCap ?? _defaultBufCap;
                if (BufCap == 0)
                    BufCap = _defaultBufCap;

                if (LabelNames.Any(_ => _ == _quantileLabel))
                    throw new ArgumentException($"{_quantileLabel} is a reserved label name");
            }

            // Objectives defines the quantile rank estimates with their respective
            // absolute error. If Objectives[q] = e, then the value reported
            // for q will be the φ-quantile value for some φ between q-e and q+e.
            // The default value is DefObjectives.
            public IReadOnlyList<QuantileEpsilonPair> Objectives { get; }

            // MaxAge defines the duration for which an observation stays relevant
            // for the summary. Must be positive. The default value is DefMaxAge.
            public TimeSpan MaxAge { get; }

            // AgeBuckets is the number of buckets used to exclude observations that
            // are older than MaxAge from the summary. A higher number has a
            // resource penalty, so only increase it if the higher resolution is
            // really required. For very high observation rates, you might want to
            // reduce the number of age buckets. With only one age bucket, you will
            // effectively see a complete reset of the summary each time MaxAge has
            // passed. The default value is DefAgeBuckets.
            public int AgeBuckets { get; }

            // BufCap defines the default sample stream buffer size.  The default
            // value of DefBufCap should suffice for most uses. If there is a need
            // to increase the value, a multiple of 500 is recommended (because that
            // is the internal buffer size of the underlying package
            // "github.com/bmizerany/perks/quantile").
            public int BufCap { get; }
        }
    }
}
