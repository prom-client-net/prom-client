using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollector
    {
        string Name { get; }

        string[] LabelNames { get; }

        void Collect(IMetricsWriter writer);
    }
}
