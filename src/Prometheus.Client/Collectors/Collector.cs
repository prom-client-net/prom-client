using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.Contracts;

// ReSharper disable StaticMemberInGenericType

namespace Prometheus.Client.Collectors
{
    public abstract class Collector<TChild> : ICollector where TChild : Child, new()
    {
        private const string _metricNameLabelRe = "^[a-zA-Z_:][a-zA-Z0-9_:]*$";

        private readonly string _help;
        private readonly Lazy<TChild> _unlabelledLazy;

        private static readonly Regex _metricNameLabelRegex = new Regex(_metricNameLabelRe);
        private static readonly Regex _reservedLabelRegex = new Regex("^__.*$");
        private static readonly LabelValues _emptyLabelValues = new LabelValues(new string[0], new string[0]);

        protected readonly ConcurrentDictionary<LabelValues, TChild> LabelledMetrics = new ConcurrentDictionary<LabelValues, TChild>();
        protected abstract CMetricType Type { get; }
        protected TChild Unlabelled => _unlabelledLazy.Value;

        public string Name { get; }
        public string[] LabelNames { get; }

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
                child.Init(this, labels1);
                return child;
            });
        }

        protected Collector(string name, string help, string[] labelNames)
        {
            Name = name;
            _help = help;
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

            _unlabelledLazy = new Lazy<TChild>(() => GetOrAddLabelled(_emptyLabelValues));
        }

        public CMetricFamily Collect()
        {
            var result = new CMetricFamily
            {
                Name = Name,
                Help = _help,
                Type = Type,
                Metrics = new CMetric[LabelledMetrics.Values.Count]
            };

            var i = 0;
            foreach (var child in LabelledMetrics.Values)
            {
                result.Metrics[i] = child.Collect();
                i++;
            }

            return result;
        }
    }
}
