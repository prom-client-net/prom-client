using Prometheus.Contracts;

namespace Prometheus.Client.Collectors
{
    public interface ICollector
    {
        MetricFamily Collect();

        string Name { get; }

        string[] LabelNames { get; }
    }
}