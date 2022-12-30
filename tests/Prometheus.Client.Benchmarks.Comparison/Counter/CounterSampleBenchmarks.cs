extern alias Their;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Counter;

public class CounterSampleBenchmarks : ComparisonBenchmarkBase
{
    private const int _opIterations = 10_000_000;

    private ICounter _counter;
    private ICounter<long> _counterInt64;
    private Their.Prometheus.ICounter _theirCounter;

    [IterationSetup]
    public void Setup()
    {
        _counter = OurMetricFactory.CreateCounter("testcounter", string.Empty);
        _counterInt64 = OurMetricFactory.CreateCounterInt64("testcounterInt64", string.Empty);

        _theirCounter = TheirMetricFactory.CreateCounter("testcounter", string.Empty);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("IncDefault")]
    public void IncDefault_Baseline()
    {
        for (var i = 0; i < _opIterations; i++)
            _theirCounter.Inc();
    }

    [Benchmark]
    [BenchmarkCategory("IncDefault")]
    public void IncDefault()
    {
        for (var i = 0; i < _opIterations; i++)
            _counter.Inc();
    }

    [Benchmark]
    [BenchmarkCategory("IncDefault")]
    public void IncDefault_Int64()
    {
        for (var i = 0; i < _opIterations; i++)
            _counterInt64.Inc();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Inc")]
    public void Inc_Baseline()
    {
        for (var i = 0; i < _opIterations; i++)
            _theirCounter.Inc(i);
    }

    [Benchmark]
    [BenchmarkCategory("Inc")]
    public void Inc()
    {
        for (var i = 0; i < _opIterations; i++)
            _counter.Inc(i);
    }

    [Benchmark]
    [BenchmarkCategory("Inc")]
    public void Inc_Int64()
    {
        for (var i = 0; i < _opIterations; i++)
            _counterInt64.Inc(i);
    }
}
