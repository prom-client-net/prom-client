using System;
#if HasITuple
using System.Runtime.CompilerServices;
#endif
using Prometheus.Client.Abstractions;

namespace Prometheus.Client
{
    public static class HistogramExtensions
    {
        public static void Observe(this IHistogram observer, double val, DateTimeOffset timestamp)
        {
            observer.Observe(val, timestamp.ToUnixTime());
        }

        public static void Observe(this IMetricFamily<IHistogram> metricFamily, double val)
        {
            metricFamily.Unlabelled.Observe(val);
        }

        public static void Observe(this IMetricFamily<IHistogram> metricFamily, double val, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.Observe(val, timestamp.ToUnixTime());
        }

        public static void Observe<TLabels>(this IMetricFamily<IHistogram, TLabels> metricFamily, double val)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Observe(val);
        }

        public static void Observe<TLabels>(this IMetricFamily<IHistogram, TLabels> metricFamily, double val, DateTimeOffset timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Observe(val, timestamp.ToUnixTime());
        }

        public static IMetricFamily<IHistogram, ValueTuple<string>> CreateHistogram(this MetricFactory factory, string name, string help, string labelName, double[] buckets = null, MetricFlags options = MetricFlags.Default)
        {
            return factory.CreateHistogram(name, help, ValueTuple.Create(labelName), buckets, options);
        }
    }
}
