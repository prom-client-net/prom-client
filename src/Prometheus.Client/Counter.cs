using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client;

/// <inheritdoc cref="ICounter" />
public sealed class Counter : MetricBase<MetricConfiguration>, ICounter
{
    private ThreadSafeDouble _value;

    public Counter(MetricConfiguration configuration, IReadOnlyList<string> labels)
        : base(configuration, labels)
    {
    }

    public void Inc()
    {
        IncInternal(1.0D, null);
    }

    public void Inc(double increment)
    {
        Inc(increment, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Inc(double increment, long? timestamp)
    {
        if (ThreadSafeDouble.IsNaN(increment))
            return;

        if (increment < 0.0D)
            throw new ArgumentOutOfRangeException(nameof(increment), "Counter cannot go down");

        IncInternal(increment, timestamp);
    }

    public void IncTo(double value)
        => IncTo(value, null);

    public void IncTo(double value, long? timestamp)
    {
        _value.IncTo(value);
        TrackObservation(timestamp);
    }

    public double Value => _value.Value;

    public void Reset()
    {
        _value.Value = default;
    }

    protected internal override void Collect(IMetricsWriter writer)
    {
        writer.WriteSample(Value, string.Empty, Configuration.LabelNames, LabelValues, Timestamp);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void IncInternal(double increment, long? timestamp)
    {
        _value.Add(increment);
        TrackObservation(timestamp);
    }
}
