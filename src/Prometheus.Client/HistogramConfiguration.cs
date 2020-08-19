using System;
using System.Globalization;
using System.Linq;

namespace Prometheus.Client
{
    public class HistogramConfiguration : MetricConfiguration
    {
        private static readonly double[] _defaultBuckets = { .005, .01, .025, .05, .075, .1, .25, .5, .75, 1, 2.5, 5, 7.5, 10 };
        private static readonly string[] _defaultFormattedBuckets;

        private readonly Lazy<string[]> _formattedBuckets;

        static HistogramConfiguration()
        {
            _defaultFormattedBuckets = GetFormattedBuckets(_defaultBuckets);
        }

        public HistogramConfiguration(string name, string help, string[] labels, double[] buckets, bool includeTimestamp)
            : base(name, help, labels, includeTimestamp)
        {
            if (LabelNames.Any(l => l == "le"))
                throw new ArgumentException("'le' is a reserved label name");

            if (buckets == null)
            {
                Buckets = _defaultBuckets;
                _formattedBuckets = new Lazy<string[]>(() => _defaultFormattedBuckets);
            }
            else
            {
                Buckets = buckets;

                if (Buckets.Length == 0)
                    throw new ArgumentException("Histogram must have at least one bucket");

                var lastVal = double.MinValue;
                foreach (var val in buckets)
                {
                    if (lastVal >= val)
                        throw new ArgumentException("Bucket values must be increasing");

                    lastVal = val;
                }

                _formattedBuckets = new Lazy<string[]>(() => GetFormattedBuckets(Buckets));
            }
        }

        public double[] Buckets { get; }

        internal string[] FormattedBuckets => _formattedBuckets.Value;

        private static string[] GetFormattedBuckets(double[] buckets)
        {
            var result = new string[buckets.Length + 1];
            for (var i = 0; i < buckets.Length; i++)
            {
                result[i] = buckets[i].ToString(CultureInfo.InvariantCulture);
            }

            result[buckets.Length] = "+Inf";
            return result;
        }
    }
}
