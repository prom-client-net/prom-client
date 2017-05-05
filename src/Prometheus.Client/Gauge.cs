using System.Threading;
using Prometheus.Advanced.DataContracts;
using Prometheus.Client.Advanced;

namespace Prometheus.Client
{
    public interface IGauge
    {
        double Value { get; }
        void Inc(double increment = 1.0D);
        void Set(double val);
        void Dec(double decrement = 1.0D);
    }

    public class Gauge : Collector<Gauge.Child>, IGauge
    {
        internal Gauge(string name, string help, string[] labelNames)
            : base(name, help, labelNames)
        {
        }

        protected override MetricType Type => MetricType.GAUGE;

        public void Inc(double increment = 1.0D)
        {
            Unlabelled.Inc(increment);
        }

        public void Set(double val)
        {
            Unlabelled.Set(val);
        }


        public void Dec(double decrement = 1.0D)
        {
            Unlabelled.Dec(decrement);
        }

        public double Value => Unlabelled.Value;


        public class Child : Advanced.Child, IGauge
        {
            private ThreadSafeDouble _value;

            public void Inc(double increment = 1.0D)
            {
                _value.Add(increment);
            }

            public void Set(double val)
            {
                _value.Value = val;
            }


            public void Dec(double decrement = 1.0D)
            {
                Inc(-decrement);
            }

            public double Value => _value.Value;


            protected override void Populate(Metric metric)
            {
                metric.gauge = new Prometheus.Advanced.DataContracts.Gauge { value = Value };
            }
        }
    }
}