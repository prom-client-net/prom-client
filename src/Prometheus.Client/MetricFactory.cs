using System;
using System.Collections.Generic;
using System.Linq;
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
        ///     Create Counter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Counter CreateCounter(string name, string help, params string[] labelNames)
        {
            return CreateCounter(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Counter CreateCounter(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            if (_registry.GetOrAdd(name, () => new Counter(name, help, includeTimestamp, labelNames)) is Counter metric)
            {
                ValidateLabelNames(labelNames, metric.LabelNames);
                return metric;
            }

            throw new InvalidOperationException($"Duplicate metric name: {name}");
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Gauge CreateGauge(string name, string help, params string[] labelNames)
        {
            return CreateGauge(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Gauge
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Gauge CreateGauge(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            if (_registry.GetOrAdd(name, () => new Gauge(name, help, includeTimestamp, labelNames)) is Gauge metric)
            {
                ValidateLabelNames(labelNames, metric.LabelNames);
                return metric;
            }

            throw new InvalidOperationException($"Duplicate metric name: {name}");
        }

        /// <summary>
        ///     Create Untyped
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Untyped CreateUntyped(string name, string help, params string[] labelNames)
        {
            return CreateUntyped(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Untyped CreateUntyped(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            if (_registry.GetOrAdd(name, () => new Untyped(name, help, includeTimestamp, labelNames)) is Untyped metric)
            {
                ValidateLabelNames(labelNames, metric.LabelNames);
                return metric;
            }

            throw new InvalidOperationException($"Duplicate metric name: {name}");
        }

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Summary CreateSummary(string name, string help, params string[] labelNames)
        {
            return CreateSummary(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Summary CreateSummary(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return CreateSummary(name, help, includeTimestamp, labelNames, null, null, null, null);
        }

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
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
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        public Summary CreateSummary(
            string name,
            string help,
            bool includeTimestamp,
            string[] labelNames,
            IList<QuantileEpsilonPair> objectives,
            TimeSpan? maxAge,
            int? ageBuckets,
            int? bufCap)
        {
            if (_registry.GetOrAdd(name, () => new Summary(name, help, includeTimestamp, labelNames, objectives, maxAge, ageBuckets, bufCap)) is Summary metric)
            {
                ValidateLabelNames(labelNames, metric.LabelNames);
                return metric;
            }

            throw new InvalidOperationException($"Duplicate metric name: {name}");
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Histogram CreateHistogram(string name, string help, params string[] labelNames)
        {
            return CreateHistogram(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Histogram CreateHistogram(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return CreateHistogram(name, help, includeTimestamp, null, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Histogram CreateHistogram(string name, string help, double[] buckets, params string[] labelNames)
        {
            return CreateHistogram(name, help, false, buckets, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Histogram CreateHistogram(string name, string help, bool includeTimestamp, double[] buckets, params string[] labelNames)
        {
            if (_registry.GetOrAdd(name, () => new Histogram(name, help, includeTimestamp, labelNames, buckets)) is Histogram metric)
            {
                ValidateLabelNames(labelNames, metric.LabelNames);
                return metric;
            }

            throw new InvalidOperationException($"Duplicate metric name: {name}");
        }

        private void ValidateLabelNames(string[] expectedNames, string[] actualNames)
        {
            expectedNames = expectedNames ?? Array.Empty<string>();
            actualNames = actualNames ?? Array.Empty<string>();

            if (!Enumerable.SequenceEqual(expectedNames, actualNames))
                throw new ArgumentException("Collector with same name must have same label names");
        }
    }
}
