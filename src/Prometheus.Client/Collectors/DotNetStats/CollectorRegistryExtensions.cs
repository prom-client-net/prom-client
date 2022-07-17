using System;

namespace Prometheus.Client.Collectors.DotNetStats
{
    public static class CollectorRegistryExtensions
    {
        public static ICollectorRegistry UseDotNetStats(this ICollectorRegistry registry)
        {
            return UseDotNetStats(registry, string.Empty);
        }
        public static ICollectorRegistry UseDotNetStats(this ICollectorRegistry registry, string prefixName)
        {
            registry.Add(new GCCollectionCountCollector(prefixName));
            registry.Add(new GCTotalMemoryCollector(prefixName));

            return registry;
        }

        [Obsolete("'addLegacyMetrics' will be removed in future versions")]
        public static ICollectorRegistry UseDotNetStats(this ICollectorRegistry registry, bool addLegacyMetrics)
        {
            return UseDotNetStats(registry, string.Empty, addLegacyMetrics);
        }

        [Obsolete("'addLegacyMetrics' will be removed in future versions")]
        public static ICollectorRegistry UseDotNetStats(this ICollectorRegistry registry, string prefixName, bool addLegacyMetrics)
        {
            registry.Add(new GCCollectionCountCollector(prefixName));
            registry.Add(new GCTotalMemoryCollector(prefixName, addLegacyMetrics));

            return registry;
        }
    }
}
