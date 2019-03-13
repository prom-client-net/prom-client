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
            var configuration = new MetricConfiguration(name, help, includeTimestamp, labelNames);
            var metric = _registry.GetOrAdd(configuration, config => new Counter(config));
            ValidateLabelNames(labelNames, metric.LabelNames);
            return metric;
        }

        /// <summary>
        ///     Create int-based counter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public CounterInt64 CreateCounterInt64(string name, string help, params string[] labelNames)
        {
            return CreateCounterInt64(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create int-based counter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        public CounterInt64 CreateCounterInt64(string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            var configuration = new MetricConfiguration(name, help, includeTimestamp, labelNames);
            var metric = _registry.GetOrAdd(configuration, config => new CounterInt64(config));
            ValidateLabelNames(labelNames, metric.LabelNames);
            return metric;
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
            var configuration = new MetricConfiguration(name, help, includeTimestamp, labelNames);
            var metric = _registry.GetOrAdd(configuration, config => new Gauge(config));
            ValidateLabelNames(labelNames, metric.LabelNames);
            return metric;
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
            var configuration = new MetricConfiguration(name, help, includeTimestamp, labelNames);
            var metric = _registry.GetOrAdd(configuration, config => new Untyped(config));
            ValidateLabelNames(labelNames, metric.LabelNames);
            return metric;
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
        public Summary CreateSummary(string name, string help, string[] labelNames, IReadOnlyList<QuantileEpsilonPair> objectives, TimeSpan maxAge, int? ageBuckets, int? bufCap)
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
            IReadOnlyList<QuantileEpsilonPair> objectives,
            TimeSpan? maxAge,
            int? ageBuckets,
            int? bufCap)
        {
            var configuration = new Summary.SummaryConfiguration(name, help, includeTimestamp, labelNames, objectives, maxAge, ageBuckets, bufCap);
            var metric = _registry.GetOrAdd(configuration, config => new Summary(config));
            ValidateLabelNames(labelNames, metric.LabelNames);
            return metric;
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
            var configuration = new Histogram.HistogramConfiguration(name, help, includeTimestamp, labelNames, buckets);
            var metric = _registry.GetOrAdd(configuration, config => new Histogram(config));
            ValidateLabelNames(labelNames, metric.LabelNames);
            return metric;
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
