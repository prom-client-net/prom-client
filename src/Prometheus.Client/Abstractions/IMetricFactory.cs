using System;
using System.Collections.Generic;

#if HasITuple
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client.Abstractions
{
    public interface IMetricFactory
    {
        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        ICounter CreateCounter(string name, string help, MetricFlags options = MetricFlags.Default);

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter, TLabels> CreateCounter<TLabels>(string name, string help, TLabels labelNames, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
        where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter> CreateCounter(string name, string help, MetricFlags options = MetricFlags.Default, params string[]labelNames);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        ICounter<long> CreateCounterInt64(string name, string help, MetricFlags options = MetricFlags.Default);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter<long>, TLabels> CreateCounterInt64<TLabels>(string name, string help, TLabels labelNames, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
        where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, MetricFlags options = MetricFlags.Default, params string[]labelNames);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        IGauge CreateGauge(string name, string help, MetricFlags options = MetricFlags.Default);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge, TLabels> CreateGauge<TLabels>(string name, string help, TLabels labelNames, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
        where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge> CreateGauge(string name, string help, MetricFlags options = MetricFlags.Default, params string[] labelNames);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        IGauge<long> CreateGaugeInt64(string name, string help, MetricFlags options = MetricFlags.Default);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge<long>, TLabels> CreateGaugeInt64<TLabels>(string name, string help, TLabels labelNames, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
        where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge<long>> CreateGaugeInt64(string name, string help, MetricFlags options = MetricFlags.Default, params string[] labelNames);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="options">Metric flags</param>
        IHistogram CreateHistogram(string name, string help, double[] buckets = null, MetricFlags options = MetricFlags.Default);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram, TLabels> CreateHistogram<TLabels>(string name, string help, TLabels labelNames, double[] buckets = null, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
        where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, double[] buckets = null, MetricFlags options = MetricFlags.Default, params string[] labelNames);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        IUntyped CreateUntyped(string name, string help, MetricFlags options = MetricFlags.Default);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IUntyped, TLabels> CreateUntyped<TLabels>(string name, string help, TLabels labelNames, MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
        where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IUntyped> CreateUntyped(string name, string help, MetricFlags options = MetricFlags.Default, params string[] labelNames);

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
        ISummary CreateSummary(
            string name,
            string help,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null,
            MetricFlags options = MetricFlags.Default);

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        IMetricFamily<ISummary, TLabels> CreateSummary<TLabels>(
            string name,
            string help,
            TLabels labelNames,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null,
            MetricFlags options = MetricFlags.Default)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
        where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="options">Metric flags</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        IMetricFamily<ISummary> CreateSummary(
            string name,
            string help,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null,
            MetricFlags options = MetricFlags.Default,
            params string[] labelNames);
    }
}
