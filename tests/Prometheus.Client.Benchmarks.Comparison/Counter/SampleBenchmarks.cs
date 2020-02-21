extern alias Their;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class SampleBenchmarks : ComparisonBenchmarkBase
    {
        private const int _opIterations = 1000000;

        private Abstractions.ICounter _counter;
        private Abstractions.ICounter<long> _counterInt64;
        private Their.Prometheus.ICounter _theirCounter;

        [IterationSetup]
        public void Setup()
        {
            _counter = OurMetricFactory.CreateCounter("testcounter", string.Empty);
            _counterInt64 = OurMetricFactory.CreateCounterInt64("testcounterInt64", string.Empty);

            _theirCounter = TheirMetricFactory.CreateCounter("testcounter", string.Empty);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_IncDefault")]
        public void Counter_IncDefaultBaseLine()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirCounter.Inc();
        }

        [Benchmark]
        [BenchmarkCategory("Counter_IncDefault")]
        public void Counter_IncDefault()
        {
            for (var i = 0; i < _opIterations; i++)
                _counter.Inc();
        }

        [Benchmark]
        [BenchmarkCategory("Counter_IncDefault")]
        public void CounterInt64_IncDefault()
        {
            for (var i = 0; i < _opIterations; i++)
                _counterInt64.Inc();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Inc")]
        public void Counter_IncBaseLine()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirCounter.Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Inc")]
        public void Counter_Inc()
        {
            for (var i = 0; i < _opIterations; i++)
                _counter.Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Inc")]
        public void CounterInt64_Inc()
        {
            for (var i = 0; i < _opIterations; i++)
                _counterInt64.Inc(i);
        }
    }
}
