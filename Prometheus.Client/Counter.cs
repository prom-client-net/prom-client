using System;
using Prometheus.Advanced.DataContracts;
using Prometheus.Client.Advanced;

namespace Prometheus.Client
{
    public interface ICounter
    {
        void Inc(double increment = 1);
        double Value { get; }
    }

    public class Counter : Collector<Counter.Child>, ICounter
    {

        internal Counter(string name, string help, string[] labelNames)
            : base(name, help, labelNames)
        {
        }

        public void Inc(double increment = 1)
        {
            Unlabelled.Inc(increment);
        }

        public class Child : Client.Advanced.Child, ICounter
        {
            private double _value;
            private readonly object _lock = new object();

            protected override void Populate(Metric metric)
            {
                metric.counter = new Prometheus.Advanced.DataContracts.Counter();
                metric.counter.value = Value;
            }

            public void Inc(double increment = 1)
            {
                if (increment < 0)
                {
                    throw new InvalidOperationException("Counter cannot go down");
                }

                lock (_lock)
                {
                    _value += increment;
                }
            }

            public double Value
            {
                get
                {
                    lock (_lock)
                    {
                        return _value;
                    }
                }
            }
        }

        public double Value => Unlabelled.Value;

        protected override MetricType Type => MetricType.COUNTER;
    }
}