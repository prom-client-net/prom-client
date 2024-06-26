using System;
#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client;

public static class CounterInt64Extensions
{
    public static void Inc(this ICounter<long> counter, long increment, DateTimeOffset timestamp)
    {
        counter.Inc(increment, timestamp.ToUnixTimeMilliseconds());
    }

    public static void IncTo(this ICounter<long> counter, long value, DateTimeOffset timestamp)
    {
        counter.IncTo(value, timestamp.ToUnixTimeMilliseconds());
    }

    public static void Inc(this IMetricFamily<ICounter<long>> metricFamily, long increment = 1)
    {
        metricFamily.Unlabelled.Inc(increment);
    }

    public static void Inc(this IMetricFamily<ICounter<long>> metricFamily, long increment, long timestamp)
    {
        metricFamily.Unlabelled.Inc(increment, timestamp);
    }

    public static void Inc(this IMetricFamily<ICounter<long>> metricFamily, long increment, DateTimeOffset timestamp)
    {
        metricFamily.Unlabelled.Inc(increment, timestamp);
    }

    public static void IncTo(this IMetricFamily<ICounter<long>> metricFamily, long value)
    {
        metricFamily.Unlabelled.IncTo(value);
    }

    public static void IncTo(this IMetricFamily<ICounter<long>> metricFamily, long value, long timestamp)
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }

    public static void IncTo(this IMetricFamily<ICounter<long>> metricFamily, long value, DateTimeOffset timestamp)
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }

    public static void Inc<TLabels>(this IMetricFamily<ICounter<long>, TLabels> metricFamily, long increment = 1)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Inc(increment);
    }

    public static void Inc<TLabels>(this IMetricFamily<ICounter<long>, TLabels> metricFamily, long increment, long timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Inc(increment, timestamp);
    }

    public static void Inc<TLabels>(this IMetricFamily<ICounter<long>, TLabels> metricFamily, long increment, DateTimeOffset timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Inc(increment, timestamp);
    }

    public static void IncTo<TLabels>(this IMetricFamily<ICounter<long>, TLabels> metricFamily, long value)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.IncTo(value);
    }

    public static void IncTo<TLabels>(this IMetricFamily<ICounter<long>, TLabels> metricFamily, long value, long timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }

    public static void IncTo<TLabels>(this IMetricFamily<ICounter<long>, TLabels> metricFamily, long value, DateTimeOffset timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }
}
