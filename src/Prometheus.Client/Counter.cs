using System;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client
{
    /// <inheritdoc cref="ICounter" />
    public class Counter : Collector<Counter.LabelledCounter, MetricConfiguration>, ICounter
    {
        internal Counter(MetricConfiguration configuration)
            : base(configuration)
        {
        }

        protected override MetricType Type => MetricType.Counter;

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

        public double Value => Unlabelled.Value;

        public void Reset()
        {
            Unlabelled.ResetValue();
            foreach (var labelledMetric in LabelledMetrics)
                labelledMetric.Value.ResetValue();
        }

        public class LabelledCounter : Labelled<MetricConfiguration>, ICounter
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

                if (increment < 0.0D)
                    throw new ArgumentOutOfRangeException(nameof(increment), "Counter cannot go down");

                _value.Add(increment);
                TimestampIfRequired(timestamp);
            }

            public double Value => _value.Value;

            protected internal override void Collect(IMetricsWriter writer)
            {
                writer.WriteSample(Value, string.Empty, Labels, Timestamp);
            }

            internal void ResetValue()
            {
                _value.Value = 0.0D;
            }
        }
    }
}
