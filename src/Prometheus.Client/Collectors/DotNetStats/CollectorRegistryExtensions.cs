namespace Prometheus.Client.Collectors.DotNetStats
{
    public static class CollectorRegistryExtensions
    {
        public static ICollectorRegistry UseDotNetStats(this ICollectorRegistry registry)
        {
            registry.Add(new GCCollectionCountCollector());
            registry.Add(new GCTotalMemoryCollector());

            return registry;
        }
    }
}
