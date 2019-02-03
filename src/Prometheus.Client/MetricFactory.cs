using System;
using System.Collections.Generic;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.SummaryImpl;

namespace Prometheus.Client
{
    public class MetricFactory
    {
        private readonly ICollectorRegistry _registry;

        public MetricFactory(ICollectorRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        ///     Create Counter
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="labelNames">Array of label names</param>
        public Counter CreateCounter(string name, string help, params string[] labelNames)
        {
            return CreateCounter(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create  Counter
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric</param>
        /// <param name="labelNames">Array of label names</param>
        public Counter CreateCounter(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            var metric = new Counter(name, help, includeTimestamp, labelNames);
            return (Counter) _registry.GetOrAdd(metric);
        }

        /// <summary>
        ///     Create Gauge
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="labelNames">Array of label names</param>
        public Gauge CreateGauge(string name, string help, params string[] labelNames)
        {
            return CreateGauge(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Gauge
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric</param>
        /// <param name="labelNames">Array of label names</param>
        public Gauge CreateGauge(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            var metric = new Gauge(name, help, includeTimestamp, labelNames);
            return (Gauge) _registry.GetOrAdd(metric);
        }
        
        /// <summary>
        ///     Create Untyped
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="labelNames">Array of label names</param>
        public Untyped CreateUntyped(string name, string help, params string[] labelNames)
        {
            return CreateUntyped(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Untyped
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric</param>
        /// <param name="labelNames">Array of label names</param>
        public Untyped CreateUntyped(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            var metric = new Untyped(name, help, includeTimestamp, labelNames);
            return (Untyped) _registry.GetOrAdd(metric);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="labelNames">Array of label names</param>
        public Summary CreateSummary(string name, string help, params string[] labelNames)
        {
            return CreateSummary(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric</param>
        /// <param name="labelNames">Array of label names</param>
        public Summary CreateSummary(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return CreateSummary(name, help, includeTimestamp, labelNames, null, null, null, null);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="labelNames">Array of label names</param>
        /// <param name="objectives"></param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        public Summary CreateSummary(string name, string help, string[] labelNames, IList<QuantileEpsilonPair> objectives, TimeSpan maxAge, int? ageBuckets, int? bufCap)
        {
            return CreateSummary(name, help, false, labelNames, objectives, maxAge, ageBuckets, bufCap);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric</param>
        /// <param name="labelNames">Array of label names</param>
        /// <param name="objectives"></param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        public Summary CreateSummary(string name, string help, bool includeTimestamp, string[] labelNames, IList<QuantileEpsilonPair> objectives, TimeSpan? maxAge, int? ageBuckets,
            int? bufCap)
        {
            var metric = new Summary(name, help, includeTimestamp, labelNames, objectives, maxAge, ageBuckets, bufCap);
            return (Summary) _registry.GetOrAdd(metric);
        }

        
        /// <summary>
        ///     Create Histogram
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="labelNames">Array of label names</param>
        public Histogram CreateHistogram(string name, string help, params string[] labelNames)
        {
            return CreateHistogram(name, help, false, labelNames);
        }

        
        /// <summary>
        ///     Create Histogram
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric</param>
        /// <param name="labelNames">Array of label names</param>
        public Histogram CreateHistogram(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return CreateHistogram(name, help, includeTimestamp, null, labelNames);
        }

        /// <summary>
        ///     Create Histogram
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="buckets">Buckets</param>
        /// <param name="labelNames">Array of label names</param>
        public Histogram CreateHistogram(string name, string help, double[] buckets, params string[] labelNames)
        {
            return CreateHistogram(name, help, false, buckets, labelNames);
        }

        /// <summary>
        ///     Create Histogram
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="help">Help text</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric</param>
        /// <param name="buckets">Buckets</param>
        /// <param name="labelNames">Array of label names</param>
        public Histogram CreateHistogram(string name, string help, bool includeTimestamp, double[] buckets, params string[] labelNames)
        {
            var metric = new Histogram(name, help, includeTimestamp, labelNames, buckets);
            return (Histogram) _registry.GetOrAdd(metric);
        }
    }
}
