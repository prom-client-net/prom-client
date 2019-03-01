using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tools;

namespace Prometheus.Client
{
    /// <inheritdoc cref="IUntyped" />
    public class Untyped : Collector<Untyped.LabelledUntyped, MetricConfiguration>, IUntyped
    {
        internal Untyped(MetricConfiguration configuration)
            : base(configuration)
        {
        }

        protected override MetricType Type => MetricType.Untyped;

        public void Set(double val)
        {
            Unlabelled.Set(val);
        }

        public double Value => Unlabelled.Value;

        public class LabelledUntyped : Labelled<MetricConfiguration>, IUntyped
        {
            private ThreadSafeDouble _value;

            public void Set(double val)
            {
                _value.Value = val;
                TimestampIfRequired();
            }

            public double Value => _value.Value;

            protected internal override void Collect(IMetricsWriter writer)
            {
                writer.WriteSample(Value, string.Empty, Labels, Timestamp);
            }
        }
    }
}
