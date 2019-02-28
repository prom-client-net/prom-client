using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Collectors.DotNetStats
{
    public static class CollectorRegistryExtensions
    {
        public static ICollectorRegistry UseDotNetStats(this ICollectorRegistry registry)
        {
            registry.Add(nameof(GCCollectionCountCollector), new GCCollectionCountCollector());
            registry.Add(nameof(GCTotalMemoryCollector), new GCTotalMemoryCollector());

            return registry;
        }
    }
}
