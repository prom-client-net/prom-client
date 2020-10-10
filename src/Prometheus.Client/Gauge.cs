using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prometheus.Client.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client
{
    /// <inheritdoc cref="IGauge" />
    public sealed class Gauge : MetricBase<MetricConfiguration>, IGauge
    {
        private ThreadSafeDouble _value;

        internal Gauge(MetricConfiguration configuration, IReadOnlyList<string> labels)
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

            IncInternal(increment, timestamp);
        }

        public void Set(double val)
        {
            Set(val, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(double val, long? timestamp)
        {
            _value.Value = val;
            TrackObservation(timestamp);
        }

        public void Dec()
        {
            IncInternal(-1.0D, null);
        }

        public void Dec(double decrement)
        {
            Dec(decrement, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dec(double decrement, long? timestamp)
        {
            if (ThreadSafeDouble.IsNaN(decrement))
                return;

            IncInternal(-decrement, timestamp);
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
}
