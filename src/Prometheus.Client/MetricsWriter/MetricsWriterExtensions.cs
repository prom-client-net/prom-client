using System.Collections.Generic;

namespace Prometheus.Client.MetricsWriter
{
    public static class MetricsWriterExtensions
    {
        public static IMetricsWriter WriteSample(
            this IMetricsWriter writer,
            double value,
            string suffix = "",
            KeyValuePair<string, string>[] labels = null,
            long? timestamp = null)
        {
            var sampleWriter = writer.StartSample(suffix);
            if ((labels != null) && (labels.Length > 0))
            {
                var labelWriter = sampleWriter.StartLabels();
                labelWriter.WriteLabels(labels);
                labelWriter.EndLabels();
            }

            sampleWriter.WriteValue(value);
            if (timestamp.HasValue)
                sampleWriter.WriteTimestamp(timestamp.Value);

            return writer;
        }

        public static IMetricsWriter WriteMetricHeader(
            this IMetricsWriter writer,
            string metricName,
            MetricType metricType,
            string help = "")
        {
            writer.StartMetric(metricName);
            if (!string.IsNullOrEmpty(help))
                writer.WriteHelp(help);

            writer.WriteType(metricType);
            return writer;
        }

        public static ILabelWriter WriteLabels(
            this ILabelWriter labelWriter,
            KeyValuePair<string, string>[] labels
        )
        {
            for (int i = 0; i < labels.Length; i++)
                labelWriter.WriteLabel(labels[i].Key, labels[i].Value);

            return labelWriter;
        }
    }
}
