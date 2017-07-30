using System;
using Prometheus.Client.Collectors;
using Prometheus.Client.Internal;
using Prometheus.Contracts;

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
        void Inc(double increment = 1.0D);
        double Value { get; }
    }

    /// <summary>
    ///     Counter metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#counter
    ///     </remarks>
    /// </summary>
    public class Counter : Collector<Counter.ThisChild>, ICounter
    {
        internal Counter(string name, string help, string[] labelNames)
            : base(name, help, labelNames)
        {
        }

        public void Inc(double increment = 1.0D)
        {
            Unlabelled.Inc(increment);
        }

        public class ThisChild : Child, ICounter
        {
            private ThreadSafeDouble _value;

            protected override void Populate(Metric metric)
            {
                metric.counter = new Contracts.Counter { value = Value };
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