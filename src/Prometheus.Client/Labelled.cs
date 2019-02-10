using System;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.Contracts;

namespace Prometheus.Client
{
    public abstract class Labelled
    {
        private long? _timestamp;
        protected bool IncludeTimestamp;
        private LabelValues _labelValues;

        internal virtual void Init(ICollector parent, LabelValues labelValues, bool includeTimestamp)
        {
            _labelValues = labelValues;
            IncludeTimestamp = includeTimestamp;
        }

        protected abstract void Populate(CMetric cMetric);

        internal CMetric Collect()
        {
            var metric = new CMetric();
            Populate(metric);
            metric.Labels = _labelValues.WireLabels;
            if (IncludeTimestamp)
                metric.Timestamp = _timestamp;
            return metric;
        }

        protected void SetTimestamp()
        {
            _timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
