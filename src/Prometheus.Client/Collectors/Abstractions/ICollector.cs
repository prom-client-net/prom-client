using Prometheus.Client.Contracts;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollector
    {
        CMetricFamily Collect();

        string Name { get; }

        string[] LabelNames { get; }
    }
}