using System;
using Prometheus.Client.Tools;

namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Untyped metric type
    /// </summary>
    public interface IUntyped
    {
        double Value { get; }

        void Set(double val);

        void Set(double val, long? timestamp);
    }

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
