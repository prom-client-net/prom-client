using System;
using System.Collections.Generic;
#if HasITuple
using System.Runtime.CompilerServices;
#endif
using Prometheus.Client.Abstractions;
using Prometheus.Client.SummaryImpl;

namespace Prometheus.Client
{
    public static class SummaryExtensions
    {
        public static void Observe(this ISummary observer, double val, DateTimeOffset timestamp)
        {
            observer.Observe(val, timestamp.ToUnixTime());
        }

        public static void Observe(this IMetricFamily<ISummary> metricFamily, double val)
        {
            metricFamily.Unlabelled.Observe(val);
        }

        public static void Observe(this IMetricFamily<ISummary> metricFamily, double val, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.Observe(val, timestamp.ToUnixTime());
        }

        public static void Observe<TLabels>(this IMetricFamily<ISummary, TLabels> metricFamily, double val)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Observe(val);
        }

        public static void Observe<TLabels>(this IMetricFamily<ISummary, TLabels> metricFamily, double val, DateTimeOffset timestamp)
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Observe(val, timestamp.ToUnixTime());
        }

        public static IMetricFamily<ISummary, ValueTuple<string>> CreateSummary(
            this MetricFactory factory,
            string name,
            string help,
            string labelName,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null,
            MetricFlags options = MetricFlags.Default)
        {
            return factory.CreateSummary(name, help, ValueTuple.Create(labelName), objectives, maxAge, ageBuckets, bufCap, options);
        }
    }
}
