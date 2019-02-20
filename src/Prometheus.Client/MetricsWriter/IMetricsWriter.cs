using Prometheus.Client.Contracts;

namespace Prometheus.Client.MetricsWriter
{
    public interface IMetricsWriter
    {
        IMetricsWriter StartMetric(string metricName);

        IMetricsWriter WriteHelp(string help);

        IMetricsWriter WriteType(MetricType metricType);

        ISampleWriter StartSample(string suffix = "");

        void CloseWriter();
    }
}
