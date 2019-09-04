using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
#if HasITuple
using System.Runtime.CompilerServices;
#endif
using Prometheus.Client.Abstractions;
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
        private readonly TImplementation _unlabelled;
        private readonly ConcurrentDictionary<TLabels, TImplementation> _labelledMetrics;

        public MetricFamily(TConfig configuration, MetricType metricType, Func<TConfig, IReadOnlyList<string>, TImplementation> instanceFactory)
        {
            _metricType = metricType;
            _configuration = configuration;
            _metricNames = new[] { _configuration.Name };
            _instanceFactory = instanceFactory;
            _unlabelled = _instanceFactory(_configuration, default);
            LabelNames = TupleHelper<TLabels>.FromArray(configuration.LabelNames);
            if(configuration.LabelNames.Count > 0)
                _labelledMetrics = new ConcurrentDictionary<TLabels, TImplementation>();
        }

        public IEnumerable<KeyValuePair<TLabels, TMetric>> Labelled => EnumerateLabelled();

        ICollectorConfiguration ICollector.Configuration => _configuration;

        IReadOnlyList<string> ICollector.MetricNames => _metricNames;

        public TMetric Unlabelled => _unlabelled;

        public TLabels LabelNames { get; }

        TMetric IMetricFamily<TMetric>.Unlabelled => _unlabelled;

        IEnumerable<KeyValuePair<IReadOnlyList<string>, TMetric>> IMetricFamily<TMetric>.Labelled => EnumerateLabelledAsStrings();

        IReadOnlyList<string> IMetricFamily<TMetric>.LabelNames => _configuration.LabelNames;

        TMetric IMetricFamily<TMetric>.WithLabels(params string[] labels)
        {
            if (_labelledMetrics == null)
                throw new InvalidOperationException("Metric family does not have any labels");

            if (labels.Length != _configuration.LabelNames.Count)
                throw new ArgumentException("Wrong number of labels");

            var labelsTuple = TupleHelper<TLabels>.FromArray(labels);
            return _labelledMetrics.GetOrAdd(labelsTuple, CreateLabelled);
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

            return _labelledMetrics.GetOrAdd(labels, CreateLabelled);
        }

        void ICollector.Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(_configuration.Name, _metricType, _configuration.Help);
            if (!_configuration.SuppressEmptySamples || _unlabelled.HasObservations)
                _unlabelled.Collect(writer);

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
                yield return new KeyValuePair<TLabels, TMetric>(labelled.Key, labelled.Value);
        }

        private IEnumerable<KeyValuePair<IReadOnlyList<string>, TMetric>> EnumerateLabelledAsStrings()
        {
            if (_labelledMetrics == null)
                yield break;

            foreach (var labelled in _labelledMetrics)
                yield return new KeyValuePair<IReadOnlyList<string>, TMetric>(TupleHelper<TLabels>.ToArray(labelled.Key), labelled.Value);
        }

        private TImplementation CreateLabelled(TLabels labels)
        {
            var labelValues = TupleHelper<TLabels>.ToArray(labels);

            if (labelValues.Any(string.IsNullOrEmpty))
                throw new ArgumentException("Label cannot be empty.");

            return _instanceFactory(_configuration, labelValues);
        }
    }
}
