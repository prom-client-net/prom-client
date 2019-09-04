using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Gauge
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class GaugeUsage
    {
        private IMetricFamily<IGauge> _gauge;

        [GlobalSetup]
        public void Setup()
        {
            var factory = new MetricFactory(new CollectorRegistry());
            _gauge = factory.CreateGauge("gauge", string.Empty, "label1", "label2");
        }

        [Benchmark]
        public IGauge LabelledCreation()
        {
            return _gauge.WithLabels("test label");
        }

        [Benchmark]
        public void Inc()
        {
            _gauge.Inc();
        }
    }
}
