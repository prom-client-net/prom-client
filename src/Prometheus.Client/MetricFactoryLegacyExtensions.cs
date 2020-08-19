using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client
{
    public static class MetricFactoryLegacyExtensions
    {
        /// <summary>
        ///     Create Counter.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<ICounter> CreateCounter(this IMetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateCounter(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create int-based counter.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<ICounter<long>> CreateCounterInt64(this IMetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateCounterInt64(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IGauge> CreateGauge(this IMetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateGauge(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create int-based gauge.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IGauge<long>> CreateGaugeInt64(this IMetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateGaugeInt64(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IHistogram> CreateHistogram(this IMetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateHistogram(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IHistogram> CreateHistogram(this IMetricFactory factory, string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return factory.CreateHistogram(name, help, includeTimestamp, null, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IHistogram> CreateHistogram(this IMetricFactory factory, string name, string help, double[] buckets, params string[] labelNames)
        {
            return factory.CreateHistogram(name, help, false, buckets, labelNames);
        }

        /// <summary>
        ///     Create Untyped
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IUntyped> CreateUntyped(this IMetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateUntyped(name, help, false, labelNames);
        }

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
