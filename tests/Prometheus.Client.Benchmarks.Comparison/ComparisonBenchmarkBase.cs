extern alias Their;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Comparison
{
    public abstract class ComparisonBenchmarkBase
    {
        private MetricFactory _factory;
        private Their.Prometheus.MetricFactory _theirMetricFactory;

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

        protected MetricFactory OurMetricFactory => _factory;

        public Their::Prometheus.MetricFactory TheirMetricFactory => _theirMetricFactory;
    }
}
