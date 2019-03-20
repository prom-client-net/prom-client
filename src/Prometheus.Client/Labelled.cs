using System;
using System.Collections.Generic;
using System.Threading;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tools;

namespace Prometheus.Client
{
    public abstract class Labelled<TConfig>
        where TConfig: MetricConfiguration
    {
        private LabelValues _labelValues;
        private long _timestamp;
        protected TConfig Configuration;

        protected IReadOnlyList<KeyValuePair<string, string>> Labels => _labelValues.Labels;

        protected long? Timestamp
        {
            get
            {
                if (!Configuration.IncludeTimestamp)
                    return null;

                return Interlocked.Read(ref _timestamp);
            }
        }

        protected internal virtual void Init(LabelValues labelValues, TConfig configuration)
        {
            _labelValues = labelValues;
            Configuration = configuration;
        }

        protected internal abstract void Collect(IMetricsWriter writer);

        protected void TimestampIfRequired(long? timestamp = null)
        {
            if (!Configuration.IncludeTimestamp)
                return;

            var now = DateTime.UtcNow.ToUnixTime();

            if (!timestamp.HasValue)
            {
                // no needs to check anything, null means now.
                Interlocked.Exchange(ref _timestamp, now);
                return;
            }

            // use now if provided timestamp is in future
            if (timestamp > now)
            {
                Interlocked.Exchange(ref _timestamp, now);
                return;
            }

            // use provided timestamp unless current timestamp bigger
            while (true)
            {
                long current = Interlocked.Read(ref _timestamp);
                if (current > timestamp)
                    return;

                if (current == Interlocked.CompareExchange(ref _timestamp, timestamp.Value, current))
                    return;
            }
        }
    }
}
