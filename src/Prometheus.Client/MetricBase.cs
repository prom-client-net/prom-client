using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client;

public abstract class MetricBase<TConfig>
    where TConfig: MetricConfiguration
{
    protected readonly TConfig Configuration;
    private readonly bool _includeTimestamp;
    private readonly bool _computeTimestamp;
    private readonly TimeSpan _timeToLive;
    private readonly Func<DateTimeOffset> _currentTimeProvider;
    private long _timestamp;

    protected MetricBase(TConfig config, IReadOnlyList<string> labelValues, Func<DateTimeOffset> currentTimeProvider = null)
    {
        Configuration = config;
        _currentTimeProvider = currentTimeProvider;
        _timeToLive = config.TimeToLive;
        _includeTimestamp = config.IncludeTimestamp;
        _computeTimestamp = _includeTimestamp || _timeToLive > TimeSpan.Zero;

        LabelValues = labelValues;
    }

    protected internal IReadOnlyList<string> LabelValues { get; }

    protected internal long? Timestamp
    {
        get
        {
            if (!_includeTimestamp)
                return null;

            return Interlocked.Read(ref _timestamp);
        }
    }

    protected internal abstract void Collect(IMetricsWriter writer);

    protected DateTimeOffset GetUtcNow()
    {
        if (_currentTimeProvider == null)
            return DateTimeOffset.UtcNow;

        return _currentTimeProvider();
    }

    public bool IsExpired()
    {
        if (_timeToLive == TimeSpan.Zero)
            return false;

        long now = GetUtcNow().ToUnixTimeMilliseconds();
        long last = Interlocked.Read(ref _timestamp);
        return TimeSpan.FromMilliseconds(now - last) > _timeToLive;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void TrackObservation(long? timestamp = null)
    {
        if (!_computeTimestamp)
            return;

        long now = GetUtcNow().ToUnixTimeMilliseconds();

        if (!timestamp.HasValue)
        {
            // no needs to check anything, null means now.
            Interlocked.Exchange(ref _timestamp, now);
            return;
        }

        // use now if provided timestamp is in future
        if (timestamp > now)
        {
            Interlocked.Exchange(ref _timestamp, now);
            return;
        }

        // use provided timestamp unless current timestamp is bigger
        while (true)
        {
            long current = Interlocked.Read(ref _timestamp);
            if (current > timestamp)
                return;

            if (current == Interlocked.CompareExchange(ref _timestamp, timestamp.Value, current))
                return;
        }
    }
}
