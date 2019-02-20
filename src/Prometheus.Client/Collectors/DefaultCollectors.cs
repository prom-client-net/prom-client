using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.Collectors.DotNetStats;
using Prometheus.Client.Collectors.ProcessStats;
#if NET45
using Prometheus.Client.Collectors.PerfCounters;
#endif

namespace Prometheus.Client.Collectors
{
    public static class DefaultCollectors
    {
        public static ICollectorRegistry UseDefaultCollectors(this ICollectorRegistry registry)
        {
            registry.UseDotNetStats();
            registry.UseProcessStats();
#if NET45
            registry.UsePerfCounters();
#endif

            return registry;
        }
    }
}
