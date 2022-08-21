using System;
using Prometheus.Client.Collectors.DotNetStats;
using Prometheus.Client.Collectors.ProcessStats;

namespace Prometheus.Client.Collectors
{
    public static class DefaultCollectors
    {
        public static ICollectorRegistry UseDefaultCollectors(this ICollectorRegistry registry)
        {
            return UseDefaultCollectors(registry, string.Empty);
        }

        public static ICollectorRegistry UseDefaultCollectors(this ICollectorRegistry registry, string prefixName)
        {
            registry.UseDotNetStats(prefixName);
            registry.UseProcessStats(prefixName);

            return registry;
        }

        [Obsolete("'addLegacyMetrics' will be removed in future versions")]
        public static ICollectorRegistry UseDefaultCollectors(this ICollectorRegistry registry, bool addLegacyMetrics)
        {
            return UseDefaultCollectors(registry, string.Empty, addLegacyMetrics);
        }

        [Obsolete("'addLegacyMetrics' will be removed in future versions")]
        public static ICollectorRegistry UseDefaultCollectors(this ICollectorRegistry registry, string prefixName, bool addLegacyMetrics)
        {
            registry.UseDotNetStats(prefixName, addLegacyMetrics);
            registry.UseProcessStats(prefixName, addLegacyMetrics);

            return registry;
        }
    }
}
