extern alias Their;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class SampleBenchmarks
    {
        private const int _opIterations = 1000000;

        private Abstractions.ICounter _counter;
        private Abstractions.ICounter<long> _counterInt64;
        private Their.Prometheus.ICounter _theirCounter;

        [IterationSetup]
        public void Setup()
        {
            var factory = new MetricFactory(new CollectorRegistry());
            _counter = factory.CreateCounter("testcounter", string.Empty);
            _counterInt64 = factory.CreateCounterInt64("testcounterInt64", string.Empty);

            var registry = Their.Prometheus.Metrics.NewCustomRegistry();
            var theirFactory = Their.Prometheus.Metrics.WithCustomRegistry(registry);

            _theirCounter = theirFactory.CreateCounter("testcounter", string.Empty);
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = 10)]
        [BenchmarkCategory("Counter_IncDefault")]
        public void Counter_IncDefaultBaseLine()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirCounter.Inc();
        }

        [Benchmark(OperationsPerInvoke = 10)]
        [BenchmarkCategory("Counter_IncDefault")]
        public void Counter_IncDefault()
        {
            for (var i = 0; i < _opIterations; i++)
                _counter.Inc();
        }

        [Benchmark(OperationsPerInvoke = 10)]
        [BenchmarkCategory("Counter_IncDefault")]
        public void CounterInt64_IncDefault()
        {
            for (var i = 0; i < _opIterations; i++)
                _counterInt64.Inc();
        }
    }
}
