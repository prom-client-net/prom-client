using System;
using System.Collections.Concurrent;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors
{
    public abstract class Collector<TChild, TConfig> : ICollector
        where TChild : Labelled<TConfig>, new()
        where TConfig : MetricConfiguration
    {
        protected readonly TConfig Configuration;
        protected readonly ConcurrentDictionary<LabelValues, TChild> LabelledMetrics = new ConcurrentDictionary<LabelValues, TChild>();

        public string[] MetricNames => new[] { Configuration.Name };

        protected Collector(TConfig configuration)
        {
            Configuration = configuration;
            Unlabelled = CreateLabelled(LabelValues.Empty);
        }

        protected abstract MetricType Type { get; }

        internal string[] LabelNames => Configuration.LabelNames;

        protected internal TChild Unlabelled { get; }

        public void Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(Configuration.Name, Type, Configuration.Help);
            Unlabelled.Collect(writer);

            foreach (var labelledMetric in LabelledMetrics)
                labelledMetric.Value.Collect(writer);
        }

        /// <summary>
        ///     Analog WithLabels for compatible with old version
        /// </summary>
        public TChild Labels(params string[] labelValues)
        {
            return WithLabels(labelValues);
        }

        /// <summary>
        ///     Metric with Label Values
        /// </summary>
        public TChild WithLabels(params string[] labelValues)
        {
            if (labelValues == null || labelValues.Length == 0)
            {
                throw new ArgumentNullException(nameof(labelValues));
            }

            var key = new LabelValues(Configuration.LabelNames, labelValues);
            return LabelledMetrics.GetOrAdd(key, CreateLabelled);
        }

        private TChild CreateLabelled(LabelValues labels)
        {
            var child = new TChild();
            child.Init(labels, Configuration);
            return child;
        }
    }
}
