using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Counter
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class CounterCreation
    {
        private MetricFactory _factory;

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
        public ICounter CreationWithLabels()
        {
             return _factory.CreateCounter("counter", "help", "label1", "label2");
        }
    }
}
