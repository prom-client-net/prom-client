using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Prometheus.Client.Internal;
using Prometheus.Contracts;

namespace Prometheus.Client.Collectors
{
    public abstract class Collector<T> : ICollector where T : Child, new()
    {
        private const string _metricNameRe = "^[a-zA-Z_:][a-zA-Z0-9_:]*$";
        private readonly string _help;
       
        private readonly Lazy<T> _unlabelledLazy;
        private readonly Regex _metricName = new Regex(_metricNameRe);
        private readonly Regex _labelNameRegex = new Regex("^[a-zA-Z_:][a-zA-Z0-9_:]*$");
        private readonly Regex _reservedLabelRegex = new Regex("^__.*$");
        private readonly LabelValues _emptyLabelValues = new LabelValues(new string[0], new string[0]);

        protected readonly ConcurrentDictionary<LabelValues, T> LabelledMetrics = new ConcurrentDictionary<LabelValues, T>();
        protected abstract MetricType Type { get; }
        protected T Unlabelled => _unlabelledLazy.Value;

        public string Name { get; }
        public string[] LabelNames { get; }
        
        public T Labels(params string[] labelValues)
        {
            var key = new LabelValues(LabelNames, labelValues);
            return GetOrAddLabelled(key);
        }

        private T GetOrAddLabelled(LabelValues key)
        {
            return LabelledMetrics.GetOrAdd(key, labels1 =>
            {
                var child = new T();
                child.Init(this, labels1);
                return child;
            });
        }

        protected Collector(string name, string help, string[] labelNames)
        {
            Name = name;
            _help = help;
            LabelNames = labelNames;

            if (!_metricName.IsMatch(name))
            {
                throw new ArgumentException("Metric name must match regex: " + _metricNameRe);
            }

            foreach (var labelName in labelNames)
            {
                if (!_labelNameRegex.IsMatch(labelName))
                {
                    throw new ArgumentException("Invalid label name!");
                }
                if (_reservedLabelRegex.IsMatch(labelName))
                {
                    throw new ArgumentException("Labels starting with double underscore are reserved!");
                }
            }

            _unlabelledLazy = new Lazy<T>(() => GetOrAddLabelled(_emptyLabelValues));
        }

        public MetricFamily Collect()
        {
            var result = new MetricFamily
            {
                name = Name,
                help = _help,
                type = Type,
            };

            foreach (var child in LabelledMetrics.Values)
            {
                result.metric.Add(child.Collect());
            }

            return result;
        }
    }
}