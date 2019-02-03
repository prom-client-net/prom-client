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
            using (var streamWriter = new StreamWriter(destination, _encoding, 1024, true))
            {
                streamWriter.NewLine = "\n";
                foreach (var metricFamily in metrics)
                    WriteFamily(streamWriter, metricFamily);
            }
        }

        private static void WriteFamily(StreamWriter streamWriter, CMetricFamily metricFamily)
        {
            // # HELP
            streamWriter.Write("# HELP ");
            streamWriter.Write(metricFamily.Name);
            streamWriter.Write(" ");
            streamWriter.WriteLine(metricFamily.Help);

            // # TYPE
            streamWriter.Write("# TYPE ");
            streamWriter.Write(metricFamily.Name);
            streamWriter.Write(" ");
            streamWriter.WriteLine(metricFamily.Type.ToString().ToLowerInvariant());

            foreach (var metric in metricFamily.Metrics)
                WriteMetric(streamWriter, metricFamily, metric);
        }

        private static void WriteMetric(StreamWriter streamWriter, CMetricFamily family, CMetric metric)
        {
            var familyName = family.Name;

            if (metric.CGauge != null)
            {
                WriteMetricValue(streamWriter, familyName, null, metric.CGauge.Value, metric.Timestamp, metric.Labels);
            }
            else if (metric.CCounter != null)
            {
                WriteMetricValue(streamWriter, familyName, null, metric.CCounter.Value, metric.Timestamp, metric.Labels);
            }
            else if (metric.CSummary != null)
            {
                WriteMetricValue(streamWriter, familyName, "_sum", metric.CSummary.SampleSum, metric.Timestamp, metric.Labels);
                WriteMetricValue(streamWriter, familyName, "_count", metric.CSummary.SampleCount, metric.Timestamp, metric.Labels);

                foreach (var quantileValuePair in metric.CSummary.Quantiles)
                {
                    var quantile = double.IsPositiveInfinity(quantileValuePair.Quantile) ? "+Inf" : quantileValuePair.Quantile.ToString(CultureInfo.InvariantCulture);

                    var quantileLabels = metric.Labels.Concat(new[]
                    {
                        new CLabelPair { Name = "quantile", Value = quantile }
                    }).ToArray();

                    WriteMetricValue(streamWriter, familyName, null, quantileValuePair.Value, metric.Timestamp, quantileLabels);
                }
            }
            else if (metric.CHistogram != null)
            {
                WriteMetricValue(streamWriter, familyName, "_sum", metric.CHistogram.SampleSum, metric.Timestamp, metric.Labels);
                WriteMetricValue(streamWriter, familyName, "_count", metric.CHistogram.SampleCount, metric.Timestamp, metric.Labels);

                foreach (var bucket in metric.CHistogram.Buckets)
                {
                    var value = double.IsPositiveInfinity(bucket.UpperBound) ? "+Inf" : bucket.UpperBound.ToString(CultureInfo.InvariantCulture);

                    var bucketLabels = metric.Labels.Concat(new[]
                    {
                        new CLabelPair { Name = "le", Value = value }
                    }).ToArray();

                    WriteMetricValue(streamWriter, familyName, "_bucket", bucket.CumulativeCount, metric.Timestamp, bucketLabels);
                }
            }
            else if (metric.CUntyped != null)
            {
                WriteMetricValue(streamWriter, familyName, null, metric.CUntyped.Value, metric.Timestamp, metric.Labels);
            }
        }

        private static void WriteMetricValue(StreamWriter writer, string familyName, string postfix, double value, long? timestamp, CLabelPair[] labels)
        {
            writer.Write(familyName);

            if (postfix != null)
                writer.Write(postfix);

            if (labels?.Any() == true)
            {
                writer.Write('{');

                bool firstLabel = true;
                foreach (var label in labels)
                {
                    if (!firstLabel)
                        writer.Write(',');

                    firstLabel = false;

                    writer.Write(label.Name);
                    writer.Write("=\"");
                    writer.Write(EscapeValue(label.Value));

                    writer.Write('"');
                }

                writer.Write('}');
            }

            writer.Write(' ');
            writer.Write(value.ToString(CultureInfo.InvariantCulture));

            if (timestamp.HasValue)
            {
                writer.Write(' ');
                writer.Write(timestamp.Value);
            }

            writer.WriteLine();
        }


        private static string EscapeValue(string val)
        {
            return val
                .Replace("\\", @"\\")
                .Replace("\n", @"\n")
                .Replace("\"", @"\""");
        }
    }
}
