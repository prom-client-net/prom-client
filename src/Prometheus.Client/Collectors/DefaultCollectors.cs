using System.Collections.Generic;
using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Collectors
{
    /// <summary>
    ///     All default Collector
    /// </summary>
    public static class DefaultCollectors
    {
        /// <summary>
        ///     Get default Collector
        /// </summary>
        public static IEnumerable<IOnDemandCollector> Get(MetricFactory metricFactory)
        {
            return new IOnDemandCollector[]
            {
                new DotNetStatsCollector(metricFactory),
                new ProcessCollector(metricFactory),
#if NET45
                new PerfCounterCollector(metricFactory)
#endif
            };
        }
    }
}