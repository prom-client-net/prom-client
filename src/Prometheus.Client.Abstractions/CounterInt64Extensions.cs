using System;
#if HasITuple
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client
{
    public static class CounterInt64Extensions
    {
        public static void Inc(this ICounter<long> counter, long increment, DateTimeOffset timestamp)
        {
            counter.Inc(increment, timestamp.ToUnixTimeMilliseconds());
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

        public static void Inc<TLabels>(this IMetricFamily<ICounter<long>, TLabels> metricFamily, long increment = 1)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment);
        }

        public static void Inc<TLabels>(this IMetricFamily<ICounter<long>, TLabels> metricFamily, long increment, long timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment, timestamp);
        }

        public static void Inc<TLabels>(this IMetricFamily<ICounter<long>, TLabels> metricFamily, long increment, DateTimeOffset timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment, timestamp);
        }

        public static IMetricFamily<ICounter<long>, ValueTuple<string>> CreateCounterInt64(this IMetricFactory factory, string name, string help, string labelName, bool includeTimestamp = false)
        {
            return factory.CreateCounterInt64(name, help, ValueTuple.Create(labelName), includeTimestamp);
        }
    }
}
