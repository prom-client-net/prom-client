using Prometheus.Client.Collectors;
using Prometheus.Client.Internal;
using Prometheus.Contracts;

namespace Prometheus.Client
{
    public abstract class Child
    {
        private LabelValues _labelValues;

        internal virtual void Init(ICollector parent, LabelValues labelValues)
        {
            _labelValues = labelValues;
        }

        protected abstract void Populate(Metric metric);

        internal Metric Collect()
        {
            var metric = new Metric();
            Populate(metric);
            metric.label = _labelValues.WireLabels;
            return metric;
        }
    }
}