using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prometheus.Client.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client
{
    /// <inheritdoc cref="IHistogram" />
    public sealed class Histogram : MetricBase<HistogramConfiguration>, IHistogram
    {
        private ThreadSafeLong[] _bucketCounts;
        private ThreadSafeDouble _sum = new ThreadSafeDouble(0.0D);

        public Histogram(HistogramConfiguration configuration, IReadOnlyList<string> labels)
            : base(configuration, labels)
        {
            _bucketCounts = new ThreadSafeLong[Configuration.Buckets.Count];
        }

        public HistogramState Value => ForkState();

        public void Observe(double val)
        {
            Observe(val, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Observe(double val, long? timestamp)
        {
            if (ThreadSafeDouble.IsNaN(val))
                return;

            for (int i = 0; i < Configuration.Buckets.Count; i++)
            {
                if (val <= Configuration.Buckets[i])
                {
                    _bucketCounts[i].Add(1);
                    break;
                }
            }

            _sum.Add(val);
            TimestampIfRequired(timestamp);
        }

        protected internal override void Collect(IMetricsWriter writer)
        {
            long cumulativeCount = 0L;

            for (int i = 0; i < _bucketCounts.Length; i++)
            {
                cumulativeCount += _bucketCounts[i].Value;
                var bucketSample = writer.StartSample("_bucket");
                var labelWriter = bucketSample.StartLabels();
                if (Labels != null)
                    labelWriter.WriteLabels(Labels);

                string labelValue = double.IsPositiveInfinity(Configuration.Buckets[i]) ? "+Inf" : Configuration.FormattedBuckets[i];
                labelWriter.WriteLabel("le", labelValue);
                labelWriter.EndLabels();

                bucketSample.WriteValue(cumulativeCount);
                if (Timestamp.HasValue)
                    bucketSample.WriteTimestamp(Timestamp.Value);

                bucketSample.EndSample();
            }

            writer.WriteSample(_sum.Value, "_sum", Labels, Timestamp);
            writer.WriteSample(cumulativeCount, "_count", Labels, Timestamp);
        }

        private HistogramState ForkState()
        {
            long cumulativeCount = 0L;
            var buckets = new KeyValuePair<double, long>[_bucketCounts.Length];

            for (int i = 0; i < _bucketCounts.Length; i++)
            {
                cumulativeCount += _bucketCounts[i].Value;
                buckets[i] = new KeyValuePair<double, long>(Configuration.Buckets[i], cumulativeCount);
            }

            return new HistogramState(cumulativeCount, _sum.Value, buckets);
        }
    }
}
