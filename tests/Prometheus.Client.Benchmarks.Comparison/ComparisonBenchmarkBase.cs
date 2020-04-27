extern alias Their;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Comparison
{
    public abstract class ComparisonBenchmarkBase
    {
        private IMetricFactory _factory;
        private Their.Prometheus.MetricFactory _theirMetricFactory;

        protected const string HelpText = "arbitrary help message for metric, not relevant for benchmarking";

        protected ComparisonBenchmarkBase()
        {
            ResetFactories();
        }

        protected void ResetFactories()
        {
            _factory = new MetricFactory(new CollectorRegistry());

            var registry = Their.Prometheus.Metrics.NewCustomRegistry();
            _theirMetricFactory = Their.Prometheus.Metrics.WithCustomRegistry(registry);
        }

        protected IMetricFactory OurMetricFactory => _factory;

        public Their::Prometheus.MetricFactory TheirMetricFactory => _theirMetricFactory;
    }
}
