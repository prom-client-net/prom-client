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
        private IMetricFamily<ICounter> _counter;

        [GlobalSetup]
        public void Setup()
        {
            var factory = new MetricFactory(new CollectorRegistry());
            _counter = factory.CreateCounter("counter", string.Empty, "label1", "label2");
        }

        [Benchmark]
        public ICounter LabelledCreation()
        {
            return _counter.WithLabels("test label1", "test label2");
        }

        [Benchmark]
        public void Inc()
        {
            _counter.Inc();
        }
    }
}
