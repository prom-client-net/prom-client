using System;
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
        private readonly IHistogramBucketStore _bucketsStore;
        private ThreadSafeDouble _sum = new ThreadSafeDouble(0.0D);

        public Histogram(HistogramConfiguration configuration, IReadOnlyList<string> labels)
            : base(configuration, labels)
        {
            if (configuration.Buckets.Length >= 25)
                _bucketsStore = new HistogramHighBucketsStore(configuration.Buckets);
            else
                _bucketsStore = new HistogramLowBucketsStore(configuration.Buckets);
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

            _bucketsStore.Observe(val);
            _sum.Add(val);
            TrackObservation(timestamp);
        }

        protected internal override void Collect(IMetricsWriter writer)
        {
            long cumulativeCount = 0L;

            for (int i = 0; i < _bucketsStore.Buckets.Length; i++)
            {
                cumulativeCount += _bucketsStore.Buckets[i].Value;
                var bucketSample = writer.StartSample("_bucket");
                var labelWriter = bucketSample.StartLabels();
                if (LabelValues != null && LabelValues.Count > 0)
                    labelWriter.WriteLabels(Configuration.LabelNames, LabelValues);

                string labelValue = Configuration.FormattedBuckets[i];
                labelWriter.WriteLabel("le", labelValue);
                labelWriter.EndLabels();

                bucketSample.WriteValue(cumulativeCount);
                if (Timestamp.HasValue)
                    bucketSample.WriteTimestamp(Timestamp.Value);

                bucketSample.EndSample();
            }

            writer.WriteSample(_sum.Value, "_sum", Configuration.LabelNames, LabelValues, Timestamp);
            writer.WriteSample(cumulativeCount, "_count", Configuration.LabelNames, LabelValues, Timestamp);
        }

        private HistogramState ForkState()
        {
            long cumulativeCount = 0L;
            var buckets = new KeyValuePair<double, long>[_bucketsStore.Buckets.Length];

            for (int i = 0; i < _bucketsStore.Buckets.Length; i++)
            {
                cumulativeCount += _bucketsStore.Buckets[i].Value;
                var bound = (i == _bucketsStore.Buckets.Length - 1) ? double.PositiveInfinity : Configuration.Buckets[i];

                buckets[i] = new KeyValuePair<double, long>(bound, cumulativeCount);
            }

            return new HistogramState(cumulativeCount, _sum.Value, buckets);
        }
    }
}
