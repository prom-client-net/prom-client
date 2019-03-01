using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tools;

namespace Prometheus.Client
{
    /// <inheritdoc cref="IGauge" />
    public class Gauge : Collector<Gauge.LabelledGauge, MetricConfiguration>, IGauge
    {
        internal Gauge(MetricConfiguration configuration)
            : base(configuration)
        {
        }

        protected override MetricType Type => MetricType.Gauge;

        public void Inc()
        {
            Inc(1.0D);
        }

        public void Inc(double increment)
        {
            Unlabelled.Inc(increment);
        }

        public void Set(double val)
        {
            Unlabelled.Set(val);
        }

        public void Dec()
        {
            Dec(1.0D);
        }

        public void Dec(double decrement)
        {
            Unlabelled.Dec(decrement);
        }

        public double Value => Unlabelled.Value;

        public class LabelledGauge : Labelled<MetricConfiguration>, IGauge
        {
            private ThreadSafeDouble _value;

            public void Inc()
            {
                Inc(1.0D);
            }

            public void Inc(double increment)
            {
                _value.Add(increment);
                TimestampIfRequired();
            }

            public void Set(double val)
            {
                _value.Value = val;
                TimestampIfRequired();
            }

            public void Dec()
            {
                Dec(1.0D);
            }

            public void Dec(double decrement)
            {
                Inc(-decrement);
            }

            public double Value => _value.Value;

            protected internal override void Collect(IMetricsWriter writer)
            {
                writer.WriteSample(Value, string.Empty, Labels, Timestamp);
            }
        }
    }
}
