using System;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.Contracts;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client
{
    public abstract class Labelled
    {
        protected bool IncludeTimestamp;
        private LabelValues _labelValues;

        internal virtual void Init(ICollector parent, LabelValues labelValues, bool includeTimestamp)
        {
            _labelValues = labelValues;
            IncludeTimestamp = includeTimestamp;
        }

        protected internal abstract void Collect(IMetricsWriter writer);

        protected CLabelPair[] Labels => _labelValues.WireLabels;

        protected long? Timestamp { get; private set; }

        protected void SetTimestamp()
        {
            if (!IncludeTimestamp)
            {
                throw new InvalidOperationException("Set IncludeTimestamp to true before call SetTimestamp");
            }

            Timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
