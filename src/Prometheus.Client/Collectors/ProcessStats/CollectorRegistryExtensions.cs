using System.Diagnostics;
using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Collectors.ProcessStats
{
    public static class CollectorRegistryExtensions
    {
        public static ICollectorRegistry UseProcessStats(this ICollectorRegistry registry)
        {
            registry.Add(nameof(ProcessCollector), new ProcessCollector(Process.GetCurrentProcess()));

            return registry;
        }
    }
}
