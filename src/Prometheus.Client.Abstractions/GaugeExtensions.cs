using System;
#if HasITuple
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client
{
    public static class GaugeExtensions
    {
        public static void Inc(this IGauge gauge, double increment, DateTimeOffset timestamp)
        {
            gauge.Inc(increment, timestamp.ToUnixTimeMilliseconds());
        }

        public static void IncTo(this IGauge gauge, double value, DateTimeOffset timestamp)
        {
            gauge.IncTo(value, timestamp.ToUnixTimeMilliseconds());
        }

        public static void Inc(this IMetricFamily<IGauge> metricFamily, double increment = 1)
        {
            metricFamily.Unlabelled.Inc(increment);
        }

        public static void Inc(this IMetricFamily<IGauge> metricFamily, double increment, long timestamp)
        {
            metricFamily.Unlabelled.Inc(increment, timestamp);
        }

        public static void Inc(this IMetricFamily<IGauge> metricFamily, double increment, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.Inc(increment, timestamp.ToUnixTimeMilliseconds());
        }

        public static void IncTo(this IMetricFamily<IGauge> metricFamily, double value)
        {
            metricFamily.Unlabelled.IncTo(value);
        }

        public static void IncTo(this IMetricFamily<IGauge> metricFamily, double value, long timestamp)
        {
            metricFamily.Unlabelled.IncTo(value, timestamp);
        }

        public static void IncTo(this IMetricFamily<IGauge> metricFamily, double value, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.IncTo(value, timestamp.ToUnixTimeMilliseconds());
        }

        public static void Dec(this IGauge gauge, double decrement, DateTimeOffset timestamp)
        {
            gauge.Dec(decrement, timestamp.ToUnixTimeMilliseconds());
        }

        public static void DecTo(this IGauge gauge, double value, DateTimeOffset timestamp)
        {
            gauge.DecTo(value, timestamp.ToUnixTimeMilliseconds());
        }

        public static void Dec(this IMetricFamily<IGauge> metricFamily, double decrement = 1)
        {
            metricFamily.Unlabelled.Dec(decrement);
        }

        public static void Dec(this IMetricFamily<IGauge> metricFamily, double decrement, long timestamp)
        {
            metricFamily.Unlabelled.Dec(decrement, timestamp);
        }

        public static void Dec(this IMetricFamily<IGauge> metricFamily, double decrement, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.Dec(decrement, timestamp);
        }

        public static void DecTo(this IMetricFamily<IGauge> metricFamily, double value)
        {
            metricFamily.Unlabelled.DecTo(value);
        }

        public static void DecTo(this IMetricFamily<IGauge> metricFamily, double value, long timestamp)
        {
            metricFamily.Unlabelled.DecTo(value, timestamp);
        }

        public static void DecTo(this IMetricFamily<IGauge> metricFamily, double value, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.DecTo(value, timestamp);
        }

        public static void Set(this IGauge gauge, double val, DateTimeOffset timestamp)
        {
            gauge.Set(val, timestamp.ToUnixTimeMilliseconds());
        }

        public static void Set(this IMetricFamily<IGauge> metricFamily, double value)
        {
            metricFamily.Unlabelled.Set(value);
        }

        public static void Set(this IMetricFamily<IGauge> metricFamily, double value, long timestamp)
        {
            metricFamily.Unlabelled.Set(value, timestamp);
        }

        public static void Set(this IMetricFamily<IGauge> metricFamily, double value, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.Set(value, timestamp);
        }

        public static void Inc<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double increment = 1)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment);
        }

        public static void Inc<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double increment, long timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment, timestamp);
        }

        public static void Inc<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double increment, DateTimeOffset timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment, timestamp);
        }

        public static void IncTo<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double value)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.IncTo(value);
        }

        public static void IncTo<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double value, long timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.IncTo(value, timestamp);
        }

        public static void IncTo<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double value, DateTimeOffset timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.IncTo(value, timestamp);
        }

        public static void Dec<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double decrement = 1)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Dec(decrement);
        }

        public static void Dec<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double decrement, long timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Dec(decrement, timestamp);
        }

        public static void Dec<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double decrement, DateTimeOffset timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Dec(decrement, timestamp);
        }

        public static void DecTo<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double value)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.DecTo(value);
        }

        public static void DecTo<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double value, long timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.DecTo(value, timestamp);
        }

        public static void DecTo<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double value, DateTimeOffset timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
            where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.DecTo(value, timestamp);
        }

        public static void Set<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double value)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Set(value);
        }

        public static void Set<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double value, long timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Set(value, timestamp);
        }

        public static void Set<TLabels>(this IMetricFamily<IGauge, TLabels> metricFamily, double value, DateTimeOffset timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Set(value, timestamp);
        }
    }
}
