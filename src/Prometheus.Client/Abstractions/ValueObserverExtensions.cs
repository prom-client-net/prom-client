using System;
using Prometheus.Client.Tools;

namespace Prometheus.Client.Abstractions
{
    public static class ValueObserverExtensions
    {
        public static void Observe(this IValueObserver observer, double val, DateTime timestamp)
        {
            observer.Observe(val, timestamp.ToUnixTime());
        }

        public static void Observe(this IValueObserver observer, double val, DateTimeOffset timestamp)
        {
            observer.Observe(val, timestamp.ToUnixTime());
        }
    }
}