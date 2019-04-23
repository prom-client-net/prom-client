using System;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client
{
    public static class CounterExtensions
    {
        public static void Inc<T>(this ICounter<T> counter, T increment, DateTime timestamp)
            where T : struct
        {
            counter.Inc(increment, timestamp.ToUnixTime());
        }

        public static void Inc<T>(this ICounter<T> counter, T increment, DateTimeOffset timestamp)
            where T : struct
        {
            counter.Inc(increment, timestamp.ToUnixTime());
        }
    }
}
