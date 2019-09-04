using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prometheus.Client.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client
{
    /// <inheritdoc cref="IUntyped" />
    public sealed class Untyped : MetricBase<MetricConfiguration>, IUntyped
    {
        internal Untyped(MetricConfiguration configuration, IReadOnlyList<string> labels)
            : base(configuration, labels)
        {
        }

        private ThreadSafeDouble _value;

        public void Set(double val)
        {
            Set(val, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(double val, long? timestamp)
        {
            _value.Value = val;
            TimestampIfRequired(timestamp);
        }

        public double Value => _value.Value;

        protected internal override void Collect(IMetricsWriter writer)
        {
            writer.WriteSample(Value, string.Empty, Labels, Timestamp);
        }
    }
}
