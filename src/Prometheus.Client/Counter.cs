using System;
using Prometheus.Advanced.DataContracts;
using Prometheus.Client.Advanced;

namespace Prometheus.Client
{
    public interface ICounter
    {
        void Inc(double increment = 1.0D);
        double Value { get; }
    }

    public class Counter : Collector<Counter.Child>, ICounter
    {
        internal Counter(string name, string help, string[] labelNames)
            : base(name, help, labelNames)
        {
        }

        public void Inc(double increment = 1.0D)
        {
            Unlabelled.Inc(increment);
        }

        public class Child : Advanced.Child, ICounter
        {
            private ThreadSafeDouble _value;

            protected override void Populate(Metric metric)
            {
                metric.counter = new Prometheus.Advanced.DataContracts.Counter {value = Value};
            }

            public void Inc(double increment = 1.0D)
            {
                if (increment < 0.0D)
                    throw new InvalidOperationException("Counter cannot go down");

                _value.Add(increment);
            }

            public double Value => _value.Value;
        }

        public double Value => Unlabelled.Value;

        protected override MetricType Type => MetricType.COUNTER;
    }
}