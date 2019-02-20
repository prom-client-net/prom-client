using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.Collectors.DotNetStats;
using Prometheus.Client.Collectors.ProcessStats;

namespace Prometheus.Client.Collectors
{
    public static class DefaultCollectors
    {
        public static ICollectorRegistry UseDefaultCollectors(this ICollectorRegistry registry)
        {
            registry.UseDotNetStats();
            registry.UseProcessStats();

            return registry;
        }
    }
}
