using System;
#if HAS_ITUPLE
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client;

public static class UntypedExtensions
{
    public static void Set(this IUntyped untyped, double val, DateTimeOffset timestamp)
    {
        untyped.Set(val, timestamp.ToUnixTimeMilliseconds());
    }

    public static void Set(this IMetricFamily<IUntyped> metricFamily, double val)
    {
        metricFamily.Unlabelled.Set(val);
    }

    public static void Set(this IMetricFamily<IUntyped> metricFamily, double val, int timestamp)
    {
        metricFamily.Unlabelled.Set(val, timestamp);
    }

    public static void Set(this IMetricFamily<IUntyped> metricFamily, double val, DateTimeOffset timestamp)
    {
        metricFamily.Unlabelled.Set(val, timestamp);
    }

    public static void Set<TLabels>(this IMetricFamily<IUntyped, TLabels> metricFamily, double val)
#if HAS_ITUPLE
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Set(val);
    }

    public static void Set<TLabels>(this IMetricFamily<IUntyped, TLabels> metricFamily, double val, int timestamp)
#if HAS_ITUPLE
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Set(val, timestamp);
    }

    public static void Set<TLabels>(this IMetricFamily<IUntyped, TLabels> metricFamily, double val, DateTimeOffset timestamp)
#if HAS_ITUPLE
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Set(val, timestamp);
    }
}
