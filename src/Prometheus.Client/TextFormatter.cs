using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Prometheus.Client.Contracts;

namespace Prometheus.Client
{
    public static class TextFormatter
    {
        private static readonly Encoding _encoding = new UTF8Encoding(false);

        public static void Format(Stream destination, CMetricFamily[] metrics)
        {
            var metricFamilys = metrics.ToArray();
            using (var streamWriter = new StreamWriter(destination, _encoding, 1024, true))
            {
                streamWriter.NewLine = "\n";
                foreach (var metricFamily in metricFamilys)
                    WriteFamily(streamWriter, metricFamily);
            }
        }

        private static void WriteFamily(StreamWriter streamWriter, CMetricFamily cMetricFamily)
        {
            streamWriter.WriteLine("# HELP {0} {1}", cMetricFamily.Name, cMetricFamily.Help);
            streamWriter.WriteLine("# TYPE {0} {1}", cMetricFamily.Name, cMetricFamily.Type);
            foreach (var metric in cMetricFamily.Metrics)
                WriteMetric(streamWriter, cMetricFamily, metric);
        }

        private static void WriteMetric(StreamWriter streamWriter, CMetricFamily family, CMetric cMetric)
        {
            var familyName = family.Name;

            if (cMetric.CGauge != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, cMetric.CGauge.Value, cMetric.Labels));
            }
            else if (cMetric.CCounter != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, cMetric.CCounter.Value, cMetric.Labels));
            }
            else if (cMetric.CSummary != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, cMetric.CSummary.SampleSum, cMetric.Labels, "_sum"));
                streamWriter.WriteLine(SimpleValue(familyName, cMetric.CSummary.SampleCount, cMetric.Labels, "_count"));

                foreach (var quantileValuePair in cMetric.CSummary.Quantiles)
                {
                    var quantile = double.IsPositiveInfinity(quantileValuePair.Quantile)
                        ? "+Inf"
                        : quantileValuePair.Quantile.ToString(CultureInfo.InvariantCulture);
                    streamWriter.WriteLine(SimpleValue(familyName, quantileValuePair.Value,
                        cMetric.Labels.Concat(new[] { new CLabelPair { Name = "quantile", Value = quantile } })));
                }
            }
            else if (cMetric.CHistogram != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, cMetric.CHistogram.SampleSum, cMetric.Labels, "_sum"));
                streamWriter.WriteLine(SimpleValue(familyName, cMetric.CHistogram.SampleCount, cMetric.Labels, "_count"));
                foreach (var bucket in cMetric.CHistogram.Buckets)
                {
                    var value = double.IsPositiveInfinity(bucket.UpperBound) ? "+Inf" : bucket.UpperBound.ToString(CultureInfo.InvariantCulture);
                    streamWriter.WriteLine(SimpleValue(familyName, bucket.CumulativeCount,
                        cMetric.Labels.Concat(new[] { new CLabelPair { Name = "le", Value = value } }), "_bucket"));
                }
            }
        }

        private static string WithLabels(string familyName, IEnumerable<CLabelPair> labels)
        {
            var labelPairs = labels as CLabelPair[] ?? labels.ToArray();
            return labelPairs.Length == 0 ? familyName : $"{familyName}{{{string.Join(",", labelPairs.Select(l => $"{l.Name}=\"{EscapeValue(l.Value)}\""))}}}";
        }

        private static string EscapeValue(string val)
        {
            return val.Replace("\\", @"\\").Replace("\n", @"\n").Replace("\"", @"\""");
        }

        private static string SimpleValue(string family, double value, IEnumerable<CLabelPair> labels, string namePostfix = null)
        {
            return $"{WithLabels(family + (namePostfix ?? ""), labels)} {value.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}