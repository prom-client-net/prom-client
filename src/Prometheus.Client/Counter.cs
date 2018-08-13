using System;
using Prometheus.Client.Collectors;
using Prometheus.Client.Contracts;
using Prometheus.Client.Internal;

namespace Prometheus.Client
{
    /// <summary>
    ///     Counter metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#counter
    ///     </remarks>
    /// </summary>
    public interface ICounter
    {
        void Inc();
        void Inc(double increment);
        double Value { get; }
    }

    /// <inheritdoc cref="ICounter" />
    public class Counter : Collector<Counter.ThisChild>, ICounter
    {
        internal Counter(string name, string help, string[] labelNames)
            : base(name, help, labelNames)
        {
        }

        public void Inc()
        {
            Inc(1.0D);
        }

        public void Inc(double increment)
        {
            Unlabelled.Inc(increment);
        }

        public double Value => Unlabelled.Value;

        public void Reset()
        {
            Unlabelled.ResetValue();
            foreach (var labelledMetric in LabelledMetrics)
            {
                labelledMetric.Value.ResetValue();
            }
        }

        protected override CMetricType Type => CMetricType.Counter;

        public class ThisChild : Child, ICounter
        {
            private ThreadSafeDouble _value;

            protected override void Populate(CMetric cMetric)
            {
                cMetric.CCounter = new CCounter { Value = Value };
            }

            public void Inc()
            {
                Inc(1.0D);
            }

            public void Inc(double increment)
            {
                if (increment < 0.0D)
                    throw new ArgumentOutOfRangeException(nameof(increment), "Counter cannot go down");

                _value.Add(increment);
            }

            public double Value => _value.Value;

            internal void ResetValue()
            {
                _value.Value = 0.0D;
            }
        }
    }
}