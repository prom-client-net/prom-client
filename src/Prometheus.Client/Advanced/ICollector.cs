using Prometheus.Advanced.DataContracts;

namespace Prometheus.Client.Advanced
{
    public interface ICollector
    {
        MetricFamily Collect();

        string Name { get; }

        string[] LabelNames { get; }
    }
}