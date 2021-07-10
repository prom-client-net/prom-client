using System.Diagnostics;

namespace Prometheus.Client.Collectors.ProcessStats
{
    public static class CollectorRegistryExtensions
    {
        public static ICollectorRegistry UseProcessStats(this ICollectorRegistry registry, string prefixName = "")
        {
            registry.Add(new ProcessCollector(Process.GetCurrentProcess(), prefixName));

            return registry;
        }
    }
}
