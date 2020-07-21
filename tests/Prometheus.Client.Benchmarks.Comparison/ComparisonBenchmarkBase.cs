extern alias Their;
using System;
using System.Collections.Generic;
using System.Linq;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Benchmarks.Comparison
{
    public abstract class ComparisonBenchmarkBase
    {
        private IMetricFactory _factory;
        private ICollectorRegistry _registry;
        private Their.Prometheus.CollectorRegistry _theirRegistry;
        private Their.Prometheus.MetricFactory _theirMetricFactory;

        protected const string HelpText = "arbitrary help message for metric, not relevant for benchmarking";

        protected ComparisonBenchmarkBase()
        {
            ResetFactories();
        }

        protected void ResetFactories()
        {
            _registry = new CollectorRegistry();
            _factory = new MetricFactory(_registry);

            _theirRegistry = Their.Prometheus.Metrics.NewCustomRegistry();
            _theirMetricFactory = Their.Prometheus.Metrics.WithCustomRegistry(_theirRegistry);

            GC.Collect();
        }

        public IEnumerable<string> GenerateMetrics(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return $"metric_{i}";
            }
        }

        public IEnumerable<string> GenerateLabelNames(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return $"label_{i}";
            }
        }

        public IEnumerable<string[]> GenerateLabels(int count, int labels)
        {
            var labelsRange = Enumerable.Range(1, labels).ToArray();

            for (var i = 0; i < count; i++)
            {
                yield return labelsRange.Select(l => $"label{i}_variant{l}").ToArray();
            }
        }

        protected IMetricFactory OurMetricFactory => _factory;

        protected ICollectorRegistry OurCollectorRegistry => _registry;

        public Their::Prometheus.MetricFactory TheirMetricFactory => _theirMetricFactory;

        protected Their::Prometheus.CollectorRegistry TheirCollectorRegistry => _theirRegistry;
    }
}
