using System;
using System.Collections.Generic;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tools;

namespace Prometheus.Client
{
    public abstract class Labelled<TConfig>
        where TConfig: MetricConfiguration
    {
        private LabelValues _labelValues;
        protected TConfig Configuration;

        protected KeyValuePair<string, string>[] Labels => _labelValues.WireLabels;

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
                Timestamp = DateTime.UtcNow.ToUnixTime();
            }
        }
    }
}
