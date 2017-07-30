using Prometheus.Client.Collectors;
using Prometheus.Client.Internal;
using Prometheus.Contracts;

namespace Prometheus.Client
{
    /// <summary>
    ///     Gauge metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#gauge
    ///     </remarks>
    /// </summary>
    public interface IGauge
    {
        double Value { get; }
        void Inc(double increment = 1.0D);
        void Set(double val);
        void Dec(double decrement = 1.0D);
    }

    /// <summary>
    ///     Gauge metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#gauge
    ///     </remarks>
    /// </summary>
    public class Gauge : Collector<Gauge.ThisChild>, IGauge
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


        public class ThisChild : Child, IGauge
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
                metric.gauge = new Contracts.Gauge { value = Value };
            }
        }
    }
}