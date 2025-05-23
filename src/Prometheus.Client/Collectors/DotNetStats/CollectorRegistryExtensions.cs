using System;

namespace Prometheus.Client.Collectors.DotNetStats;

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
}
