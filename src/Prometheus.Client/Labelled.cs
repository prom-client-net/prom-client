using System;
using Prometheus.Client.Collectors;
using Prometheus.Client.Contracts;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client
{
    public abstract class Labelled<TConfig>
        where TConfig: MetricConfiguration
    {
        private LabelValues _labelValues;
        protected TConfig Configuration;

        protected CLabelPair[] Labels => _labelValues.WireLabels;

        protected long? Timestamp { get; private set; }

        protected internal virtual void Init(LabelValues labelValues, TConfig configuration)
        {
            _labelValues = labelValues;
            Configuration = configuration;
        }

        protected internal abstract void Collect(IMetricsWriter writer);

        protected void TimestampIfRequired()
        {
            if (Configuration.IncludeTimestamp)
            {
                // TODO: Should it be number of milliseconds?
                // https://prometheus.io/docs/instrumenting/exposition_formats/
                Timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            }
        }
    }
}
