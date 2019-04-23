using System;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client.Extensions
{
    public static class UntypedExtensions
    {
        public static void Set(this IUntyped untyped, double val, DateTime timestamp)
        {
            untyped.Set(val, timestamp.ToUnixTime());
        }

        public static void Set(this IUntyped untyped, double val, DateTimeOffset timestamp)
        {
            untyped.Set(val, timestamp.ToUnixTime());
        }
    }
}
