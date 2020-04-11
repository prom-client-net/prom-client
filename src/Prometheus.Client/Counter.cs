using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prometheus.Client.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client
{
    /// <inheritdoc cref="ICounter" />
    public sealed class Counter : MetricBase<MetricConfiguration>, ICounter
    {
        private ThreadSafeDouble _value = default;

        public Counter(MetricConfiguration configuration, IReadOnlyList<string> labels)
            : base(configuration, labels)
        {
        }

        public void Inc()
        {
            Inc(1.0D, null);
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
                ThrowInvalidIncArgument();

            _value.Add(increment);
            TimestampIfRequired(timestamp);
        }

        public double Value => _value.Value;

        protected internal override void Collect(IMetricsWriter writer)
        {
            writer.WriteSample(Value, string.Empty, Labels, Timestamp);
        }

        private void ThrowInvalidIncArgument()
        {
            throw new ArgumentOutOfRangeException("increment", "Counter cannot go down");
        }
    }
}
