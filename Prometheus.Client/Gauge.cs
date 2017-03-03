using System.Threading;
using Prometheus.Advanced.DataContracts;
using Prometheus.Client.Advanced;

namespace Prometheus.Client
{
    public interface IGauge
    {
        double Value { get; }
        void Inc(double increment = 1);
        void Set(double val);
        void Dec(double decrement = 1);
    }

    public class Gauge : Collector<Gauge.Child>, IGauge
    {
        internal Gauge(string name, string help, string[] labelNames)
            : base(name, help, labelNames)
        {
        }

        protected override MetricType Type => MetricType.GAUGE;

        public void Inc(double increment = 1)
        {
            Unlabelled.Inc(increment);
        }

        public void Set(double val)
        {
            Unlabelled.Set(val);
        }


        public void Dec(double decrement = 1)
        {
            Unlabelled.Dec(decrement);
        }

        public double Value => Unlabelled.Value;


        public class Child : Advanced.Child, IGauge
        {
            private readonly object _lock = new object();
            private double _value;

            public void Inc(double increment = 1)
            {
                lock (_lock)
                {
                    _value += increment;
                }
            }

            public void Set(double val)
            {
                Interlocked.Exchange(ref _value, val);
            }


            public void Dec(double decrement = 1)
            {
                Inc(-decrement);
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

            protected override void Populate(Metric metric)
            {
                metric.gauge = new Prometheus.Advanced.DataContracts.Gauge();
                lock (_lock)
                {
                    metric.gauge.value = _value;
                }
            }
        }
    }
}