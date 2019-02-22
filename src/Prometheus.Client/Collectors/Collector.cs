using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;

// ReSharper disable StaticMemberInGenericType

namespace Prometheus.Client.Collectors
{
    public abstract class Collector<TChild> : ICollector
        where TChild : Labelled, new()
    {
        private const string _metricNameLabelRe = "^[a-zA-Z_:][a-zA-Z0-9_:]*$";
        private readonly Lazy<TChild> _unlabelledLazy;

        private static readonly Regex _metricNameLabelRegex = new Regex(_metricNameLabelRe);
        private static readonly Regex _reservedLabelRegex = new Regex("^__.*$");

        protected readonly string Help;
        private readonly bool _includeTimestamp;
        protected readonly ConcurrentDictionary<LabelValues, TChild> LabelledMetrics = new ConcurrentDictionary<LabelValues, TChild>();
        
        protected abstract MetricType Type { get; }
        protected TChild Unlabelled => _unlabelledLazy.Value;

        public string Name { get; }
        public string[] LabelNames { get; }

        protected Collector(string name, string help, bool includeTimestamp, string[] labelNames)
        {
            Name = name;
            Help = help;
            _includeTimestamp = includeTimestamp;
            LabelNames = labelNames;

            if (!_metricNameLabelRegex.IsMatch(name))
                throw new ArgumentException("Metric name must match regex: " + _metricNameLabelRegex);

            foreach (var labelName in labelNames)
            {
                if (!_metricNameLabelRegex.IsMatch(labelName))
                    throw new ArgumentException("Label name must match regex: " + _metricNameLabelRegex);

                if (_reservedLabelRegex.IsMatch(labelName))
                    throw new ArgumentException("Labels starting with double underscore are reserved!");
            }

            _unlabelledLazy = new Lazy<TChild>(() => GetOrAddLabelled(LabelValues.Empty));
        }
        
        /// <summary>
        ///     Analog WithLabels for compatible with old version
        /// </summary>
        public TChild Labels(params string[] labelValues) => WithLabels(labelValues);

        /// <summary>
        ///     Metric with Label Values
        /// </summary>
        public TChild WithLabels(params string[] labelValues)
        {
            var key = new LabelValues(LabelNames, labelValues);
            return GetOrAddLabelled(key);
        }

        private TChild GetOrAddLabelled(LabelValues key)
        {
            return LabelledMetrics.GetOrAdd(key, labels1 =>
            {
                var child = new TChild();
                child.Init(this, labels1, _includeTimestamp);
                return child;
            });
        }

        public void Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(Name, Type, Help);

            foreach (var labelled in LabelledMetrics.Values)
            {
                labelled.Collect(writer);
            }
        }
    }
}
