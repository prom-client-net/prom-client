using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client
{
    public abstract class MetricBase<TConfig>
        where TConfig: MetricConfiguration
    {
        protected readonly TConfig Configuration;
        private readonly bool _includeTimestamp;
        private long _timestamp;
        private bool _hasObservation;

        protected IReadOnlyList<KeyValuePair<string, string>> Labels { get; }

        protected MetricBase(TConfig config, IReadOnlyList<string> labelValues)
        {
            Configuration = config;
            _includeTimestamp = config.IncludeTimestamp;

            if (labelValues != null && labelValues.Count > 0)
            {
                if (config.LabelNames.Count != labelValues.Count)
                    throw new ArgumentException("Incorrect number of labels");

                Labels = config.LabelNames.Zip(labelValues, (name, value) => new KeyValuePair<string, string>(name, value)).ToArray();
            }
        }

        public bool HasObservations => Volatile.Read(ref _hasObservation);

        protected long? Timestamp
        {
            get
            {
                if (!_includeTimestamp)
                    return null;

                return Interlocked.Read(ref _timestamp);
            }
        }

        protected internal abstract void Collect(IMetricsWriter writer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void TimestampIfRequired(long? timestamp = null)
        {
            Volatile.Write(ref _hasObservation, true);

            if (!_includeTimestamp)
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
