using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Prometheus.Client.Contracts;

[assembly: InternalsVisibleTo("Prometheus.Client.Tests")]

namespace Prometheus.Client.Internal
{
    internal static class TextFormatter
    {
        private static readonly Encoding _encoding = new UTF8Encoding(false);

        public static void Format(Stream destination, IEnumerable<MetricFamily> metrics)
        {
            var metricFamilys = metrics.ToArray();
            using (var streamWriter = new StreamWriter(destination, _encoding))
            {
                streamWriter.NewLine = "\n";
                foreach (var metricFamily in metricFamilys)
                    WriteFamily(streamWriter, metricFamily);
            }
        }

        internal static string Format(IEnumerable<MetricFamily> metrics)
        {
            var metricFamilys = metrics.ToArray();
            var s = new StringBuilder();
            foreach (var metricFamily in metricFamilys)
                s.Append(WriteFamily(metricFamily));

            return s.ToString();
        }

        private static void WriteFamily(StreamWriter streamWriter, MetricFamily metricFamily)
        {
            streamWriter.WriteLine("# HELP {0} {1}", metricFamily.Name, metricFamily.Help);
            streamWriter.WriteLine("# TYPE {0} {1}", metricFamily.Name, metricFamily.Type);
            foreach (var metric in metricFamily.Metrics)
                WriteMetric(streamWriter, metricFamily, metric);
        }

        private static string WriteFamily(MetricFamily metricFamily)
        {
            var s = new StringBuilder();
            s.AppendLine($"# HELP {metricFamily.Name} {metricFamily.Help}");
            s.AppendLine($"# TYPE {metricFamily.Name} {metricFamily.Type}");
            foreach (var metric in metricFamily.Metrics)
                s.Append(WriteMetric(metricFamily, metric));
            return s.ToString();
        }

        private static void WriteMetric(StreamWriter streamWriter, MetricFamily family, Metric metric)
        {
            var familyName = family.Name;

            if (metric.Gauge != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.Gauge.Value, metric.Labels));
            }
            else if (metric.Counter != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.Counter.Value, metric.Labels));
            }
            else if (metric.Summary != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.Summary.SampleSum, metric.Labels, "_sum"));
                streamWriter.WriteLine(SimpleValue(familyName, metric.Summary.SampleCount, metric.Labels, "_count"));

                foreach (var quantileValuePair in metric.Summary.Quantiles)
                {
                    var quantile = double.IsPositiveInfinity(quantileValuePair.quantile) ? "+Inf" : quantileValuePair.quantile.ToString(CultureInfo.InvariantCulture);
                    streamWriter.WriteLine(SimpleValue(familyName, quantileValuePair.Value, metric.Labels.Concat(new[] {new LabelPair {Name = "quantile", Value = quantile}})));
                }
            }
            else if (metric.Histogram != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.Histogram.SampleSum, metric.Labels, "_sum"));
                streamWriter.WriteLine(SimpleValue(familyName, metric.Histogram.SampleCount, metric.Labels, "_count"));
                foreach (var bucket in metric.Histogram.Buckets)
                {
                    var value = double.IsPositiveInfinity(bucket.UpperBound) ? "+Inf" : bucket.UpperBound.ToString(CultureInfo.InvariantCulture);
                    streamWriter.WriteLine(SimpleValue(familyName, bucket.CumulativeCount, metric.Labels.Concat(new[] {new LabelPair {Name = "le", Value = value}}), "_bucket"));
                }
            }
        }

        private static string WriteMetric(MetricFamily family, Metric metric)
        {
            var s = new StringBuilder();
            var familyName = family.Name;

            if (metric.Gauge != null)
            {
                s.AppendLine(SimpleValue(familyName, metric.Gauge.Value, metric.Labels));
            }
            else if (metric.Counter != null)
            {
                s.AppendLine(SimpleValue(familyName, metric.Counter.Value, metric.Labels));
            }
            else if (metric.Summary != null)
            {
                s.AppendLine(SimpleValue(familyName, metric.Summary.SampleSum, metric.Labels, "_sum"));
                s.AppendLine(SimpleValue(familyName, metric.Summary.SampleCount, metric.Labels, "_count"));

                foreach (var quantileValuePair in metric.Summary.Quantiles)
                {
                    var quantile = double.IsPositiveInfinity(quantileValuePair.quantile) ? "+Inf" : quantileValuePair.quantile.ToString(CultureInfo.InvariantCulture);
                    s.AppendLine(SimpleValue(familyName, quantileValuePair.Value, metric.Labels.Concat(new[] {new LabelPair {Name = "quantile", Value = quantile}})));
                }
            }
            else if (metric.Histogram != null)
            {
                s.AppendLine(SimpleValue(familyName, metric.Histogram.SampleSum, metric.Labels, "_sum"));
                s.AppendLine(SimpleValue(familyName, metric.Histogram.SampleCount, metric.Labels, "_count"));
                foreach (var bucket in metric.Histogram.Buckets)
                {
                    var value = double.IsPositiveInfinity(bucket.UpperBound) ? "+Inf" : bucket.UpperBound.ToString(CultureInfo.InvariantCulture);
                    s.AppendLine(SimpleValue(familyName, bucket.CumulativeCount, metric.Labels.Concat(new[] {new LabelPair {Name = "le", Value = value}}), "_bucket"));
                }
            }

            return s.ToString();
        }

        private static string WithLabels(string familyName, IEnumerable<LabelPair> labels)
        {
            var labelPairs = labels as LabelPair[] ?? labels.ToArray();
            if (labelPairs.Length == 0)
                return familyName;
            return $"{familyName}{{{string.Join(",", labelPairs.Select(l => $"{l.Name}=\"{EscapeValue(l.Value)}\""))}}}";
        }

        private static string EscapeValue(string val)
        {
            return val.Replace("\\", @"\\").Replace("\n", @"\n").Replace("\"", @"\""");
        }

        private static string SimpleValue(string family, double value, IEnumerable<LabelPair> labels, string namePostfix = null)
        {
            return $"{WithLabels(family + (namePostfix ?? ""), labels)} {value.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}