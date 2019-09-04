using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.SummaryImpl;

namespace Prometheus.Client
{
    public class MetricFactory
    {
        private readonly ICollectorRegistry _registry;
        private readonly object _factoryProxyLock = new object();
        private Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter>>[] _counterFactoryProxies;
        private Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter<long>>>[] _counterInt64FactoryProxies;
        private Func<MetricFactory, MetricConfiguration, IMetricFamily<IGauge>>[] _gaugeFactoryProxies;
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
        /// <param name="options">Metric flags</param>
        public ICounter CreateCounter(string name, string help, MetricFlags options = MetricFlags.Default)
        {
            var metric = TryGetByName<IMetricFamily<ICounter, ValueTuple>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, null, options);
                metric = CreateCounterInternal<ValueTuple>( configuration);
            }

            ValidateLabelNames(metric.LabelNames, default);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Label names</param>
        public IMetricFamily<ICounter, TLabels> CreateCounter<TLabels>(string name, string help, TLabels labels, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<ICounter, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, TupleHelper<TLabels>.ToArray(labels), options);
                metric = CreateCounterInternal<TLabels>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Label names</param>
        public IMetricFamily<ICounter> CreateCounter(string name, string help, MetricFlags options = MetricFlags.Default, params string[] labels)
        {
            var metric = TryGetByName<IMetricFamily<ICounter>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, labels, options);
                metric = GetCounterFactory(labels?.Length ?? 0)(this, configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        public ICounter<long> CreateCounterInt64(string name, string help, MetricFlags options = MetricFlags.Default)
        {
            var metric = TryGetByName<IMetricFamily<ICounter<long>, ValueTuple>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, null, options);
                metric = CreateCounterInt64Internal<ValueTuple>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, default);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Label names</param>
        public IMetricFamily<ICounter<long>, TLabels> CreateCounterInt64<TLabels>(string name, string help, TLabels labels, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<ICounter<long>, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, TupleHelper<TLabels>.ToArray(labels), options);
                metric = CreateCounterInt64Internal<TLabels>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Label names</param>
        public IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, MetricFlags options = MetricFlags.Default, params string[] labels)
        {
            var metric = TryGetByName<IMetricFamily<ICounter<long>>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, labels, options);
                metric = GetCounterInt64Factory(labels?.Length ?? 0)(this, configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        public IGauge CreateGauge(string name, string help, MetricFlags options = MetricFlags.Default)
        {
            var metric = TryGetByName<IMetricFamily<IGauge, ValueTuple>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, null, options);
                metric = CreateGaugeInternal<ValueTuple>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, default);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Label names</param>
        public IMetricFamily<IGauge, TLabels> CreateGauge<TLabels>(string name, string help, TLabels labels, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<IGauge, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, TupleHelper<TLabels>.ToArray(labels), options);
                metric = CreateGaugeInternal<TLabels>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Label names</param>
        public IMetricFamily<IGauge> CreateGauge(string name, string help, MetricFlags options = MetricFlags.Default, params string[] labels)
        {
            var metric = TryGetByName<IMetricFamily<IGauge>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, labels, options);
                metric = GetGaugeFactory(labels?.Length ?? 0)(this, configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="options">Metric flags</param>
        public IHistogram CreateHistogram(string name, string help, double[] buckets = null, MetricFlags options = MetricFlags.Default)
        {
            var metric = TryGetByName<IMetricFamily<IHistogram, ValueTuple>>(name);
            if (metric == null)
            {
                var configuration = new HistogramConfiguration(name, help, null, buckets, options);
                metric = CreateHistogramInternal<ValueTuple>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, default);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Label names</param>
        public IMetricFamily<IHistogram, TLabels> CreateHistogram<TLabels>(string name, string help, TLabels labels, double[] buckets = null, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<IHistogram, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new HistogramConfiguration(name, help, TupleHelper<TLabels>.ToArray(labels), buckets, options);
                metric = CreateHistogramInternal<TLabels>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Label names</param>
        public IMetricFamily<IHistogram> CreateHistogram(string name, string help, double[] buckets = null, MetricFlags options = MetricFlags.Default, params string[] labels)
        {
            var metric = TryGetByName<IMetricFamily<IHistogram>>(name);
            if (metric == null)
            {
                var configuration = new HistogramConfiguration(name, help, labels, buckets, options);
                metric = GetHistogramFactory(labels?.Length ?? 0)(this, configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        public IUntyped CreateUntyped(string name, string help, MetricFlags options = MetricFlags.Default)
        {
            var metric = TryGetByName<IMetricFamily<IUntyped, ValueTuple>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, null, options);
                metric = CreateUntypedInternal<ValueTuple>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, default);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Label names</param>
        public IMetricFamily<IUntyped, TLabels> CreateUntyped<TLabels>(string name, string help, TLabels labels, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<IUntyped, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, TupleHelper<TLabels>.ToArray(labels), options);
                metric = CreateUntypedInternal<TLabels>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Label names</param>
        public IMetricFamily<IUntyped> CreateUntyped(string name, string help, MetricFlags options = MetricFlags.Default, params string[] labels)
        {
            var metric = TryGetByName<IMetricFamily<IUntyped>>(name);
            if (metric == null)
            {
                var configuration = new MetricConfiguration(name, help, labels, options);
                metric = GetUntypedFactory(labels?.Length ?? 0)(this, configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        public ISummary CreateSummary(
            string name,
            string help,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null,
            MetricFlags options = MetricFlags.Default)
        {
            var metric = TryGetByName<IMetricFamily<ISummary, ValueTuple>>(name);
            if (metric == null)
            {
                var configuration = new SummaryConfiguration(name, help, null, options, objectives, maxAge, ageBuckets, bufCap);
                metric = CreateSummaryInternal<ValueTuple>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, default);
            return metric.Unlabelled;
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        public IMetricFamily<ISummary, TLabels> CreateSummary<TLabels>(
            string name,
            string help,
            TLabels labels,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null,
            MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            var metric = TryGetByName<IMetricFamily<ISummary, TLabels>>(name);
            if (metric == null)
            {
                var configuration = new SummaryConfiguration(name, help, TupleHelper<TLabels>.ToArray(labels), options, objectives, maxAge, ageBuckets, bufCap);
                metric = CreateSummaryInternal<TLabels>(configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labels">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        public IMetricFamily<ISummary> CreateSummary(
            string name,
            string help,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null,
            MetricFlags options = MetricFlags.Default,
            params string[] labels)
        {
            var metric = TryGetByName<IMetricFamily<ISummary>>(name);
            if (metric == null)
            {
                var configuration = new SummaryConfiguration(name, help, labels, options, objectives, maxAge, ageBuckets, bufCap);
                metric = GetSummaryFactory(labels?.Length ?? 0)(this, configuration);
            }

            ValidateLabelNames(metric.LabelNames, labels);
            return metric;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TCollector TryGetByName<TCollector>(string name)
        {
            if (_registry.TryGet(name, out var collector))
            {
                if (collector is TCollector metric)
                    return metric;

                ThrowLabelsValidationExceprion();
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
            if (!expectedNames.Equals(actualNames))
                ThrowLabelsValidationExceprion();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ValidateLabelNames(IReadOnlyList<string> expectedNames, IReadOnlyList<string> actualNames)
        {
            if (expectedNames == null && actualNames == null)
                return;

            expectedNames ??= Array.Empty<string>();
            actualNames ??= Array.Empty<string>();

            if (expectedNames.Count != actualNames.Count)
                ThrowLabelsValidationExceprion();

            for (var i = 0; i < expectedNames.Count; i++)
            {
                if(expectedNames[i] != actualNames[i])
                    ThrowLabelsValidationExceprion();
            }
        }

        private static void ThrowLabelsValidationExceprion()
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
                    (cfg, labels) => new Counter(cfg, labels)));
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
                    (cfg, labels) => new CounterInt64(cfg, labels)));
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
                    (cfg, labels) => new Gauge(cfg, labels)));
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
                    (cfg, labels) => new Histogram(cfg, labels)));
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
                    (cfg, labels) => new Untyped(cfg, labels)));
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
                    (cfg, labels) => new Summary(cfg, labels)));
        }

        internal Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter>> GetCounterFactory(int labelsLen)
        {
            return GetFactory(ref _counterFactoryProxies, nameof(CreateCounterInternal), labelsLen);
        }

        internal Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter<long>>> GetCounterInt64Factory(int labelsLen)
        {
            return GetFactory(ref _counterInt64FactoryProxies, nameof(CreateCounterInt64Internal), labelsLen);
        }

        internal Func<MetricFactory, MetricConfiguration, IMetricFamily<IGauge>> GetGaugeFactory(int labelsLen)
        {
            return GetFactory(ref _gaugeFactoryProxies, nameof(CreateGaugeInternal), labelsLen);
        }

        internal Func<MetricFactory, MetricConfiguration, IMetricFamily<IUntyped>> GetUntypedFactory(int labelsLen)
        {
            return GetFactory(ref _untypedFactoryProxies, nameof(CreateUntypedInternal), labelsLen);
        }

        internal Func<MetricFactory, HistogramConfiguration, IMetricFamily<IHistogram>> GetHistogramFactory(int labelsLen)
        {
            return GetFactory(ref _histogramFactoryProxies, nameof(CreateHistogramInternal), labelsLen);
        }

        internal Func<MetricFactory, SummaryConfiguration, IMetricFamily<ISummary>> GetSummaryFactory(int labelsLen)
        {
            return GetFactory(ref _summaryFactoryProxies, nameof(CreateSummaryInternal), labelsLen);
        }

        private Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>> GetFactory<TConfiguration, TMetric>(ref Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>>[] cache, string targetMethodName, int labelsLen)
            where TConfiguration : MetricConfiguration
            where TMetric : IMetric
        {
            if (cache?.GetUpperBound(0) > labelsLen)
                return cache[labelsLen];

            lock (_factoryProxyLock)
            {
                if (cache?.GetUpperBound(0) > labelsLen)
                    return cache[labelsLen];

                var tmp = new Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>>[labelsLen + 1];
                if (cache != null)
                    Array.Copy(cache, tmp, cache.Length);

                var configurationParameter = Expression.Parameter(typeof(TConfiguration), "configuration");
                var factoryParameter = Expression.Parameter(typeof(MetricFactory), "factory");
                for (var i = cache?.Length ?? 0; i <= labelsLen; i++)
                {
                    var labelsTupleType = TupleHelper.MakeValueTupleType(i);

                    var targetMethodCall = Expression.Call(
                        factoryParameter,
                        targetMethodName,
                        new [] { labelsTupleType },
                        configurationParameter);

                    tmp[i] = Expression.Lambda<Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>>>(targetMethodCall, factoryParameter, configurationParameter).Compile();
                }

                cache = tmp;
            }

            return cache[labelsLen];
        }
    }
}
