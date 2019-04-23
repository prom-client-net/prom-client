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
            return CreateCounter(name, help, includeTimestamp, true, labelNames);
        }

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="labelNames">Array of label names.</param>
        public Counter CreateCounter(string name, string help, bool includeTimestamp, bool suppressEmptySamples, params string[] labelNames)
        {
            var metric = TryGetByName<Counter>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, includeTimestamp, suppressEmptySamples, labelNames);
                metric = _registry.GetOrAdd(configuration, config => new Counter(config));
            }

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
            return CreateCounterInt64(name, help, includeTimestamp, true, labelNames);
        }

        /// <summary>
        ///     Create int-based counter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="labelNames">Array of label names.</param>
        public CounterInt64 CreateCounterInt64(string name, string help, bool includeTimestamp, bool suppressEmptySamples, params string[] labelNames)
        {
            var metric = TryGetByName<CounterInt64>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, includeTimestamp, suppressEmptySamples, labelNames);
                metric = _registry.GetOrAdd(configuration, config => new CounterInt64(config));
            }

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
            return CreateGauge(name, help, includeTimestamp, true, labelNames);
        }

        /// <summary>
        ///     Create Gauge
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="labelNames">Array of label names.</param>
        public Gauge CreateGauge(string name, string help, bool includeTimestamp, bool suppressEmptySamples, params string[] labelNames)
        {
            var metric = TryGetByName<Gauge>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, includeTimestamp, suppressEmptySamples, labelNames);
                metric = _registry.GetOrAdd(configuration, config => new Gauge(config));
            }

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
            return CreateUntyped(name, help, includeTimestamp, true, labelNames);
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="labelNames">Array of label names.</param>
        public Untyped CreateUntyped(string name, string help, bool includeTimestamp, bool suppressEmptySamples, params string[] labelNames)
        {
            var metric = TryGetByName<Untyped>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, includeTimestamp, suppressEmptySamples, labelNames);
                metric = _registry.GetOrAdd(configuration, config => new Untyped(config));
            }

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
            return CreateSummary(name, help, includeTimestamp, true, labelNames, objectives, maxAge, ageBuckets, bufCap);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        public Summary CreateSummary(
            string name,
            string help,
            bool includeTimestamp,
            bool suppressEmptySamples,
            string[] labelNames,
            IReadOnlyList<QuantileEpsilonPair> objectives,
            TimeSpan? maxAge,
            int? ageBuckets,
            int? bufCap)
        {
            var metric = TryGetByName<Summary>(name);
            if (metric == null)
            {
                var configuration = new Summary.SummaryConfiguration(name, help, includeTimestamp, suppressEmptySamples, labelNames, objectives, maxAge, ageBuckets, bufCap);
                metric = _registry.GetOrAdd(configuration, config => new Summary(config));
            }

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
            return CreateHistogram(name, help, includeTimestamp, true, buckets, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Array of label names.</param>
        public Histogram CreateHistogram(string name, string help, bool includeTimestamp, bool suppressEmptySamples, double[] buckets, params string[] labelNames)
        {
            var metric = TryGetByName<Histogram>(name);
            if (metric == null)
            {
                var configuration = new Histogram.HistogramConfiguration(name, help, includeTimestamp, suppressEmptySamples, labelNames, buckets);
                metric = _registry.GetOrAdd(configuration, config => new Histogram(config));
            }

            ValidateLabelNames(labelNames, metric.LabelNames);
            return metric;
        }

        private TCollector TryGetByName<TCollector>(string name)
            where TCollector: class, ICollector
        {
            if (_registry.TryGet(name, out var collector))
            {
                if (collector is TCollector metric)
                {
                    return metric;
                }

                throw new InvalidOperationException($"Duplicated collector name: {name}");
            }

            return null;
        }

        private void ValidateLabelNames(IReadOnlyList<string> expectedNames, IReadOnlyList<string> actualNames)
        {
            expectedNames = expectedNames ?? Array.Empty<string>();
            actualNames = actualNames ?? Array.Empty<string>();

            if (!Enumerable.SequenceEqual(expectedNames, actualNames))
                throw new ArgumentException("Collector with same name must have same label names");
        }
    }
}
