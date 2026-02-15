using System;
#if HAS_ITUPLE
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client;

public static class CounterExtensions
{
    public static void Inc(this ICounter counter, double increment, DateTimeOffset timestamp)
    {
        counter.Inc(increment, timestamp.ToUnixTimeMilliseconds());
    }

    public static void IncTo(this ICounter counter, double value, DateTimeOffset timestamp)
    {
        counter.IncTo(value, timestamp.ToUnixTimeMilliseconds());
    }

    public static void Inc(this IMetricFamily<ICounter> metricFamily, double increment = 1)
    {
        metricFamily.Unlabelled.Inc(increment);
    }

    public static void Inc(this IMetricFamily<ICounter> metricFamily, double increment, long timestamp)
    {
        metricFamily.Unlabelled.Inc(increment, timestamp);
    }

    public static void Inc(this IMetricFamily<ICounter> metricFamily, double increment, DateTimeOffset timestamp)
    {
        metricFamily.Unlabelled.Inc(increment, timestamp);
    }

    public static void IncTo(this IMetricFamily<ICounter> metricFamily, double value)
    {
        metricFamily.Unlabelled.IncTo(value);
    }

    public static void IncTo(this IMetricFamily<ICounter> metricFamily, double value, long timestamp)
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }

    public static void IncTo(this IMetricFamily<ICounter> metricFamily, double value, DateTimeOffset timestamp)
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }

    public static void Inc<TLabels>(this IMetricFamily<ICounter, TLabels> metricFamily, double increment = 1)
#if HAS_ITUPLE
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Inc(increment);
    }

    public static void Inc<TLabels>(this IMetricFamily<ICounter, TLabels> metricFamily, double increment, long timestamp)
#if HAS_ITUPLE
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Inc(increment, timestamp);
    }

    public static void Inc<TLabels>(this IMetricFamily<ICounter, TLabels> metricFamily, double increment, DateTimeOffset timestamp)
#if HAS_ITUPLE
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Inc(increment, timestamp);
    }

    public static void IncTo<TLabels>(this IMetricFamily<ICounter, TLabels> metricFamily, double value)
#if HAS_ITUPLE
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.IncTo(value);
    }

    public static void IncTo<TLabels>(this IMetricFamily<ICounter, TLabels> metricFamily, double value, long timestamp)
#if HAS_ITUPLE
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }

    public static void IncTo<TLabels>(this IMetricFamily<ICounter, TLabels> metricFamily, double value, DateTimeOffset timestamp)
#if HAS_ITUPLE
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.IncTo(value, timestamp);
    }
}
