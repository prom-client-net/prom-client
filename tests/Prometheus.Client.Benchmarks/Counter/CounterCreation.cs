using BenchmarkDotNet.Attributes;
using Prometheus.Client;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Counter
{
    [MemoryDiagnoser]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class CounterCreation
    {
        private IMetricFactory _factory;

        [GlobalSetup]
        public void Setup()
        {
            _factory = new MetricFactory(new CollectorRegistry());
        }

        [Benchmark]
        public ICounter Creation()
        {
            return _factory.CreateCounter("counter", string.Empty);
        }

        [Benchmark]
        public IMetricFamily<ICounter> CreationWithLabels()
        {
             return _factory.CreateCounter("counter", "help", "label1", "label2");
        }

        [Benchmark]
        public IMetricFamily<ICounter, (string, string)> CreationWithTupleLabels()
        {
            return _factory.CreateCounter("counter", "help", ("label1", "label2"));
        }
    }
}
