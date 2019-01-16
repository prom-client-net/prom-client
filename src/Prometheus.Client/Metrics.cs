using System;
using System.Collections.Generic;
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
        public static readonly MetricFactory DefaultFactory = new MetricFactory(CollectorRegistry.Instance);

        /// <summary>
        ///     Create Counter
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="labelNames">Array of label names</param>
        public static Counter CreateCounter(string name, string help, params string[] labelNames)
        {
            return DefaultFactory.CreateCounter(name, help, labelNames);
        }
        
        /// <summary>
        ///     Create  Counter
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric</param>
        /// <param name="labelNames">Array of label names</param>
        public static Counter CreateCounter(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return DefaultFactory.CreateCounter(name, help, includeTimestamp, labelNames);
        }

        /// <summary>
        ///     Create Gauge in default MetricFactory
        /// </summary>
        public static Gauge CreateGauge(string name, string help, params string[] labelNames)
        {
            return DefaultFactory.CreateGauge(name, help, labelNames);
        }

        /// <summary>
        ///     Create Summary in default MetricFactory
        /// </summary>
        public static Summary CreateSummary(string name, string help, params string[] labelNames)
        {
            return DefaultFactory.CreateSummary(name, help, labelNames);
        }

        /// <summary>
        ///     Create Summary in default MetricFactory
        /// </summary>
        public static Summary CreateSummary(string name, string help, string[] labelNames, IList<QuantileEpsilonPair> objectives, TimeSpan maxAge,
            int? ageBuckets, int? bufCap)
        {
            return DefaultFactory.CreateSummary(name, help, labelNames, objectives, maxAge, ageBuckets, bufCap);
        }

        /// <summary>
        ///     Create Histogram in default MetricFactory
        /// </summary>
        public static Histogram CreateHistogram(string name, string help, params string[] labelNames)
        {
            return CreateHistogram(name, help, null, labelNames);
        }

        /// <summary>
        ///     Create Histogram in default MetricFactory
        /// </summary>
        public static Histogram CreateHistogram(string name, string help, double[] buckets, params string[] labelNames)
        {
            return DefaultFactory.CreateHistogram(name, help, buckets, labelNames);
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
