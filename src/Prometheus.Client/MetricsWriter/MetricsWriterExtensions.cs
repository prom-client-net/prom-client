using System;
using System.Collections.Generic;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client.MetricsWriter
{
    public static class MetricsWriterExtensions
    {
        public static IMetricsWriter WriteSample(
            this IMetricsWriter writer,
            double value,
            string suffix = "",
            IReadOnlyList<string> labelNames = null,
            IReadOnlyList<string> labelValues = null,
            long? timestamp = null)
        {
            var sampleWriter = writer.StartSample(suffix);
            if ((labelValues != null) && (labelValues.Count > 0))
            {
                var labelWriter = sampleWriter.StartLabels();
                labelWriter.WriteLabels(labelNames, labelValues);
                labelWriter.EndLabels();
            }

            sampleWriter.WriteValue(value);
            if (timestamp.HasValue)
                sampleWriter.WriteTimestamp(timestamp.Value);

            sampleWriter.EndSample();
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
            IReadOnlyList<string> labelNames,
            IReadOnlyList<string> labelValues
        )
        {
            if(labelNames == null)
                throw new ArgumentNullException(nameof(labelNames));

            if(labelValues == null)
                throw new ArgumentNullException(nameof(labelValues));

            if (labelNames.Count != labelValues.Count)
            {
                throw new InvalidOperationException("Label names and values does not match");
            }

            for (int i = 0; i < labelNames.Count; i++)
                labelWriter.WriteLabel(labelNames[i], labelValues[i]);

            return labelWriter;
        }
    }
}
