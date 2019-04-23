using System;
using Prometheus.Client.Tools;

namespace Prometheus.Client.Abstractions
{
    public static class GaugeExtensions
    {
        public static void Inc(this IGauge gauge, double increment, DateTime timestamp)
        {
            gauge.Inc(increment, timestamp.ToUnixTime());
        }

        public static void Inc(this IGauge gauge, double increment, DateTimeOffset timestamp)
        {
            gauge.Inc(increment, timestamp.ToUnixTime());
        }

        public static void Set(this IGauge gauge, double val, DateTime timestamp)
        {
            gauge.Set(val, timestamp.ToUnixTime());
        }

        public static void Set(this IGauge gauge, double val, DateTimeOffset timestamp)
        {
            gauge.Set(val, timestamp.ToUnixTime());
        }

        public static void Dec(this IGauge gauge, double decrement, DateTime timestamp)
        {
            gauge.Dec(decrement, timestamp.ToUnixTime());
        }

        public static void Dec(this IGauge gauge, double decrement, DateTimeOffset timestamp)
        {
            gauge.Dec(decrement, timestamp.ToUnixTime());
        }
    }
}