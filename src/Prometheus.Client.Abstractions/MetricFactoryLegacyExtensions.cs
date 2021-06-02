using System;
using System.Collections.Generic;

namespace Prometheus.Client
{
    public static class MetricFactoryLegacyExtensions
    {
        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<ISummary> CreateSummary(this IMetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateSummary(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<ISummary> CreateSummary(this IMetricFactory factory, string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return factory.CreateSummary(name, help, labelNames, includeTimestamp);
        }

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        public static IMetricFamily<ISummary> CreateSummary(this IMetricFactory factory, string name, string help, string[] labelNames, IReadOnlyList<QuantileEpsilonPair> objectives, TimeSpan maxAge, int? ageBuckets, int? bufCap)
        {
            return factory.CreateSummary(name, help, labelNames, false, objectives, maxAge, ageBuckets, bufCap);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        public static IMetricFamily<ISummary> CreateSummary(
            this IMetricFactory factory,
            string name,
            string help,
            bool includeTimestamp,
            string[] labelNames,
            IReadOnlyList<QuantileEpsilonPair> objectives,
            TimeSpan? maxAge,
            int? ageBuckets,
            int? bufCap)
        {
            return factory.CreateSummary(name, help, labelNames, includeTimestamp, objectives, maxAge, ageBuckets, bufCap);
        }
    }
}
