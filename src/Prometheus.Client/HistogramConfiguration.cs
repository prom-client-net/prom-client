using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Prometheus.Client
{
    public class HistogramConfiguration : MetricConfiguration
    {
        private static readonly double[] _defaultBuckets = { .005, .01, .025, .05, .075, .1, .25, .5, .75, 1, 2.5, 5, 7.5, 10 };

        public HistogramConfiguration(string name, string help, IReadOnlyList<string> labels, IReadOnlyList<double> buckets, MetricFlags options)
            : base(name, help, labels, options)
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
