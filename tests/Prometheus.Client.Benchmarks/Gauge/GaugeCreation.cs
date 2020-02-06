using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Gauge
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class GaugeCreation
    {
        private MetricFactory _factory;

        [GlobalSetup]
        public void Setup()
        {
            _factory = new MetricFactory(new CollectorRegistry());
        }

        [Benchmark]
        public IGauge Creation()
        {
            return _factory.CreateGauge("gauge", string.Empty);
        }

        [Benchmark]
        public IMetricFamily<IGauge> CreationWithLabels()
        {
             return _factory.CreateGauge("gauge", "help", "label1", "label2");
        }
    }
}
