using System;
using System.Globalization;
using System.Linq;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tools;

namespace Prometheus.Client
{
    /// <inheritdoc cref="IHistogram" />
    public class Histogram : Collector<Histogram.LabelledHistogram>, IHistogram
    {
        private static readonly double[] _defaultBuckets = { .005, .01, .025, .05, .075, .1, .25, .5, .75, 1, 2.5, 5, 7.5, 10 };
        private readonly double[] _buckets;

        internal Histogram(string name, string help, bool includeTimestamp, string[] labelNames, double[] buckets = null)
            : base(name, help, includeTimestamp, labelNames)
        {
            if (labelNames.Any(l => l == "le"))
                throw new ArgumentException("'le' is a reserved label name");

            _buckets = buckets ?? _defaultBuckets;

            if (_buckets.Length == 0)
                throw new ArgumentException("Histogram must have at least one bucket");

            if (!double.IsPositiveInfinity(_buckets[_buckets.Length - 1]))
                _buckets = _buckets.Concat(new[] { double.PositiveInfinity }).ToArray();

            for (int i = 1; i < _buckets.Length; i++)
            {
                if (_buckets[i] <= _buckets[i - 1])
                    throw new ArgumentException("Bucket values must be increasing");
            }
        }

        /// <summary>
        ///     Metric Type
        /// </summary>
        protected override MetricType Type => MetricType.Histogram;

        public void Observe(double val)
        {
            Unlabelled.Observe(val);
        }

        public class LabelledHistogram : Labelled, IHistogram
        {
            private ThreadSafeLong[] _bucketCounts;
            private ThreadSafeDouble _sum = new ThreadSafeDouble(0.0D);
            private double[] _upperBounds;

            public void Observe(double val)
            {
                if (double.IsNaN(val))
                    return;

                for (int i = 0; i < _upperBounds.Length; i++)
                {
                    if (val <= _upperBounds[i])
                    {
                        _bucketCounts[i].Add(1);
                        break;
                    }
                }

                _sum.Add(val);

                if (IncludeTimestamp)
                    SetTimestamp();
            }

            internal override void Init(ICollector parent, LabelValues labelValues, bool includeTimestamp)
            {
                base.Init(parent, labelValues, includeTimestamp);

                _upperBounds = ((Histogram)parent)._buckets;
                _bucketCounts = new ThreadSafeLong[_upperBounds.Length];
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
                    string labelValue = double.IsPositiveInfinity(_upperBounds[i]) ? "+Inf" : _upperBounds[i].ToString(CultureInfo.InvariantCulture);
                    labelWriter.WriteLabel("le", labelValue);
                    labelWriter.EndLabels();

                    bucketSample.WriteValue(cumulativeCount);
                    if (IncludeTimestamp && Timestamp.HasValue)
                        bucketSample.WriteTimestamp(Timestamp.Value);
                }

                writer.WriteSample(_sum.Value, "_sum", Labels, Timestamp);
                writer.WriteSample(cumulativeCount, "_count", Labels, Timestamp);
            }
        }
    }
}
