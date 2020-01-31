using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client
{
    public static class ValueObserverExtensions
    {
        public static void ObserveDuration(this IValueObserver observer, Action method, DurationUnit unit = DurationUnit.Seconds)
        {
            var ts = Stopwatch.GetTimestamp();
            try
            {
                method();
            }
            finally
            {
                observer.Observe(GetDuration(ts, unit));
            }
        }

        public static T ObserveDuration<T>(this IValueObserver observer, Func<T> method, DurationUnit unit = DurationUnit.Seconds)
        {
            var ts = Stopwatch.GetTimestamp();
            try
            {
                return method();
            }
            finally
            {
                observer.Observe(GetDuration(ts, unit));
            }
        }

        public static async Task ObserveDurationAsync(this IValueObserver observer, Func<Task> method, DurationUnit unit = DurationUnit.Seconds)
        {
            var ts = Stopwatch.GetTimestamp();
            try
            {
                await method().ConfigureAwait(false);
            }
            finally
            {
                observer.Observe(GetDuration(ts, unit));
            }
        }

        public static async Task<T> ObserveDurationAsync<T>(this IValueObserver observer, Func<Task<T>> method, DurationUnit unit = DurationUnit.Seconds)
        {
            var ts = Stopwatch.GetTimestamp();
            try
            {
                return await method().ConfigureAwait(false);
            }
            finally
            {
                observer.Observe(GetDuration(ts, unit));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double GetDuration(long startTs, DurationUnit unit)
        {
            var elapsed = Stopwatch.GetTimestamp() - startTs;
            var frequency = (double)Stopwatch.Frequency / (unit == DurationUnit.Seconds ? 1 : 1000);

            return elapsed / frequency;
        }
    }
}
