using System;

namespace Prometheus.Client.Collectors;

public static class CollectorRegistryExtensions
{
    public static void MoveTo(this ICollectorRegistry registry, string collectorName, ICollectorRegistry destination)
    {
        if (destination == null)
            throw new ArgumentNullException(nameof(destination));

        var collector = registry.Remove(collectorName);
        if (collector == null)
            throw new ArgumentOutOfRangeException(nameof(collectorName), collectorName, "Collector does not exist in the source registry");

        destination.Add(collector);
    }

    public static void CopyTo(this ICollectorRegistry registry, string collectorName, ICollectorRegistry destination)
    {
        if (destination == null)
            throw new ArgumentNullException(nameof(destination));

        if (!registry.TryGet(collectorName, out var collector))
        {
            throw new ArgumentOutOfRangeException(nameof(collectorName), collectorName, "Collector does not exist in the source registry");
        }

        destination.Add(collector);
    }
}
