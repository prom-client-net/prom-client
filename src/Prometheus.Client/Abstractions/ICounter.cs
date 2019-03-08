using System;
using Prometheus.Client.Tools;

namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Counter metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#counter
    ///     </remarks>
    /// </summary>
    public interface ICounter<T>
    {
        T Value { get; }

        void Inc();

        void Inc(T increment);

        void Inc(T increment, long? timestamp);
    }

    public interface ICounter : ICounter<double>
    {
    }

    public static class CounterExtensions
    {
        public static void Inc<T>(this ICounter<T> counter, T increment, DateTime timestamp)
        {
            counter.Inc(increment, timestamp.ToUnixTime());
        }

        public static void Inc<T>(this ICounter<T> counter, T increment, DateTimeOffset timestamp)
        {
            counter.Inc(increment, timestamp.ToUnixTime());
        }
    }
}
