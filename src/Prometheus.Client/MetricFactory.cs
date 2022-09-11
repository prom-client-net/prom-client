using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Prometheus.Client.Collectors;

namespace Prometheus.Client
{
    public class MetricFactory : IMetricFactory
    {
        private readonly ICollectorRegistry _registry;
        private readonly object _factoryProxyLock = new object();
        private Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter>>[] _counterFactoryProxies;
        private Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter<long>>>[] _counterInt64FactoryProxies;
        private Func<MetricFactory, MetricConfiguration, IMetricFamily<IGauge>>[] _gaugeFactoryProxies;
        private Func<MetricFactory, MetricConfiguration, IMetricFamily<IGauge<long>>>[] _gaugeInt64FactoryProxies;
        private Func<MetricFactory, MetricConfiguration, IMetricFamily<IUntyped>>[] _untypedFactoryProxies;
        private Func<MetricFactory, HistogramConfiguration, IMetricFamily<IHistogram>>[] _histogramFactoryProxies;
        private Func<MetricFactory, SummaryConfiguration, IMetricFamily<ISummary>>[] _summaryFactoryProxies;

        public MetricFactory(ICollectorRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        public ICounter CreateCounter(string name, string help, bool includeTimestamp = false)
        {
            var metric = CreateCounter(name, help, ValueTuple.Create(), includeTimestamp);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<ICounter, TLabels> CreateCounter<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<ICounter, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp);
                metric = CreateCounterInternal<TLabels>(configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<ICounter> CreateCounter(string name, string help, params string[] labelNames)
        {
            return CreateCounter(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<ICounter> CreateCounter(string name, string help, bool includeTimestamp = false, params string[] labelNames)
        {
            var metric = TryGetByName<IMetricFamily<ICounter>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, labelNames, includeTimestamp);
                metric = GetCounterFactory(labelNames?.Length ?? 0)(this, configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        public ICounter<long> CreateCounterInt64(string name, string help, bool includeTimestamp = false)
        {
            var metric = CreateCounterInt64(name, help, ValueTuple.Create(), includeTimestamp);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<ICounter<long>, TLabels> CreateCounterInt64<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<ICounter<long>, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp);
                metric = CreateCounterInt64Internal<TLabels>(configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, params string[] labelNames)
        {
            return CreateCounterInt64(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, bool includeTimestamp = false, params string[] labelNames)
        {
            var metric = TryGetByName<IMetricFamily<ICounter<long>>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, labelNames, includeTimestamp);
                metric = GetCounterInt64Factory(labelNames?.Length ?? 0)(this, configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        public IGauge CreateGauge(string name, string help, bool includeTimestamp = false)
        {
            var metric = CreateGauge(name, help, ValueTuple.Create(), includeTimestamp);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IGauge, TLabels> CreateGauge<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<IGauge, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp);
                metric = CreateGaugeInternal<TLabels>(configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IGauge> CreateGauge(string name, string help, params string[] labelNames)
        {
            return CreateGauge(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IGauge> CreateGauge(string name, string help, bool includeTimestamp = false, params string[] labelNames)
        {
            var metric = TryGetByName<IMetricFamily<IGauge>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, labelNames, includeTimestamp);
                metric = GetGaugeFactory(labelNames?.Length ?? 0)(this, configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        public IGauge<long> CreateGaugeInt64(string name, string help, bool includeTimestamp = false)
        {
            var metric = CreateGaugeInt64(name, help, ValueTuple.Create(), includeTimestamp);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IGauge<long>, TLabels> CreateGaugeInt64<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<IGauge<long>, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp);
                metric = CreateGaugeInt64Internal<TLabels>(configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IGauge<long>> CreateGaugeInt64(string name, string help, params string[] labelNames)
        {
            return CreateGaugeInt64(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IGauge<long>> CreateGaugeInt64(string name, string help, bool includeTimestamp = false, params string[] labelNames)
        {
            var metric = TryGetByName<IMetricFamily<IGauge<long>>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, labelNames, includeTimestamp);
                metric = GetGaugeInt64Factory(labelNames?.Length ?? 0)(this, configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        public IHistogram CreateHistogram(string name, string help, bool includeTimestamp = false, double[] buckets = null)
        {
            var metric = CreateHistogram(name, help, ValueTuple.Create(), includeTimestamp, buckets);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IHistogram, TLabels> CreateHistogram<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, double[] buckets = null)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<IHistogram, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new HistogramConfiguration(name, help, LabelsHelper.ToArray(labelNames), buckets, includeTimestamp);
                metric = CreateHistogramInternal<TLabels>(configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IHistogram> CreateHistogram(string name, string help, params string[] labelNames)
        {
            return CreateHistogram(name, help, false, null, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp = false, params string[] labelNames)
        {
            return CreateHistogram(name, help, includeTimestamp, null, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IHistogram> CreateHistogram(string name, string help, double[] buckets = null, params string[] labelNames)
        {
            return CreateHistogram(name, help, false, buckets, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp = false, double[] buckets = null, params string[] labelNames)
        {
            var metric = TryGetByName<IMetricFamily<IHistogram>>(name);
            if (metric == null)
            {
                var configuration = new HistogramConfiguration(name, help, labelNames, buckets, includeTimestamp);
                metric = GetHistogramFactory(labelNames?.Length ?? 0)(this, configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        public IUntyped CreateUntyped(string name, string help, bool includeTimestamp = false)
        {
            var metric = CreateUntyped(name, help, ValueTuple.Create(), includeTimestamp);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IUntyped, TLabels> CreateUntyped<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<IUntyped, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp);
                metric = CreateUntypedInternal<TLabels>(configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IUntyped> CreateUntyped(string name, string help, params string[] labelNames)
        {
            return CreateUntyped(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        public IMetricFamily<IUntyped> CreateUntyped(string name, string help, bool includeTimestamp = false, params string[] labelNames)
        {
            var metric = TryGetByName<IMetricFamily<IUntyped>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, labelNames, includeTimestamp);
                metric = GetUntypedFactory(labelNames?.Length ?? 0)(this, configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        public IMetricFamily<ISummary> CreateSummary(string name, string help, params string[] labelNames)
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
        public IMetricFamily<ISummary> CreateSummary(string name, string help, bool includeTimestamp = false, params string[] labelNames)
        {
            return CreateSummary(name, help, labelNames, includeTimestamp);
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
        public IMetricFamily<ISummary> CreateSummary(string name, string help, string[] labelNames, IReadOnlyList<QuantileEpsilonPair> objectives, TimeSpan maxAge, int? ageBuckets,
            int? bufCap)
        {
            return CreateSummary(name, help, labelNames, false, objectives, maxAge, ageBuckets, bufCap);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        public ISummary CreateSummary(
            string name,
            string help,
            bool includeTimestamp = false,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null)
        {
            var metric = CreateSummary(name, help, ValueTuple.Create(), includeTimestamp, objectives, maxAge, ageBuckets, bufCap);
            return metric.Unlabelled;
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
        public IMetricFamily<ISummary, TLabels> CreateSummary<TLabels>(
            string name,
            string help,
            TLabels labelNames,
            bool includeTimestamp = false,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<ISummary, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new SummaryConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp, objectives, maxAge, ageBuckets, bufCap);
                metric = CreateSummaryInternal<TLabels>(configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
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
        public IMetricFamily<ISummary> CreateSummary(
            string name,
            string help,
            string[] labelNames,
            bool includeTimestamp,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null)
        {
            var metric = TryGetByName<IMetricFamily<ISummary>>(name);
            if (metric == null)
            {
                var configuration = new SummaryConfiguration(name, help, labelNames, includeTimestamp, objectives, maxAge, ageBuckets, bufCap);
                metric = GetSummaryFactory(labelNames?.Length ?? 0)(this, configuration);
            }
            else
            {
                ValidateLabelNames(metric.LabelNames, labelNames);
            }

            return metric;
        }

        public void Release(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            _registry.Remove(name);
        }

        public void Release<TMetric>(IMetricFamily<TMetric> metricFamily)
            where TMetric : IMetric
        {
            if (metricFamily == null)
                throw new ArgumentNullException(nameof(metricFamily));

            _registry.Remove(metricFamily.Name);
        }

        public void Release<TMetric, TLabels>(IMetricFamily<TMetric, TLabels> metricFamily)
            where TMetric : IMetric
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            if (metricFamily == null)
                throw new ArgumentNullException(nameof(metricFamily));

            _registry.Remove(metricFamily.Name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TCollector TryGetByName<TCollector>(string name)
        {
            if (_registry.TryGet(name, out var collector))
            {
                if (collector is TCollector metric)
                    return metric;
            }

            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ValidateLabelNames<TLabels>(TLabels expectedNames, TLabels actualNames)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            if (LabelsHelper.GetHashCode(expectedNames) != LabelsHelper.GetHashCode(actualNames))
                ThrowLabelsValidationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ValidateLabelNames(IReadOnlyList<string> expectedNames, IReadOnlyList<string> actualNames)
        {
            if (expectedNames == null && actualNames == null)
                return;

            expectedNames ??= Array.Empty<string>();
            actualNames ??= Array.Empty<string>();

            if (LabelsHelper.GetHashCode(expectedNames) != LabelsHelper.GetHashCode(actualNames))
                ThrowLabelsValidationException();
        }

        private static void ThrowLabelsValidationException()
        {
            throw new InvalidOperationException("Collector with same name must have same type and same label names");
        }

        internal MetricFamily<ICounter, Counter, TLabels, MetricConfiguration> CreateCounterInternal<TLabels>(MetricConfiguration configuration)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            return _registry.GetOrAdd(configuration,
                config => new MetricFamily<ICounter, Counter, TLabels, MetricConfiguration>(
                    config,
                    MetricType.Counter,
                    (cfg, labelNames) => new Counter(cfg, labelNames)));
        }

        internal MetricFamily<ICounter<long>, CounterInt64, TLabels, MetricConfiguration> CreateCounterInt64Internal<TLabels>(MetricConfiguration configuration)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            return _registry.GetOrAdd(configuration,
                config => new MetricFamily<ICounter<long>, CounterInt64, TLabels, MetricConfiguration>(
                    config,
                    MetricType.Counter,
                    (cfg, labelNames) => new CounterInt64(cfg, labelNames)));
        }

        internal MetricFamily<IGauge, Gauge, TLabels, MetricConfiguration> CreateGaugeInternal<TLabels>(MetricConfiguration configuration)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            return _registry.GetOrAdd(configuration,
                config => new MetricFamily<IGauge, Gauge, TLabels, MetricConfiguration>(
                    config,
                    MetricType.Gauge,
                    (cfg, labelNames) => new Gauge(cfg, labelNames)));
        }

        internal MetricFamily<IGauge<long>, GaugeInt64, TLabels, MetricConfiguration> CreateGaugeInt64Internal<TLabels>(MetricConfiguration configuration)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            return _registry.GetOrAdd(configuration,
                config => new MetricFamily<IGauge<long>, GaugeInt64, TLabels, MetricConfiguration>(
                    config,
                    MetricType.Gauge,
                    (cfg, labelNames) => new GaugeInt64(cfg, labelNames)));
        }

        internal MetricFamily<IHistogram, Histogram, TLabels, HistogramConfiguration> CreateHistogramInternal<TLabels>(HistogramConfiguration configuration)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            return _registry.GetOrAdd(configuration,
                config => new MetricFamily<IHistogram, Histogram, TLabels, HistogramConfiguration>(
                    config,
                    MetricType.Histogram,
                    (cfg, labelNames) => new Histogram(cfg, labelNames)));
        }

        internal MetricFamily<IUntyped, Untyped, TLabels, MetricConfiguration> CreateUntypedInternal<TLabels>(MetricConfiguration configuration)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            return _registry.GetOrAdd(configuration,
                config => new MetricFamily<IUntyped, Untyped, TLabels, MetricConfiguration>(
                    config,
                    MetricType.Untyped,
                    (cfg, labelNames) => new Untyped(cfg, labelNames)));
        }

        internal MetricFamily<ISummary, Summary, TLabels, SummaryConfiguration> CreateSummaryInternal<TLabels>(SummaryConfiguration configuration)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            return _registry.GetOrAdd(configuration,
                config => new MetricFamily<ISummary, Summary, TLabels, SummaryConfiguration>(
                    config,
                    MetricType.Summary,
                    (cfg, labelNames) => new Summary(cfg, labelNames)));
        }

        internal Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter>> GetCounterFactory(int labelNamesLen)
        {
            return GetFactory(ref _counterFactoryProxies, nameof(CreateCounterInternal), labelNamesLen);
        }

        internal Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter<long>>> GetCounterInt64Factory(int labelNamesLen)
        {
            return GetFactory(ref _counterInt64FactoryProxies, nameof(CreateCounterInt64Internal), labelNamesLen);
        }

        internal Func<MetricFactory, MetricConfiguration, IMetricFamily<IGauge>> GetGaugeFactory(int labelNamesLen)
        {
            return GetFactory(ref _gaugeFactoryProxies, nameof(CreateGaugeInternal), labelNamesLen);
        }

        internal Func<MetricFactory, MetricConfiguration, IMetricFamily<IGauge<long>>> GetGaugeInt64Factory(int labelNamesLen)
        {
            return GetFactory(ref _gaugeInt64FactoryProxies, nameof(CreateGaugeInt64Internal), labelNamesLen);
        }

        internal Func<MetricFactory, MetricConfiguration, IMetricFamily<IUntyped>> GetUntypedFactory(int labelNamesLen)
        {
            return GetFactory(ref _untypedFactoryProxies, nameof(CreateUntypedInternal), labelNamesLen);
        }

        internal Func<MetricFactory, HistogramConfiguration, IMetricFamily<IHistogram>> GetHistogramFactory(int labelNamesLen)
        {
            return GetFactory(ref _histogramFactoryProxies, nameof(CreateHistogramInternal), labelNamesLen);
        }

        internal Func<MetricFactory, SummaryConfiguration, IMetricFamily<ISummary>> GetSummaryFactory(int labelNamesLen)
        {
            return GetFactory(ref _summaryFactoryProxies, nameof(CreateSummaryInternal), labelNamesLen);
        }

        private Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>> GetFactory<TConfiguration, TMetric>(
            ref Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>>[] cache, string targetMethodName, int labelNamesLen)
            where TConfiguration : MetricConfiguration
            where TMetric : IMetric
        {
            if (cache?.GetUpperBound(0) > labelNamesLen)
                return cache[labelNamesLen];

            lock (_factoryProxyLock)
            {
                if (cache?.GetUpperBound(0) > labelNamesLen)
                    return cache[labelNamesLen];

                var tmp = new Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>>[labelNamesLen + 1];
                if (cache != null)
                    Array.Copy(cache, tmp, cache.Length);

                var configurationParameter = Expression.Parameter(typeof(TConfiguration), "configuration");
                var factoryParameter = Expression.Parameter(typeof(MetricFactory), "factory");
                for (var i = cache?.Length ?? 0; i <= labelNamesLen; i++)
                {
                    var labelNamesTupleType = LabelsHelper.MakeValueTupleType(i);

                    var targetMethodCall = Expression.Call(
                        factoryParameter,
                        targetMethodName,
                        new[] { labelNamesTupleType },
                        configurationParameter);

                    tmp[i] = Expression.Lambda<Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>>>(targetMethodCall, factoryParameter, configurationParameter).Compile();
                }

                cache = tmp;
            }

            return cache[labelNamesLen];
        }
    }
}
