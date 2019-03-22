using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tools;

namespace Prometheus.Client
{
    /// <inheritdoc cref="IHistogram" />
    public class Histogram : Collector<Histogram.LabelledHistogram, Histogram.HistogramConfiguration>, IHistogram
    {
        internal Histogram(HistogramConfiguration configuration)
            : base(configuration)
        {
        }

        public HistogramState Value => Unlabelled.Value;

        protected override MetricType Type => MetricType.Histogram;

        public void Observe(double val)
        {
            Unlabelled.Observe(val);
        }

        public void Observe(double val, long? timestamp)
        {
            Unlabelled.Observe(val, timestamp);
        }

        public class LabelledHistogram : Labelled<HistogramConfiguration>, IHistogram
        {
            private ThreadSafeLong[] _bucketCounts;
            private ThreadSafeDouble _sum = new ThreadSafeDouble(0.0D);

            public HistogramState Value => ForkState();

            public void Observe(double val)
            {
                Observe(val, null);
            }

            public void Observe(double val, long? timestamp)
            {
                if (double.IsNaN(val))
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

            protected internal override void Init(LabelValues labelValues, HistogramConfiguration configuration)
            {
                base.Init(labelValues, configuration);

                _bucketCounts = new ThreadSafeLong[Configuration.Buckets.Count];
            }

            protected internal override void Collect(IMetricsWriter writer)
            {
                long cumulativeCount = 0L;

                for (int i = 0; i < _bucketCounts.Length; i++)
                {
                    cumulativeCount += _bucketCounts[i].Value;
                    var bucketSample = writer.StartSample("_bucket");
                    var labelWriter = bucketSample.StartLabels();
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

        public class HistogramConfiguration : MetricConfiguration
        {
            private static readonly double[] _defaultBuckets = { .005, .01, .025, .05, .075, .1, .25, .5, .75, 1, 2.5, 5, 7.5, 10 };

            public HistogramConfiguration(string name, string help, bool includeTimestamp, IReadOnlyList<string> labels, IReadOnlyList<double> buckets)
                : base(name, help, includeTimestamp, labels)
            {
                Buckets = buckets ?? _defaultBuckets;

                if (LabelNames.Any(l => l == "le"))
                    throw new ArgumentException("'le' is a reserved label name");

                if (Buckets.Count == 0)
                    throw new ArgumentException("Histogram must have at least one bucket");

                if (!double.IsPositiveInfinity(Buckets[Buckets.Count - 1]))
                    Buckets = Buckets.Concat(new[] { double.PositiveInfinity }).ToArray();

                for (int i = 1; i < Buckets.Count; i++)
                {
                    if (Buckets[i] <= Buckets[i - 1])
                        throw new ArgumentException("Bucket values must be increasing");
                }

                FormattedBuckets = Buckets
                    .Select(b => b.ToString(CultureInfo.InvariantCulture))
                    .ToArray();
            }

            public IReadOnlyList<double> Buckets { get; }

            internal IReadOnlyList<string> FormattedBuckets { get; }
        }
    }
}
