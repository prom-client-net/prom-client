using System;
using System.Threading.Tasks;

namespace Prometheus.Client.MetricsWriter.Abstractions
{
    public interface IMetricsWriter : IDisposable
    {
        IMetricsWriter StartMetric(string metricName);

        IMetricsWriter WriteHelp(string help);

        IMetricsWriter WriteType(MetricType metricType);

        ISampleWriter StartSample(string suffix = "");

        IMetricsWriter EndMetric();

        Task CloseWriterAsync();

        Task FlushAsync();
    }
}
