using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollector
    {
        void Collect(IMetricsWriter writer);

        string Name { get; }

        string[] LabelNames { get; }
    }
}
