namespace Prometheus.Client.Collectors.DotNetStats
{
    public static class CollectorRegistryExtensions
    {
        public static ICollectorRegistry UseDotNetStats(this ICollectorRegistry registry, bool addLegacyMetricNames = false)
        {
            return UseDotNetStats(registry, string.Empty, addLegacyMetricNames);
        }

        public static ICollectorRegistry UseDotNetStats(this ICollectorRegistry registry, string prefixName, bool addLegacyMetricNames = false)
        {
            registry.Add(new GCCollectionCountCollector(prefixName));
            registry.Add(new GCTotalMemoryCollector(prefixName, addLegacyMetricNames));

            return registry;
        }
    }
}
