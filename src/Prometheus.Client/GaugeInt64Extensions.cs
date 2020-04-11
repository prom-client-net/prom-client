using System;
#if HasITuple
using System.Runtime.CompilerServices;
#endif
using Prometheus.Client.Abstractions;

namespace Prometheus.Client
{
    public static class GaugeInt64Extensions
    {
        public static void Inc(this IGauge<long> gauge, long increment, DateTimeOffset timestamp)
        {
            gauge.Inc(increment, timestamp.ToUnixTime());
        }

        public static void Inc(this IMetricFamily<IGauge<long>> metricFamily, long increment = 1)
        {
            metricFamily.Unlabelled.Inc(increment);
        }

        public static void Inc(this IMetricFamily<IGauge<long>> metricFamily, long increment, long timestamp)
        {
            metricFamily.Unlabelled.Inc(increment, timestamp);
        }

        public static void Inc(this IMetricFamily<IGauge<long>> metricFamily, long increment, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.Inc(increment, timestamp.ToUnixTime());
        }

        public static void Dec(this IGauge<long> gauge, long decrement, DateTimeOffset timestamp)
        {
            gauge.Dec(decrement, timestamp.ToUnixTime());
        }

        public static void Dec(this IMetricFamily<IGauge<long>> metricFamily, long decrement = 1)
        {
            metricFamily.Unlabelled.Dec(decrement);
        }

        public static void Dec(this IMetricFamily<IGauge<long>> metricFamily, long decrement, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.Dec(decrement, timestamp);
        }

        public static void Set(this IGauge<long> gauge, long val, DateTimeOffset timestamp)
        {
            gauge.Set(val, timestamp.ToUnixTime());
        }

        public static void Set(this IMetricFamily<IGauge<long>> metricFamily, long value)
        {
            metricFamily.Unlabelled.Set(value);
        }

        public static void Set(this IMetricFamily<IGauge<long>> metricFamily, long value, long timestamp)
        {
            metricFamily.Unlabelled.Set(value, timestamp);
        }

        public static void Set(this IMetricFamily<IGauge<long>> metricFamily, long value, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.Set(value, timestamp);
        }

        public static void Inc<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long increment = 1)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment);
        }

        public static void Inc<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long increment, long timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment, timestamp);
        }

        public static void Inc<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long increment, DateTimeOffset timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment, timestamp);
        }

        public static void Dec<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long decrement = 1)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Dec(decrement);
        }

        public static void Dec<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long decrement, DateTimeOffset timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Dec(decrement, timestamp);
        }

        public static void Set<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Set(value);
        }

        public static void Set<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value, long timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Set(value, timestamp);
        }

        public static void Set<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value, DateTimeOffset timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Set(value, timestamp);
        }

        public static IMetricFamily<IGauge<long>, ValueTuple<string>> CreateGaugeInt64(this MetricFactory factory, string name, string help, string labelName, MetricFlags options = MetricFlags.Default)
        {
            return factory.CreateGaugeInt64(name, help, ValueTuple.Create(labelName), options);
        }
    }
}
