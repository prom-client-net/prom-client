using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

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
            Unlabelled.Inc();
        }

        public void Inc(double increment)
        {
            Unlabelled.Inc(increment);
        }

        public void Inc(double increment, long? timestamp)
        {
            Unlabelled.Inc(increment, timestamp);
        }

        public void Set(double val)
        {
            Unlabelled.Set(val);
        }

        public void Set(double val, long? timestamp)
        {
            Unlabelled.Set(val, timestamp);
        }

        public void Dec()
        {
            Unlabelled.Dec();
        }

        public void Dec(double decrement)
        {
            Unlabelled.Dec(decrement);
        }

        public void Dec(double decrement, long? timestamp)
        {
            Unlabelled.Dec(decrement, timestamp);
        }

        public double Value => Unlabelled.Value;

        public class LabelledGauge : Labelled<MetricConfiguration>, IGauge
        {
            private ThreadSafeDouble _value;

            public void Inc()
            {
                Inc(1.0D, null);
            }

            public void Inc(double increment)
            {
                Inc(increment, null);
            }

            public void Inc(double increment, long? timestamp)
            {
                if (double.IsNaN(increment))
                    return;

                _value.Add(increment);
                TimestampIfRequired(timestamp);
            }

            public void Set(double val)
            {
                Set(val, null);
            }

            public void Set(double val, long? timestamp)
            {
                _value.Value = val;
                TimestampIfRequired(timestamp);
            }

            public void Dec()
            {
                Dec(1.0D, null);
            }

            public void Dec(double decrement)
            {
                Dec(decrement, null);
            }

            public void Dec(double decrement, long? timestamp)
            {
                if (double.IsNaN(decrement))
                    return;

                _value.Add(-decrement);
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
