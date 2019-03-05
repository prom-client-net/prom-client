using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client
{
    public enum DurationUnit
    {
        Milliseconds,
        Seconds,
    }

    public static class ValueObserverExtensions
    {
        public static void ObserveDuration(this IValueObserver observer, Action method, DurationUnit unit = DurationUnit.Seconds)
        {
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

        public static T ObserveDuration<T>(this IValueObserver observer, Func<T> method, DurationUnit unit = DurationUnit.Seconds)
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

        public async static Task ObserveDurationAsync(this IValueObserver observer, Func<Task> method, DurationUnit unit = DurationUnit.Seconds)
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

        public async static Task<T> ObserveDurationAsync<T>(this IValueObserver observer, Func<Task<T>> method, DurationUnit unit = DurationUnit.Seconds)
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
