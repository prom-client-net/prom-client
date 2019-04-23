using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
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

        public SummaryState Value => Unlabelled.Value;

        public void Observe(double val)
        {
            Unlabelled.Observe(val);
        }

        public void Observe(double val, long? timestamp)
        {
            Unlabelled.Observe(val, timestamp);
        }

        public class LabelledSummary : Labelled<SummaryConfiguration>, ISummary
        {
            private static readonly ArrayPool<double> _arrayPool = ArrayPool<double>.Shared;
            // Protects hotBuf and hotBufExpTime.
            private readonly object _bufLock = new object();

            // Protects every other moving part.
            // Lock bufMtx before mtx if both are needed.
            private readonly object _lock = new object();

            private SampleBuffer _buffer;
            private DateTime _bufferExpTime;
            private long _count;
            private QuantileStream _headStream;
            private DateTime _headStreamExpTime;
            private int _headStreamIdx;

            private TimeSpan _streamDuration;
            private QuantileStream[] _streams;
            private double _sum;

            public SummaryState Value
            {
                get
                {
                    var values = new double[Configuration.SortedObjectives.Count];
                    ForkState(DateTime.UtcNow, out var count, out var sum, values);
                    var zipped = values.Zip(Configuration.SortedObjectives, (v, k) => new KeyValuePair<double, double>(k, v)).ToArray();
                    return new SummaryState(count, sum, zipped);
                }
            }

            public void Observe(double val)
            {
                Observe(val, null);
            }

            public void Observe(double val, long? timestamp)
            {
                Observe(val, timestamp, DateTime.UtcNow);
            }

            /// <summary>
            ///     For unit tests only
            /// </summary>
            internal void Observe(double val, long? timestamp, DateTime now)
            {
                lock (_bufLock)
                {
                    if (now > _bufferExpTime)
                        Flush(now);

                    _buffer.Append(val);

                    if (_buffer.IsFull)
                        Flush(now);
                }

                TimestampIfRequired(timestamp);
            }

            protected internal override void Init(LabelValues labelValues, SummaryConfiguration configuration)
            {
                Init(labelValues, configuration, DateTime.UtcNow);
            }

            internal void Init(LabelValues labelValues, SummaryConfiguration configuration, DateTime now)
            {
                base.Init(labelValues, configuration);

                _buffer = new SampleBuffer(Configuration.BufCap);
                _streamDuration = new TimeSpan(Configuration.MaxAge.Ticks / Configuration.AgeBuckets);
                _headStreamExpTime = now.Add(_streamDuration);
                _bufferExpTime = _headStreamExpTime;

                _streams = new QuantileStream[Configuration.AgeBuckets];
                for (int i = 0; i < Configuration.AgeBuckets; i++)
                    _streams[i] = QuantileStream.NewTargeted(Configuration.Objectives);

                _headStream = _streams[0];
            }

            internal void ForkState(DateTime now, out long count, out double sum, double[] values)
            {
                lock (_bufLock)
                {
                    lock (_lock)
                    {
                        // FlushBuffer even if buffer is empty to set new bufferExpTime.
                        FlushBuffer(now);

                        for (int i = 0; i < Configuration.SortedObjectives.Count; i++)
                        {
                            double rank = Configuration.SortedObjectives[i];
                            double value = _headStream.Count == 0 ? double.NaN : _headStream.Query(rank);

                            values[i] = value;
                        }

                        count = _count;
                        sum = _sum;
                    }
                }
            }

            protected internal override void Collect(IMetricsWriter writer)
            {
                var values = _arrayPool.Rent(Configuration.SortedObjectives.Count);

                try
                {
                    ForkState(DateTime.UtcNow, out var count, out var sum, values);

                    for (int i = 0; i < Configuration.SortedObjectives.Count; i++)
                    {
                        var bucketSample = writer.StartSample();
                        var labelWriter = bucketSample.StartLabels();
                        labelWriter.WriteLabels(Labels);
                        labelWriter.WriteLabel("quantile", Configuration.FormattedObjectives[i]);
                        labelWriter.EndLabels();

                        bucketSample.WriteValue(values[i]);
                        if (Timestamp.HasValue)
                            bucketSample.WriteTimestamp(Timestamp.Value);

                        bucketSample.EndSample();
                    }

                    writer.WriteSample(sum, "_sum", Labels, Timestamp);
                    writer.WriteSample(count, "_count", Labels, Timestamp);
                }
                finally
                {
                    _arrayPool.Return(values);
                }
            }

            // Flush needs bufMtx locked.
            private void Flush(DateTime now)
            {
                lock (_lock)
                {
                    FlushBuffer(now);
                }
            }

            // FlushBuffer needs mtx AND bufMtx locked. 
            private void FlushBuffer(DateTime now)
            {
                for (int bufIdx = 0; bufIdx < _buffer.Position; bufIdx++)
                {
                    double value = _buffer[bufIdx];

                    foreach (var quantileStream in _streams)
                        quantileStream.Insert(value);

                    _count++;
                    _sum += value;
                }

                _buffer.Reset();

                // buffer is now empty and gets new expiration set.
                while (now > _bufferExpTime)
                    _bufferExpTime = _bufferExpTime.Add(_streamDuration);

                MaybeRotateStreams();
            }

            // MaybeRotateStreams needs mtx AND bufMtx locked.
            private void MaybeRotateStreams()
            {
                while (!_bufferExpTime.Equals(_headStreamExpTime))
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
                bool suppressEmptySamples,
                IReadOnlyList<string> labelNames,
                IReadOnlyList<QuantileEpsilonPair> objectives = null,
                TimeSpan? maxAge = null,
                int? ageBuckets = null,
                int? bufCap = null)
            : base(name, help, includeTimestamp, suppressEmptySamples, labelNames)
            {
                Objectives = objectives;
                if (Objectives == null || Objectives.Count == 0)
                {
                    Objectives = DefaultObjectives;
                }

                var sorted = new double[Objectives.Count];
                for (int i = 0; i < Objectives.Count; i++)
                    sorted[i] = Objectives[i].Quantile;

                Array.Sort(sorted);
                SortedObjectives = sorted;
                FormattedObjectives = SortedObjectives
                    .Select(o => o.ToString(CultureInfo.InvariantCulture))
                    .ToArray();

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

            internal IReadOnlyList<double> SortedObjectives { get; }

            internal IReadOnlyList<string> FormattedObjectives { get; }
        }
    }
}
