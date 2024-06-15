using System;
#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client;

public static class GaugeInt64Extensions
{
    public static void Inc(this IGauge<long> gauge, long increment, DateTimeOffset timestamp)
    {
        gauge.Inc(increment, timestamp.ToUnixTimeMilliseconds());
    }

    public static void IncTo(this IGauge<long> gauge, long value, DateTimeOffset timestamp)
    {
        gauge.IncTo(value, timestamp.ToUnixTimeMilliseconds());
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
        metricFamily.Unlabelled.Inc(increment, timestamp.ToUnixTimeMilliseconds());
    }

    public static void IncTo(this IMetricFamily<IGauge<long>> metricFamily, long value)
    {
        metricFamily.Unlabelled.IncTo(value);
    }

    public static void IncTo(this IMetricFamily<IGauge<long>> metricFamily, long value, long timestamp)
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }

    public static void IncTo(this IMetricFamily<IGauge<long>> metricFamily, long value, DateTimeOffset timestamp)
    {
        metricFamily.Unlabelled.IncTo(value, timestamp.ToUnixTimeMilliseconds());
    }

    public static void Dec(this IGauge<long> gauge, long decrement, DateTimeOffset timestamp)
    {
        gauge.Dec(decrement, timestamp.ToUnixTimeMilliseconds());
    }

    public static void DecTo(this IGauge<long> gauge, long value, DateTimeOffset timestamp)
    {
        gauge.DecTo(value, timestamp.ToUnixTimeMilliseconds());
    }

    public static void Dec(this IMetricFamily<IGauge<long>> metricFamily, long decrement = 1)
    {
        metricFamily.Unlabelled.Dec(decrement);
    }

    public static void Dec(this IMetricFamily<IGauge<long>> metricFamily, long decrement, long timestamp)
    {
        metricFamily.Unlabelled.Dec(decrement, timestamp);
    }

    public static void Dec(this IMetricFamily<IGauge<long>> metricFamily, long decrement, DateTimeOffset timestamp)
    {
        metricFamily.Unlabelled.Dec(decrement, timestamp);
    }

    public static void DecTo(this IMetricFamily<IGauge<long>> metricFamily, long value)
    {
        metricFamily.Unlabelled.DecTo(value);
    }

    public static void DecTo(this IMetricFamily<IGauge<long>> metricFamily, long value, long timestamp)
    {
        metricFamily.Unlabelled.DecTo(value, timestamp);
    }

    public static void DecTo(this IMetricFamily<IGauge<long>> metricFamily, long value, DateTimeOffset timestamp)
    {
        metricFamily.Unlabelled.DecTo(value, timestamp);
    }

    public static void Set(this IGauge<long> gauge, long val, DateTimeOffset timestamp)
    {
        gauge.Set(val, timestamp.ToUnixTimeMilliseconds());
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
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Inc(increment);
    }

    public static void Inc<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long increment, long timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Inc(increment, timestamp);
    }

    public static void Inc<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long increment, DateTimeOffset timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Inc(increment, timestamp);
    }

    public static void IncTo<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.IncTo(value);
    }

    public static void IncTo<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value, long timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }

    public static void IncTo<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value, DateTimeOffset timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }

    public static void Dec<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long decrement = 1)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Dec(decrement);
    }

    public static void Dec<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long decrement, long timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Dec(decrement, timestamp);
    }

    public static void Dec<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long decrement, DateTimeOffset timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Dec(decrement, timestamp);
    }

    public static void DecTo<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.DecTo(value);
    }

    public static void DecTo<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value, long timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.DecTo(value, timestamp);
    }

    public static void DecTo<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value, DateTimeOffset timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.DecTo(value, timestamp);
    }

    public static void Set<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Set(value);
    }

    public static void Set<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value, long timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Set(value, timestamp);
    }

    public static void Set<TLabels>(this IMetricFamily<IGauge<long>, TLabels> metricFamily, long value, DateTimeOffset timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Set(value, timestamp);
    }
}
