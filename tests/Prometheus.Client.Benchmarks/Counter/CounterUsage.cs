using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Counter
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class CounterUsage
    {
        private Client.Counter _counter;

        [GlobalSetup]
        public void Setup()
        {
            var factory = new MetricFactory(new CollectorRegistry());
            _counter = factory.CreateCounter("counter", string.Empty, "label");
        }

        [Benchmark]
        public ICounter LabelledCreation()
        {
            return _counter.WithLabels("test label");
        }

        [Benchmark]
        public void Inc()
        {
            _counter.Inc();
        }
    }
}
