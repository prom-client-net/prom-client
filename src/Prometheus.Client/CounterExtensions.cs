using System;
#if HasITuple
using System.Runtime.CompilerServices;
#endif
using Prometheus.Client.Abstractions;

namespace Prometheus.Client
{
    public static class CounterExtensions
    {
        public static void Inc(this ICounter counter, double increment, DateTimeOffset timestamp)
        {
            counter.Inc(increment, timestamp.ToUnixTime());
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

        public static void Inc<TLabels>(this IMetricFamily<ICounter, TLabels> metricFamily, double increment = 1)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment);
        }

        public static void Inc<TLabels>(this IMetricFamily<ICounter, TLabels> metricFamily, double increment, long timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment, timestamp);
        }

        public static void Inc<TLabels>(this IMetricFamily<ICounter, TLabels> metricFamily, double increment, DateTimeOffset timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Inc(increment, timestamp);
        }

        public static IMetricFamily<ICounter, ValueTuple<string>> CreateCounter(this MetricFactory factory, string name, string help, string labelName, MetricFlags options = MetricFlags.Default)
        {
            return factory.CreateCounter(name, help, ValueTuple.Create(labelName), options);
        }
    }
}
