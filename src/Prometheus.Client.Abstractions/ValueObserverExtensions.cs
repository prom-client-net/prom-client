using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Prometheus.Client;

public static class ValueObserverExtensions
{
    public static void ObserveDuration(this IValueObserver observer, Action method)
    {
        var ts = Stopwatch.GetTimestamp();
        try
        {
            method();
        }
        finally
        {
            observer.Observe(GetDuration(ts));
        }
    }

    public static void ObserveDuration(this IValueObserver observer, Action method, Action<Exception> onObserveException)
    {
        var ts = Stopwatch.GetTimestamp();
        try
        {
            method();
        }
        finally
        {
            ObserveHandlingException(observer, GetDuration(ts), onObserveException);
        }
    }

    public static T ObserveDuration<T>(this IValueObserver observer, Func<T> method)
    {
        var ts = Stopwatch.GetTimestamp();
        try
        {
            return method();
        }
        finally
        {
            observer.Observe(GetDuration(ts));
        }
    }

    public static T ObserveDuration<T>(this IValueObserver observer, Func<T> method, Action<Exception> onObserveException)
    {
        var ts = Stopwatch.GetTimestamp();
        try
        {
            return method();
        }
        finally
        {
            ObserveHandlingException(observer, GetDuration(ts), onObserveException);
        }
    }

    public static async Task ObserveDurationAsync(this IValueObserver observer, Func<Task> method)
    {
        var ts = Stopwatch.GetTimestamp();
        try
        {
            await method().ConfigureAwait(false);
        }
        finally
        {
            observer.Observe(GetDuration(ts));
        }
    }

    public static async Task ObserveDurationAsync(this IValueObserver observer, Func<Task> method, Action<Exception> onObserveException)
    {
        var ts = Stopwatch.GetTimestamp();
        try
        {
            await method().ConfigureAwait(false);
        }
        finally
        {
            ObserveHandlingException(observer, GetDuration(ts), onObserveException);
        }
    }

    public static async Task<T> ObserveDurationAsync<T>(this IValueObserver observer, Func<Task<T>> method)
    {
        var ts = Stopwatch.GetTimestamp();
        try
        {
            return await method().ConfigureAwait(false);
        }
        finally
        {
            observer.Observe(GetDuration(ts));
        }
    }

    public static async Task<T> ObserveDurationAsync<T>(this IValueObserver observer, Func<Task<T>> method, Action<Exception> onObserveException)
    {
        var ts = Stopwatch.GetTimestamp();
        try
        {
            return await method().ConfigureAwait(false);
        }
        finally
        {
            ObserveHandlingException(observer, GetDuration(ts), onObserveException);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double GetDuration(long startTs)
    {
        var elapsed = Stopwatch.GetTimestamp() - startTs;
        var frequency = (double)Stopwatch.Frequency;

        return elapsed / frequency;
    }

    private static void ObserveHandlingException(IValueObserver observer, double duration, Action<Exception> onObserveException)
    {
        if (onObserveException is null)
        {
            observer.Observe(duration);
            return;
        }

        try
        {
            observer.Observe(duration);
        }
        catch (Exception ex)
        {
            try
            {
                onObserveException(ex);
            }
            catch
            {
                // Swallow exceptions from the handler so ObserveDuration* does not throw due to handler failures
            }
        }
    }
}
