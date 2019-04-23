using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

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

        public void Set(double val, long? timestamp)
        {
            Unlabelled.Set(val, timestamp);
        }

        public double Value => Unlabelled.Value;

        public class LabelledUntyped : Labelled<MetricConfiguration>, IUntyped
        {
            private ThreadSafeDouble _value;

            public void Set(double val)
            {
                Set(val, null);
            }

            public void Set(double val, long? timestamp)
            {
                if (double.IsNaN(val))
                    return;

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
}
