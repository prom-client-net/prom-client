using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors
{
    public abstract class Collector<TChild, TConfig> : ICollector
        where TChild : Labelled<TConfig>, new()
        where TConfig : MetricConfiguration
    {
        private readonly IReadOnlyList<string> _metricNames;
        protected readonly TConfig Configuration;
        protected readonly ConcurrentDictionary<LabelValues, TChild> LabelledMetrics = new ConcurrentDictionary<LabelValues, TChild>();

        protected Collector(TConfig configuration)
        {
            Configuration = configuration;
            Unlabelled = CreateLabelled(LabelValues.Empty);
            _metricNames = new[] { Configuration.Name };
        }

        public IEnumerable<KeyValuePair<LabelValues, TChild>> Labelled => EnumerateLabelled();

        internal IReadOnlyList<string> LabelNames => Configuration.LabelNames;

        protected abstract MetricType Type { get; }

        protected internal TChild Unlabelled { get; }

        ICollectorConfiguration ICollector.Configuration => Configuration;

        IReadOnlyList<string> ICollector.MetricNames => _metricNames;

        void ICollector.Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(Configuration.Name, Type, Configuration.Help);
            if(!Configuration.SuppressEmptySamples || Unlabelled.HasObservations)
                Unlabelled.Collect(writer);

            foreach (var labelledMetric in LabelledMetrics)
            {
                if(!Configuration.SuppressEmptySamples || labelledMetric.Value.HasObservations)
                    labelledMetric.Value.Collect(writer);
            }

            writer.EndMetric();
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

        private IEnumerable<KeyValuePair<LabelValues, TChild>> EnumerateLabelled()
        {
            foreach (var labelled in LabelledMetrics)
            {
                yield return labelled;
            }
        }
    }
}
