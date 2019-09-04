using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client
{
    public static class ValueObserverExtensions
    {
        public static void ObserveDuration(this IValueObserver observer, Action method)
        {
            ObserveDuration(observer, method, DurationUnit.Seconds);
        }

        public static void ObserveDuration(this IValueObserver observer, Action method, DurationUnit unit)
        {
            // TODO: avoid allocation by using Stopwatch.GetTimestamp
            var stopwatch = Stopwatch.StartNew();
            try
            {
                method();
            }
            finally
            {
                observer.Observe(GetDurationValue(stopwatch.Elapsed, unit));
            }
        }

        public static T ObserveDuration<T>(this IValueObserver observer, Func<T> method)
        {
            return ObserveDuration(observer, method, DurationUnit.Seconds);
        }

        public static T ObserveDuration<T>(this IValueObserver observer, Func<T> method, DurationUnit unit)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                return method();
            }
            finally
            {
                observer.Observe(GetDurationValue(stopwatch.Elapsed, unit));
            }
        }

        public static Task ObserveDurationAsync(this IValueObserver observer, Func<Task> method)
        {
            return ObserveDurationAsync(observer, method, DurationUnit.Seconds);
        }

        public static async Task ObserveDurationAsync(this IValueObserver observer, Func<Task> method, DurationUnit unit)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await method().ConfigureAwait(false);
            }
            finally
            {
                observer.Observe(GetDurationValue(stopwatch.Elapsed, unit));
            }
        }

        public static Task<T> ObserveDurationAsync<T>(this IValueObserver observer, Func<Task<T>> method)
        {
            return ObserveDurationAsync(observer, method, DurationUnit.Seconds);
        }

        public static async Task<T> ObserveDurationAsync<T>(this IValueObserver observer, Func<Task<T>> method, DurationUnit unit)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                return await method().ConfigureAwait(false);
            }
            finally
            {
                observer.Observe(GetDurationValue(stopwatch.Elapsed, unit));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double GetDurationValue(TimeSpan span, DurationUnit unit)
        {
            switch (unit)
            {
                case DurationUnit.Milliseconds:
                    return span.TotalMilliseconds;
                case DurationUnit.Seconds:
                    return span.TotalSeconds;
                default:
                    return -1;
            }
        }
    }
}
