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

        [Obsolete("'addLegacyMetricNames' will be removed in future versions")]
        public static ICollectorRegistry UseDotNetStats(this ICollectorRegistry registry, bool addLegacyMetricNames)
        {
            return UseDotNetStats(registry, string.Empty, addLegacyMetricNames);
        }

        [Obsolete("'addLegacyMetricNames' will be removed in future versions")]
        public static ICollectorRegistry UseDotNetStats(this ICollectorRegistry registry, string prefixName, bool addLegacyMetricNames)
        {
            registry.Add(new GCCollectionCountCollector(prefixName));
            registry.Add(new GCTotalMemoryCollector(prefixName, addLegacyMetricNames));

            return registry;
        }
    }
}
