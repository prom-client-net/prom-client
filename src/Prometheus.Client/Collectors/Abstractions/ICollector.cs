using System.Collections.Generic;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollector
    {
        ICollectorConfiguration Configuration { get; }

        IReadOnlyList<string> MetricNames { get; }

        void Collect(IMetricsWriter writer);
    }
}
