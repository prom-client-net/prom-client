using Prometheus.Client.Collectors;
using Prometheus.Client.Contracts;
using Prometheus.Client.Internal;

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
        void Inc();
        void Inc(double increment);
        void Set(double val);
        void Dec();
        void Dec(double decrement);
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


        public class ThisChild : Child, IGauge
        {
            private ThreadSafeDouble _value;

            public void Inc()
            {
                Inc(1.0D);
            }

            public void Inc(double increment)
            {
                _value.Add(increment);
            }

            public void Set(double val)
            {
                _value.Value = val;
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


            protected override void Populate(Metric metric)
            {
                metric.Gauge = new Contracts.Gauge { Value = Value };
            }
        }
    }
}