using System;
using Prometheus.Client.Collectors;

namespace Prometheus.Client
{
    /// <summary>
    ///     Static container for DefaultCollectorRegistry and DefaultFactory
    /// </summary>
    public static class Metrics
    {
        private static readonly Lazy<ICollectorRegistry> _defaultCollectorRegistry = new(() => new CollectorRegistry());

        private static readonly Lazy<IMetricFactory> _defaultFactory = new(() => new MetricFactory(DefaultCollectorRegistry));

        public static ICollectorRegistry DefaultCollectorRegistry => _defaultCollectorRegistry.Value;

        public static IMetricFactory DefaultFactory => _defaultFactory.Value;
    }
}
