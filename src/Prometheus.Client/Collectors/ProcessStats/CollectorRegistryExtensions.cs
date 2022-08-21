using System;
using System.Diagnostics;

namespace Prometheus.Client.Collectors.ProcessStats
{
    public static class CollectorRegistryExtensions
    {
        public static ICollectorRegistry UseProcessStats(this ICollectorRegistry registry)
        {
            return UseProcessStats(registry, string.Empty);
        }

        public static ICollectorRegistry UseProcessStats(this ICollectorRegistry registry, string prefixName)
        {
            registry.Add(new ProcessCollector(Process.GetCurrentProcess(), prefixName));

            return registry;
        }

        [Obsolete("'addLegacyMetrics' will be removed in future versions")]
        public static ICollectorRegistry UseProcessStats(this ICollectorRegistry registry, bool addLegacyMetrics)
        {
            return UseProcessStats(registry, string.Empty, addLegacyMetrics);
        }

        [Obsolete("'addLegacyMetrics' will be removed in future versions")]
        public static ICollectorRegistry UseProcessStats(this ICollectorRegistry registry, string prefixName, bool addLegacyMetrics)
        {
            registry.Add(new ProcessCollector(Process.GetCurrentProcess(), prefixName, addLegacyMetrics));

            return registry;
        }
    }
}
