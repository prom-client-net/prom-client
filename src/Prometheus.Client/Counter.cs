using System;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.Contracts;
using Prometheus.Client.Tools;

namespace Prometheus.Client
{
    /// <inheritdoc cref="ICounter" />
    public class Counter : Collector<Counter.ThisChild>, ICounter
    {
        internal Counter(string name, string help, string[] labelNames)
            : this(name, help, false, labelNames)
        {
        }
        
        internal Counter(string name, string help, bool includeTimestamp, string[] labelNames)
            : base(name, help, includeTimestamp, labelNames)
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
                labelledMetric.Value.ResetValue();          
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
