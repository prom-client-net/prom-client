using System;
using System.Linq;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.Contracts;
using Prometheus.Client.Tools;

namespace Prometheus.Client
{
    /// <inheritdoc cref="IHistogram" />
    public class Histogram : Collector<Histogram.ThisChild>, IHistogram
    {
        private static readonly double[] _defaultBuckets = { .005, .01, .025, .05, .075, .1, .25, .5, .75, 1, 2.5, 5, 7.5, 10 };
        private readonly double[] _buckets;

        internal Histogram(string name, string help, string[] labelNames, double[] buckets = null)
            : this(name, help, false, labelNames, buckets)
        {
        }
        
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
                if (_buckets[i] <= _buckets[i - 1])
                    throw new ArgumentException("Bucket values must be increasing");
        }

        /// <summary>
        ///     Metric Type
        /// </summary>
        protected override CMetricType Type => CMetricType.Histogram;

        public void Observe(double val)
        {
            Unlabelled.Observe(val);
        }

        public class ThisChild : Child, IHistogram
        {
            private ThreadSafeLong[] _bucketCounts;
            private ThreadSafeDouble _sum = new ThreadSafeDouble(0.0D);
            private double[] _upperBounds;

            public void Observe(double val)
            {
                if (double.IsNaN(val))
                    return;


                for (int i = 0; i < _upperBounds.Length; i++)
                    if (val <= _upperBounds[i])
                    {
                        _bucketCounts[i].Add(1);
                        break;
                    }

                _sum.Add(val);
            }

            internal override void Init(ICollector parent, LabelValues labelValues)
            {
                base.Init(parent, labelValues);

                _upperBounds = ((Histogram) parent)._buckets;
                _bucketCounts = new ThreadSafeLong[_upperBounds.Length];
            }

            protected override void Populate(CMetric cMetric)
            {
                var wireMetric = new CHistogram
                {
                    SampleCount = 0L,
                    Buckets = new CBucket[_bucketCounts.Length]
                };

                for (int i = 0; i < _bucketCounts.Length; i++)
                {
                    wireMetric.SampleCount += (ulong) _bucketCounts[i].Value;
                    wireMetric.Buckets[i] = new CBucket
                    {
                        UpperBound = _upperBounds[i],
                        CumulativeCount = wireMetric.SampleCount
                    };
                }

                wireMetric.SampleSum = _sum.Value;

                cMetric.CHistogram = wireMetric;
            }
        }
    }
}
