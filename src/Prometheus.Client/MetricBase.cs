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
        private bool _hasObservations;

        protected IReadOnlyList<KeyValuePair<string, string>> Labels { get; }

        protected MetricBase(TConfig config, IReadOnlyList<string> labelValues, Func<DateTimeOffset> currentTimeProvider = null)
        {
            if (currentTimeProvider == null)
                currentTimeProvider = () => DateTimeOffset.UtcNow;

            CurrentTimeProvider = currentTimeProvider;
            Configuration = config;
            _includeTimestamp = config.IncludeTimestamp;

            if (labelValues != null && labelValues.Count > 0)
                Labels = config.LabelNames.Zip(labelValues, (name, value) => new KeyValuePair<string, string>(name, value)).ToArray();

            LabelValues = labelValues;
        }

        protected internal bool HasObservations => Volatile.Read(ref _hasObservations);

        protected internal IReadOnlyList<string> LabelValues { get; }

        protected internal long? Timestamp
        {
            get
            {
                if (!_includeTimestamp)
                    return null;

                return Interlocked.Read(ref _timestamp);
            }
        }

        protected internal abstract void Collect(IMetricsWriter writer);

        protected Func<DateTimeOffset> CurrentTimeProvider { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void TimestampIfRequired(long? timestamp = null)
        {
            Volatile.Write(ref _hasObservations, true);

            if (!_includeTimestamp)
                return;

            var now = CurrentTimeProvider().ToUnixTimeMilliseconds();

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

            // use provided timestamp unless current timestamp is bigger
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
