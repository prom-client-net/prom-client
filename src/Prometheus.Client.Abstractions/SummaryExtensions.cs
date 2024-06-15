using System;
using System.Collections.Generic;
#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client;

public static class SummaryExtensions
{
    public static void Observe(this ISummary observer, double val, DateTimeOffset timestamp)
    {
        observer.Observe(val, timestamp.ToUnixTimeMilliseconds());
    }

    public static void Observe(this IMetricFamily<ISummary> metricFamily, double val)
    {
        metricFamily.Unlabelled.Observe(val);
    }

    public static void Observe(this IMetricFamily<ISummary> metricFamily, double val, long timestamp)
    {
        metricFamily.Unlabelled.Observe(val, timestamp);
    }

    public static void Observe(this IMetricFamily<ISummary> metricFamily, double val, DateTimeOffset timestamp)
    {
        metricFamily.Unlabelled.Observe(val, timestamp.ToUnixTimeMilliseconds());
    }

    public static void Observe<TLabels>(this IMetricFamily<ISummary, TLabels> metricFamily, double val)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Observe(val);
    }

    public static void Observe<TLabels>(this IMetricFamily<ISummary, TLabels> metricFamily, double val, long timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Observe(val, timestamp);
    }

    public static void Observe<TLabels>(this IMetricFamily<ISummary, TLabels> metricFamily, double val, DateTimeOffset timestamp)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        metricFamily.Unlabelled.Observe(val, timestamp.ToUnixTimeMilliseconds());
    }
}
