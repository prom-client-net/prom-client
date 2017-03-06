using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Prometheus.Advanced.DataContracts;

[assembly: InternalsVisibleTo("Prometheus.Client.Tests")]

namespace Prometheus.Client.Internal
{
    
    internal class AsciiFormatter
    {
        public static void Format(Stream destination, IEnumerable<MetricFamily> metrics)
        {
            var metricFamilys = metrics.ToArray();
            using (var streamWriter = new StreamWriter(destination, Encoding.UTF8))
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
            streamWriter.WriteLine("# HELP {0} {1}", metricFamily.name, metricFamily.help);
            streamWriter.WriteLine("# TYPE {0} {1}", metricFamily.name, metricFamily.type);
            foreach (var metric in metricFamily.metric)
                WriteMetric(streamWriter, metricFamily, metric);
        }

        private static string WriteFamily(MetricFamily metricFamily)
        {
            var s = new StringBuilder();
            s.AppendLine($"# HELP {metricFamily.name} {metricFamily.help}");
            s.AppendLine($"# TYPE {metricFamily.name} {metricFamily.type}");
            foreach (var metric in metricFamily.metric)
                s.Append(WriteMetric(metricFamily, metric));
            return s.ToString();
        }

        private static void WriteMetric(StreamWriter streamWriter, MetricFamily family, Metric metric)
        {
            var familyName = family.name;

            if (metric.gauge != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.gauge.value, metric.label));
            }
            else if (metric.counter != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.counter.value, metric.label));
            }
            else if (metric.summary != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.summary.sample_sum, metric.label, "_sum"));
                streamWriter.WriteLine(SimpleValue(familyName, metric.summary.sample_count, metric.label, "_count"));

                foreach (var quantileValuePair in metric.summary.quantile)
                {
                    var quantile = double.IsPositiveInfinity(quantileValuePair.quantile) ? "+Inf" : quantileValuePair.quantile.ToString(CultureInfo.InvariantCulture);
                    streamWriter.WriteLine(SimpleValue(familyName, quantileValuePair.value, metric.label.Concat(new[] {new LabelPair {name = "quantile", value = quantile}})));
                }
            }
            else if (metric.histogram != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.histogram.sample_sum, metric.label, "_sum"));
                streamWriter.WriteLine(SimpleValue(familyName, metric.histogram.sample_count, metric.label, "_count"));
                foreach (var bucket in metric.histogram.bucket)
                {
                    var value = double.IsPositiveInfinity(bucket.upper_bound) ? "+Inf" : bucket.upper_bound.ToString(CultureInfo.InvariantCulture);
                    streamWriter.WriteLine(SimpleValue(familyName, bucket.cumulative_count, metric.label.Concat(new[] {new LabelPair {name = "le", value = value}}), "_bucket"));
                }
            }
        }

        private static string WriteMetric(MetricFamily family, Metric metric)
        {
            var s = new StringBuilder();
            var familyName = family.name;

            if (metric.gauge != null)
            {
                s.AppendLine(SimpleValue(familyName, metric.gauge.value, metric.label));
            }
            else if (metric.counter != null)
            {
                s.AppendLine(SimpleValue(familyName, metric.counter.value, metric.label));
            }
            else if (metric.summary != null)
            {
                s.AppendLine(SimpleValue(familyName, metric.summary.sample_sum, metric.label, "_sum"));
                s.AppendLine(SimpleValue(familyName, metric.summary.sample_count, metric.label, "_count"));

                foreach (var quantileValuePair in metric.summary.quantile)
                {
                    var quantile = double.IsPositiveInfinity(quantileValuePair.quantile) ? "+Inf" : quantileValuePair.quantile.ToString(CultureInfo.InvariantCulture);
                    s.AppendLine(SimpleValue(familyName, quantileValuePair.value, metric.label.Concat(new[] {new LabelPair {name = "quantile", value = quantile}})));
                }
            }
            else if (metric.histogram != null)
            {
                s.AppendLine(SimpleValue(familyName, metric.histogram.sample_sum, metric.label, "_sum"));
                s.AppendLine(SimpleValue(familyName, metric.histogram.sample_count, metric.label, "_count"));
                foreach (var bucket in metric.histogram.bucket)
                {
                    var value = double.IsPositiveInfinity(bucket.upper_bound) ? "+Inf" : bucket.upper_bound.ToString(CultureInfo.InvariantCulture);
                    s.AppendLine(SimpleValue(familyName, bucket.cumulative_count, metric.label.Concat(new[] {new LabelPair {name = "le", value = value}}), "_bucket"));
                }
            }

            return s.ToString();
        }

        private static string WithLabels(string familyName, IEnumerable<LabelPair> labels)
        {
            var labelPairs = labels as LabelPair[] ?? labels.ToArray();
            if (labelPairs.Length == 0)
                return familyName;
            return $"{familyName}{{{string.Join(",", labelPairs.Select(l => $"{l.name}=\"{EscapeValue(l.value)}\""))}}}";
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