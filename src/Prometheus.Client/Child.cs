using Prometheus.Client.Collectors;
using Prometheus.Client.Contracts;

namespace Prometheus.Client
{
    public abstract class Child
    {
        private LabelValues _labelValues;

        internal virtual void Init(ICollector parent, LabelValues labelValues)
        {
            _labelValues = labelValues;
        }

        protected abstract void Populate(CMetric cMetric);

        internal CMetric Collect()
        {
            var metric = new CMetric();
            Populate(metric);
            metric.Labels = _labelValues.WireLabels;
            return metric;
        }
    }
}