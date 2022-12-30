using System;
#if HasITuple
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client
{
    public static class HistogramExtensions
    {
        public static void Observe(this IHistogram observer, double val, DateTimeOffset timestamp)
        {
            observer.Observe(val, timestamp.ToUnixTimeMilliseconds());
        }

        public static void Observe(this IMetricFamily<IHistogram> metricFamily, double val)
        {
            metricFamily.Unlabelled.Observe(val);
        }

        public static void Observe(this IMetricFamily<IHistogram> metricFamily, double val, long timestamp)
        {
            metricFamily.Unlabelled.Observe(val, timestamp);
        }

        public static void Observe(this IMetricFamily<IHistogram> metricFamily, double val, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.Observe(val, timestamp.ToUnixTimeMilliseconds());
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

        public static void Observe<TLabels>(this IMetricFamily<IHistogram, TLabels> metricFamily, double val, long timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Observe(val, timestamp);
        }

        public static void Observe<TLabels>(this IMetricFamily<IHistogram, TLabels> metricFamily, double val, DateTimeOffset timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Observe(val, timestamp.ToUnixTimeMilliseconds());
        }
    }
}
