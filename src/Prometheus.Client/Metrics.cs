using System;
using System.Collections.Generic;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.SummaryImpl;

namespace Prometheus.Client
{
    /// <summary>
    ///     Metrics creator
    /// </summary>
    public static class Metrics
    {
        public static readonly ICollectorRegistry DefaultCollectorRegistry = new CollectorRegistry();

        public static readonly MetricFactory DefaultFactory = new MetricFactory(DefaultCollectorRegistry);

        /// <summary>
        ///     Create Counter
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<ICounter> CreateCounter(string name, string help, params string[] labelNames)
        {
            return DefaultFactory.CreateCounter(name, help, labelNames);
        }

        /// <summary>
        ///     Create  Counter
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<ICounter> CreateCounter(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return DefaultFactory.CreateCounter(name, help, includeTimestamp, labelNames);
        }

        /// <summary>
        ///     Create int-based counter
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, params string[] labelNames)
        {
            return DefaultFactory.CreateCounterInt64(name, help, labelNames);
        }

        /// <summary>
        ///     Create int-based counter
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return DefaultFactory.CreateCounterInt64(name, help, includeTimestamp, labelNames);
        }

        /// <summary>
        ///     Create Gauge
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IGauge> CreateGauge(string name, string help, params string[] labelNames)
        {
            return DefaultFactory.CreateGauge(name, help, labelNames);
        }

        /// <summary>
        ///     Create Gauge
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IGauge> CreateGauge(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return DefaultFactory.CreateGauge(name, help, includeTimestamp, labelNames);
        }

        /// <summary>
        ///     Create Untyped
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IUntyped> CreateUntyped(string name, string help, params string[] labelNames)
        {
            return DefaultFactory.CreateUntyped(name, help, labelNames);
        }

        /// <summary>
        ///     Create Untyped
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IUntyped> CreateUntyped(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return DefaultFactory.CreateUntyped(name, help, includeTimestamp, labelNames);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<ISummary> CreateSummary(string name, string help, params string[] labelNames)
        {
            return DefaultFactory.CreateSummary(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<ISummary> CreateSummary(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return DefaultFactory.CreateSummary(name, help, includeTimestamp, labelNames, null, null, null, null);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge">.</param>
        /// <param name="ageBuckets">.</param>
        /// <param name="bufCap">.</param>
        public static IMetricFamily<ISummary> CreateSummary(
            string name,
            string help,
            string[] labelNames,
            IReadOnlyList<QuantileEpsilonPair> objectives,
            TimeSpan maxAge,
            int? ageBuckets,
            int? bufCap)
        {
            return DefaultFactory.CreateSummary(name, help, false, labelNames, objectives, maxAge, ageBuckets, bufCap);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge">.</param>
        /// <param name="ageBuckets">.</param>
        /// <param name="bufCap">.</param>
        public static IMetricFamily<ISummary> CreateSummary(
            string name,
            string help,
            bool includeTimestamp,
            string[] labelNames,
            IReadOnlyList<QuantileEpsilonPair> objectives,
            TimeSpan? maxAge,
            int? ageBuckets,
            int? bufCap)
        {
            return DefaultFactory.CreateSummary(name, help, includeTimestamp, labelNames, objectives, maxAge, ageBuckets, bufCap);
        }

        /// <summary>
        ///     Create Histogram
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IHistogram> CreateHistogram(string name, string help, params string[] labelNames)
        {
            return DefaultFactory.CreateHistogram(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Histogram
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return DefaultFactory.CreateHistogram(name, help, includeTimestamp, null, labelNames);
        }

        /// <summary>
        ///     Create Histogram
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IHistogram> CreateHistogram(string name, string help, double[] buckets, params string[] labelNames)
        {
            return DefaultFactory.CreateHistogram(name, help, false, buckets, labelNames);
        }

        /// <summary>
        ///     Create Histogram
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Array of label names.</param>
        public static IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp, double[] buckets, params string[] labelNames)
        {
            return DefaultFactory.CreateHistogram(name, help, includeTimestamp, buckets, labelNames);
        }

        /// <summary>
        ///     Create MetricFactory with custom Registry
        /// </summary>
        public static MetricFactory WithCustomRegistry(ICollectorRegistry registry)
        {
            return new MetricFactory(registry);
        }
    }
}
