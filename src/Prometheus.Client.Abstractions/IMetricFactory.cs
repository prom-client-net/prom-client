using System;
using System.Collections.Generic;

#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client
{
    public interface IMetricFactory
    {
        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        ICounter CreateCounter(string name, string help, bool includeTimestamp = false);

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<ICounter, ValueTuple<string>> CreateCounter(string name, string help, string labelName, bool includeTimestamp = false);

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter, TLabels> CreateCounter<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter> CreateCounter(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter> CreateCounter(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        ICounter<long> CreateCounterInt64(string name, string help, bool includeTimestamp = false);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<ICounter<long>, ValueTuple<string>> CreateCounterInt64(string name, string help, string labelName, bool includeTimestamp = false);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter<long>, TLabels> CreateCounterInt64<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IGauge CreateGauge(string name, string help, bool includeTimestamp = false);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge, TLabels> CreateGauge<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge> CreateGauge(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<IGauge, ValueTuple<string>> CreateGauge(string name, string help, string labelName, bool includeTimestamp = false);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge> CreateGauge(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IGauge<long> CreateGaugeInt64(string name, string help, bool includeTimestamp = false);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<IGauge<long>, ValueTuple<string>> CreateGaugeInt64(string name, string help, string labelName, bool includeTimestamp = false);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge<long>, TLabels> CreateGaugeInt64<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge<long>> CreateGaugeInt64(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge<long>> CreateGaugeInt64(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IHistogram CreateHistogram(string name, string help, bool includeTimestamp = false, double[] buckets = null);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<IHistogram, ValueTuple<string>> CreateHistogram(string name, string help, string labelName, bool includeTimestamp = false, double[] buckets = null);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram, TLabels> CreateHistogram<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, double[] buckets = null)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, double[] buckets = null, params string[] labelNames);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp = false, double[] buckets = null, params string[] labelNames);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IUntyped CreateUntyped(string name, string help, bool includeTimestamp = false);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<IUntyped, ValueTuple<string>> CreateUntyped(string name, string help, string labelName, bool includeTimestamp = false);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IUntyped, TLabels> CreateUntyped<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IUntyped> CreateUntyped(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IUntyped> CreateUntyped(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        IMetricFamily<ISummary> CreateSummary(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        IMetricFamily<ISummary> CreateSummary(string name, string help, bool includeTimestamp = false, params string[] labelNames);

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
        IMetricFamily<ISummary> CreateSummary(
            string name,
            string help,
            string[] labelNames,
            IReadOnlyList<QuantileEpsilonPair> objectives,
            TimeSpan maxAge,
            int? ageBuckets,
            int? bufCap);

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
        ISummary CreateSummary(
            string name,
            string help,
            bool includeTimestamp = false,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null);

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        IMetricFamily<ISummary, ValueTuple<string>> CreateSummary(
            string name,
            string help,
            string labelName,
            bool includeTimestamp = false,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null);

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
        IMetricFamily<ISummary, TLabels> CreateSummary<TLabels>(
            string name,
            string help,
            TLabels labelNames,
            bool includeTimestamp = false,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

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
        IMetricFamily<ISummary> CreateSummary(
            string name,
            string help,
            string[] labelNames,
            bool includeTimestamp,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null);

        void Release(string name);

        void Release<TMetric>(IMetricFamily<TMetric> metricFamily)
            where TMetric : IMetric;

        void Release<TMetric, TLabels>(IMetricFamily<TMetric, TLabels> metricFamily)
            where TMetric : IMetric
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif
    }
}
