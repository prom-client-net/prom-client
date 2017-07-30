using System;
using System.Collections.Generic;
using Prometheus.Client.Collectors;
using Prometheus.Client.SummaryImpl;

namespace Prometheus.Client
{
    /// <summary>
    ///     Metrics creator
    /// </summary>
    public static class Metrics
    {
        private static readonly MetricFactory DefaultFactory = new MetricFactory(CollectorRegistry.Instance);

        /// <summary>
        ///     Create Counter in default MetricFactory
        /// </summary>
        public static Counter CreateCounter(string name, string help, params string[] labelNames)
        {
            return DefaultFactory.CreateCounter(name, help, labelNames);
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
        public static Summary CreateSummary(string name, string help, string[] labelNames, IList<QuantileEpsilonPair> objectives, TimeSpan maxAge, int? ageBuckets, int? bufCap)
        {
            return DefaultFactory.CreateSummary(name, help, labelNames, objectives, maxAge, ageBuckets, bufCap);
        }

        /// <summary>
        ///     Create Histogram in default MetricFactory
        /// </summary>
        public static Histogram CreateHistogram(string name, string help, double[] buckets = null, params string[] labelNames)
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