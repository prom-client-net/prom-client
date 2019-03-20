using System.Collections.Generic;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollector
    {
        void Collect(IMetricsWriter writer);

        IReadOnlyList<string> MetricNames { get; }
    }
}
