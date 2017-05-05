using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Prometheus.Advanced;
using Prometheus.Advanced.DataContracts;
using Prometheus.Client.Internal;

namespace Prometheus.Client.Advanced
{
    public abstract class Collector<T> : ICollector where T : Child, new()
    {
        private const string MetricNameRe = "^[a-zA-Z_:][a-zA-Z0-9_:]*$";

        private readonly ConcurrentDictionary<LabelValues, T> _labelledMetrics = new ConcurrentDictionary<LabelValues, T>();
        private readonly string _help;
        private readonly Lazy<T> _unlabelledLazy;

        private static readonly Regex MetricName = new Regex(MetricNameRe);
        private static readonly Regex LabelNameRegex = new Regex("^[a-zA-Z_:][a-zA-Z0-9_:]*$");
        private static readonly Regex ReservedLabelRegex = new Regex("^__.*$");
        private static readonly LabelValues EmptyLabelValues = new LabelValues(new string[0], new string[0]);

        protected abstract MetricType Type { get; }

        public string Name { get; }

        public string[] LabelNames { get; }

        protected T Unlabelled => _unlabelledLazy.Value;

        public T Labels(params string[] labelValues)
        {
            var key = new LabelValues(LabelNames, labelValues);
            return GetOrAddLabelled(key);
        }

        private T GetOrAddLabelled(LabelValues key)
        {
            return _labelledMetrics.GetOrAdd(key, labels1 =>
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

            if (!MetricName.IsMatch(name))
            {
                throw new ArgumentException("Metric name must match regex: " + MetricNameRe);
            }

            foreach (var labelName in labelNames)
            {
                if (!LabelNameRegex.IsMatch(labelName))
                {
                    throw new ArgumentException("Invalid label name!");
                }
                if (ReservedLabelRegex.IsMatch(labelName))
                {
                    throw new ArgumentException("Labels starting with double underscore are reserved!");
                }
            }

            _unlabelledLazy = new Lazy<T>(() => GetOrAddLabelled(EmptyLabelValues));
        }

        public MetricFamily Collect()
        {
            var result = new MetricFamily()
            {
                name = Name,
                help = _help,
                type = Type,
            };

            foreach (var child in _labelledMetrics.Values)
            {
                result.metric.Add(child.Collect());
            }

            return result;
        }
    }
}