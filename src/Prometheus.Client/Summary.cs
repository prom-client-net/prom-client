using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Prometheus.Client.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;
using Prometheus.Client.SummaryImpl;

namespace Prometheus.Client
{
    /// <inheritdoc cref="ISummary" />
    public sealed class Summary : MetricBase<SummaryConfiguration>, ISummary
    {
        private static readonly ArrayPool<double> _arrayPool = ArrayPool<double>.Shared;

        private readonly QuantileStream _quantileStream;
        private ThreadSafeDouble _sum = default;
        private ThreadSafeLong _count = default;

        public Summary(SummaryConfiguration configuration, IReadOnlyList<string> labels, Func<DateTimeOffset> currentTimeProvider = null)
            : base(configuration, labels, currentTimeProvider)
        {
            var streamDuration = new TimeSpan(Configuration.MaxAge.Ticks / Configuration.AgeBuckets);

            _quantileStream = new QuantileStream(
                Configuration.BufCap,
                streamDuration,
                configuration.AgeBuckets,
                Invariants.Targeted(Configuration.Objectives),
                currentTimeProvider);
        }

        public SummaryState Value
        {
            get
            {
                var values = new double[Configuration.SortedObjectives.Length];
                ForkState(out var count, out var sum, values);
                var zipped = values.Zip(Configuration.SortedObjectives, (v, k) => new KeyValuePair<double, double>(k, v)).ToArray();
                return new SummaryState(count, sum, zipped);
            }
        }

        public void Observe(double val)
        {
            Observe(val, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Observe(double val, long? timestamp)
        {
            _quantileStream.Append(val);
            _count.Add(1);
            _sum.Add(val);

            TrackObservation(timestamp);
        }

        internal void ForkState(out long count, out double sum, double[] values)
        {
            _quantileStream.FlushBuffer();

            for (int i = 0; i < Configuration.SortedObjectives.Length; i++)
            {
                double rank = Configuration.SortedObjectives[i];
                double value = _quantileStream.Query(rank);

                values[i] = value;
            }

            count = _count.Value;
            sum = _sum.Value;
        }

        protected internal override void Collect(IMetricsWriter writer)
        {
            var values = _arrayPool.Rent(Configuration.SortedObjectives.Length);

            try
            {
                ForkState(out var count, out var sum, values);

                for (int i = 0; i < Configuration.SortedObjectives.Length; i++)
                {
                    var bucketSample = writer.StartSample();
                    var labelWriter = bucketSample.StartLabels();
                    if (LabelValues != null && LabelValues.Count > 0)
                        labelWriter.WriteLabels(Configuration.LabelNames, LabelValues);

                    labelWriter.WriteLabel("quantile", Configuration.FormattedObjectives[i]);
                    labelWriter.EndLabels();

                    bucketSample.WriteValue(values[i]);
                    if (Timestamp.HasValue)
                        bucketSample.WriteTimestamp(Timestamp.Value);

                    bucketSample.EndSample();
                }

                writer.WriteSample(sum, "_sum", Configuration.LabelNames, LabelValues, Timestamp);
                writer.WriteSample(count, "_count", Configuration.LabelNames, LabelValues, Timestamp);
            }
            finally
            {
                _arrayPool.Return(values);
            }
        }
    }
}
