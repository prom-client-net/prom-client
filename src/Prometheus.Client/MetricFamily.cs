using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
#if HasITuple
using System.Runtime.CompilerServices;
#endif
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client
{
    public sealed class MetricFamily<TMetric, TImplementation, TLabels, TConfig> : IMetricFamily<TMetric, TLabels>, IMetricFamily<TMetric>, ICollector
        where TMetric : IMetric
        where TImplementation : MetricBase<TConfig>, TMetric
        where TConfig : MetricConfiguration
#if HasITuple
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        private readonly MetricType _metricType;
        private readonly TConfig _configuration;
        private readonly IReadOnlyList<string> _metricNames;
        private readonly Func<TConfig, IReadOnlyList<string>, TImplementation> _instanceFactory;
        private readonly Lazy<TImplementation> _unlabelled;
        private readonly ConcurrentDictionary<int, TImplementation> _labelledMetrics;

        public MetricFamily(TConfig configuration, MetricType metricType, Func<TConfig, IReadOnlyList<string>, TImplementation> instanceFactory)
        {
            _metricType = metricType;
            _configuration = configuration;
            _metricNames = new[] { _configuration.Name };
            _instanceFactory = instanceFactory;
            _unlabelled = new Lazy<TImplementation>(() => _instanceFactory(_configuration, default));
            LabelNames = LabelsHelper.FromArray<TLabels>(configuration.LabelNames);
            if(configuration.LabelNames.Count > 0)
                _labelledMetrics = new ConcurrentDictionary<int, TImplementation>();
        }

        public IEnumerable<KeyValuePair<TLabels, TMetric>> Labelled => EnumerateLabelled();

        CollectorConfiguration ICollector.Configuration => _configuration;

        IReadOnlyList<string> ICollector.MetricNames => _metricNames;

        public TMetric Unlabelled => _unlabelled.Value;

        public TLabels LabelNames { get; }

        TMetric IMetricFamily<TMetric>.Unlabelled => _unlabelled.Value;

        IEnumerable<KeyValuePair<IReadOnlyList<string>, TMetric>> IMetricFamily<TMetric>.Labelled => EnumerateLabelledAsStrings();

        IReadOnlyList<string> IMetricFamily<TMetric>.LabelNames => _configuration.LabelNames;

        TMetric IMetricFamily<TMetric>.WithLabels(params string[] labels)
        {
            if (_labelledMetrics == null)
                throw new InvalidOperationException("Metric family does not have any labels");

            if (labels.Length != _configuration.LabelNames.Count)
                throw new ArgumentException("Wrong number of labels");

            var key = LabelsHelper.GetHashCode(labels);

            if (_labelledMetrics.TryGetValue(key, out var metric))
            {
                return metric;
            }

            metric = CreateLabelled(labels);
            return _labelledMetrics.GetOrAdd(key, metric);
        }

        [Obsolete("This method is obsolete. Use WithLabels instead.")]
        TMetric IMetricFamily<TMetric>.Labels(params string[] labels)
        {
            return ((IMetricFamily<TMetric>)this).WithLabels(labels);
        }

        public TMetric WithLabels(TLabels labels)
        {
            if (_labelledMetrics == null)
                throw new InvalidOperationException("Metric family does not have any labels");

            var key = LabelsHelper.GetHashCode(labels);

            if (_labelledMetrics.TryGetValue(key, out var metric))
            {
                return metric;
            }

            metric = CreateLabelled(LabelsHelper.ToArray(labels));
            return _labelledMetrics.GetOrAdd(key, metric);
        }

        void ICollector.Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(_configuration.Name, _metricType, _configuration.Help);
            if (_unlabelled.IsValueCreated)
                _unlabelled.Value.Collect(writer);

            if (_labelledMetrics != null)
            {
                foreach (var labelledMetric in _labelledMetrics)
                    labelledMetric.Value.Collect(writer);
            }

            writer.EndMetric();
        }

        private IEnumerable<KeyValuePair<TLabels, TMetric>> EnumerateLabelled()
        {
            if (_labelledMetrics == null)
                yield break;

            foreach (var labelled in _labelledMetrics)
                yield return new KeyValuePair<TLabels, TMetric>(LabelsHelper.FromArray<TLabels>(labelled.Value.LabelValues), labelled.Value);
        }

        private IEnumerable<KeyValuePair<IReadOnlyList<string>, TMetric>> EnumerateLabelledAsStrings()
        {
            if (_labelledMetrics == null)
                yield break;

            foreach (var labelled in _labelledMetrics)
                yield return new KeyValuePair<IReadOnlyList<string>, TMetric>(labelled.Value.LabelValues, labelled.Value);
        }

        private TImplementation CreateLabelled(IReadOnlyList<string> labels)
        {
            for (var i = 0; i < labels.Count; i++)
            {
                if(string.IsNullOrEmpty(labels[i]))
                    throw new ArgumentException("Label cannot be empty.");
            }

            return _instanceFactory(_configuration, labels);
        }
    }
}
