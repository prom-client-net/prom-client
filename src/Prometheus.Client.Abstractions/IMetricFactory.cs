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
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        ICounter CreateCounter(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default);

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        IMetricFamily<ICounter, ValueTuple<string>> CreateCounter(string name, string help, string labelName, bool includeTimestamp = false, TimeSpan timeToLive = default);

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        IMetricFamily<ICounter, TLabels> CreateCounter<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, TimeSpan timeToLive = default)
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
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter> CreateCounter(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default, params string[] labelNames);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        ICounter<long> CreateCounterInt64(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        IMetricFamily<ICounter<long>, ValueTuple<string>> CreateCounterInt64(string name, string help, string labelName, bool includeTimestamp = false, TimeSpan timeToLive = default);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        IMetricFamily<ICounter<long>, TLabels> CreateCounterInt64<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, TimeSpan timeToLive = default)
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
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not set for the given duration</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default, params string[] labelNames);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not set for the given duration</param>
        IGauge CreateGauge(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not set for the given duration</param>
        IMetricFamily<IGauge, TLabels> CreateGauge<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, TimeSpan timeToLive = default)
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
        /// <param name="timeToLive">Remove metric if not set for the given duration</param>
        IMetricFamily<IGauge, ValueTuple<string>> CreateGauge(string name, string help, string labelName, bool includeTimestamp = false, TimeSpan timeToLive = default);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge> CreateGauge(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not set for the given duration</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge> CreateGauge(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default, params string[] labelNames);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not set for the given duration</param>
        IGauge<long> CreateGaugeInt64(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not set for the given duration</param>
        IMetricFamily<IGauge<long>, ValueTuple<string>> CreateGaugeInt64(string name, string help, string labelName, bool includeTimestamp = false, TimeSpan timeToLive = default);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not set for the given duration</param>
        IMetricFamily<IGauge<long>, TLabels> CreateGaugeInt64<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, TimeSpan timeToLive = default)
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
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not set for the given duration</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge<long>> CreateGaugeInt64(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default, params string[] labelNames);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="buckets">Buckets.</param>
        IHistogram CreateHistogram(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default, double[] buckets = null);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="buckets">Buckets.</param>
        IMetricFamily<IHistogram, ValueTuple<string>> CreateHistogram(string name, string help, string labelName, bool includeTimestamp = false, TimeSpan timeToLive = default, double[] buckets = null);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="buckets">Buckets.</param>
        IMetricFamily<IHistogram, TLabels> CreateHistogram<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, TimeSpan timeToLive = default, double[] buckets = null)
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
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default, params string[] labelNames);

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
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default, double[] buckets = null, params string[] labelNames);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        IUntyped CreateUntyped(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        IMetricFamily<IUntyped, ValueTuple<string>> CreateUntyped(string name, string help, string labelName, bool includeTimestamp = false, TimeSpan timeToLive = default);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        IMetricFamily<IUntyped, TLabels> CreateUntyped<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, TimeSpan timeToLive = default)
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
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IUntyped> CreateUntyped(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default, params string[] labelNames);

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
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="labelNames">Array of label names.</param>
        IMetricFamily<ISummary> CreateSummary(string name, string help, bool includeTimestamp = false, TimeSpan timeToLive = default, params string[] labelNames);

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
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        ISummary CreateSummary(
            string name,
            string help,
            bool includeTimestamp = false,
            TimeSpan timeToLive = default,
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
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        IMetricFamily<ISummary, ValueTuple<string>> CreateSummary(
            string name,
            string help,
            string labelName,
            bool includeTimestamp = false,
            TimeSpan timeToLive = default,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null);

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        IMetricFamily<ISummary, TLabels> CreateSummary<TLabels>(
            string name,
            string help,
            TLabels labelNames,
            bool includeTimestamp = false,
            TimeSpan timeToLive = default,
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
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        IMetricFamily<ISummary> CreateSummary(
            string name,
            string help,
            string[] labelNames,
            bool includeTimestamp,
            TimeSpan timeToLive = default,
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
