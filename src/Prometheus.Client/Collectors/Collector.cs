using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Prometheus.Client.Contracts;

// ReSharper disable StaticMemberInGenericType

namespace Prometheus.Client.Collectors
{
    public abstract class Collector<T> : ICollector where T : Child, new()
    {
        private const string _metricNameLabelRe = "^[a-zA-Z_:][a-zA-Z0-9_:]*$";
        
        private readonly string _help;
        private readonly Lazy<T> _unlabelledLazy;
        
        private static readonly Regex _metricNameLabelRegex = new Regex(_metricNameLabelRe);
        private static readonly Regex _reservedLabelRegex = new Regex("^__.*$");
        private static readonly LabelValues _emptyLabelValues = new LabelValues(new string[0], new string[0]);

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

            if (!_metricNameLabelRegex.IsMatch(name))
                throw new ArgumentException("Metric name must match regex: " + _metricNameLabelRegex);
            
            foreach (var labelName in labelNames)
            {
                if (!_metricNameLabelRegex.IsMatch(labelName))
                    throw new ArgumentException("Label name must match regex: " + _metricNameLabelRegex);
                
                if (_reservedLabelRegex.IsMatch(labelName))
                    throw new ArgumentException("Labels starting with double underscore are reserved!"); 
            }

            _unlabelledLazy = new Lazy<T>(() => GetOrAddLabelled(_emptyLabelValues));
        }

        public MetricFamily Collect()
        {
            var result = new MetricFamily
            {
                Name = Name,
                Help = _help,
                Type = Type
            };

            foreach (var child in LabelledMetrics.Values)
            {
                result.Metrics.Add(child.Collect());
            }

            return result;
        }
    }
}