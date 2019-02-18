using Prometheus.Client.Contracts;

namespace Prometheus.Client.MetricsWriter
{
    public interface IMetricsWriter
    {
        IMetricsWriter StartMetric(string metricName);

        IMetricsWriter WriteHelp(string help);

        IMetricsWriter WriteType(CMetricType metricType);

        ISampleWriter StartSample(string suffix = "");

        void CloseWriter();
    }
}
