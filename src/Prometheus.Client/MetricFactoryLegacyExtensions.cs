using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prometheus.Client.Abstractions;
using Prometheus.Client.SummaryImpl;

namespace Prometheus.Client
{
    public static class MetricFactoryLegacyExtensions
    {
        /// <summary>
        ///     Create Counter.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ICounter> CreateCounter(this MetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateCounter(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ICounter> CreateCounter(this MetricFactory factory, string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return factory.CreateCounter(name, help, includeTimestamp, true, labelNames);
        }

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ICounter> CreateCounter(this MetricFactory factory, string name, string help, bool includeTimestamp, bool suppressEmptySamples, params string[] labelNames)
        {
            var options = BuildMetricFlags(includeTimestamp, suppressEmptySamples);
            return factory.CreateCounter(name, help, options, labelNames);
        }

        /// <summary>
        ///     Create int-based counter.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ICounter<long>> CreateCounterInt64(this MetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateCounterInt64(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create int-based counter.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ICounter<long>> CreateCounterInt64(this MetricFactory factory, string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return factory.CreateCounterInt64(name, help, includeTimestamp, true, labelNames);
        }

        /// <summary>
        ///     Create int-based counter.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ICounter<long>> CreateCounterInt64(this MetricFactory factory, string name, string help, bool includeTimestamp, bool suppressEmptySamples, params string[] labelNames)
        {
            var options = BuildMetricFlags(includeTimestamp, suppressEmptySamples);
            return factory.CreateCounterInt64(name, help, options, labelNames);
        }

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IGauge> CreateGauge(this MetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateGauge(name, help, false, true, labelNames);
        }

        /// <summary>
        ///     Create Gauge
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IGauge> CreateGauge(this MetricFactory factory, string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return factory.CreateGauge(name, help, includeTimestamp, true, labelNames);
        }

        /// <summary>
        ///     Create Gauge
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IGauge> CreateGauge(this MetricFactory factory, string name, string help, bool includeTimestamp, bool suppressEmptySamples, params string[] labelNames)
        {
            var options = BuildMetricFlags(includeTimestamp, suppressEmptySamples);
            return factory.CreateGauge(name, help, options, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IHistogram> CreateHistogram(this MetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateHistogram(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IHistogram> CreateHistogram(this MetricFactory factory, string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return factory.CreateHistogram(name, help, includeTimestamp, null, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IHistogram> CreateHistogram(this MetricFactory factory, string name, string help, double[] buckets, params string[] labelNames)
        {
            return factory.CreateHistogram(name, help, false, buckets, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IHistogram> CreateHistogram(this MetricFactory factory, string name, string help, bool includeTimestamp, double[] buckets, params string[] labelNames)
        {
            return factory.CreateHistogram(name, help, includeTimestamp, true, buckets, labelNames);
        }

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IHistogram> CreateHistogram(this MetricFactory factory, string name, string help, bool includeTimestamp, bool suppressEmptySamples, double[] buckets, params string[] labelNames)
        {
            var options = BuildMetricFlags(includeTimestamp, suppressEmptySamples);
            return factory.CreateHistogram(name, help, buckets, options, labelNames);
        }

        /// <summary>
        ///     Create Untyped
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IUntyped> CreateUntyped(this MetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateUntyped(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IUntyped> CreateUntyped(this MetricFactory factory, string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return factory.CreateUntyped(name, help, includeTimestamp, true, labelNames);
        }

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<IUntyped> CreateUntyped(this MetricFactory factory, string name, string help, bool includeTimestamp, bool suppressEmptySamples, params string[] labelNames)
        {
            var options = BuildMetricFlags(includeTimestamp, suppressEmptySamples);
            return factory.CreateUntyped(name, help, options, labelNames);
        }

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ISummary> CreateSummary(this MetricFactory factory, string name, string help, params string[] labelNames)
        {
            return factory.CreateSummary(name, help, false, labelNames);
        }

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ISummary> CreateSummary(this MetricFactory factory, string name, string help, bool includeTimestamp, params string[] labelNames)
        {
            return factory.CreateSummary(name, help, includeTimestamp, labelNames, null, null, null, null);
        }

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ISummary> CreateSummary(this MetricFactory factory, string name, string help, string[] labelNames, IReadOnlyList<QuantileEpsilonPair> objectives, TimeSpan maxAge, int? ageBuckets, int? bufCap)
        {
            return factory.CreateSummary(name, help, false, labelNames, objectives, maxAge, ageBuckets, bufCap);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ISummary> CreateSummary(
            this MetricFactory factory,
            string name,
            string help,
            bool includeTimestamp,
            string[] labelNames,
            IReadOnlyList<QuantileEpsilonPair> objectives,
            TimeSpan? maxAge,
            int? ageBuckets,
            int? bufCap)
        {
            return factory.CreateSummary(name, help, includeTimestamp, true, labelNames, objectives, maxAge, ageBuckets, bufCap);
        }

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="factory">Metric factory</param>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="suppressEmptySamples">Define if empty samples should be included into scrape output</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMetricFamily<ISummary> CreateSummary(
            this MetricFactory factory,
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
            var options = BuildMetricFlags(includeTimestamp, suppressEmptySamples);
            return factory.CreateSummary(name, help, objectives, maxAge, ageBuckets, bufCap, options, labelNames);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MetricFlags BuildMetricFlags(bool includeTimestamp, bool suppressEmptySamples)
        {
            var options = MetricFlags.None;
            if (includeTimestamp)
                options |= MetricFlags.IncludeTimestamp;

            if (suppressEmptySamples)
                options |= MetricFlags.SupressEmptySamples;

            return options;
        }
    }
}
