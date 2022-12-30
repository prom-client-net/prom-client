extern alias Their;
using System;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Summary;

public class SummaryCreationBenchmarks : ComparisonBenchmarkBase
{
    private const int _metricsPerIteration = 10_000;

    private readonly string[] _metricNames;
    private readonly string[] _labelNames = { "foo", "bar", "baz" };

    public SummaryCreationBenchmarks()
    {
        _metricNames = GenerateMetricNames(_metricsPerIteration);
    }

    [IterationSetup]
    public void Setup()
    {
        ResetFactories();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Single")]
    public void Single_Baseline()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            TheirMetricFactory.CreateSummary("testsummary", HelpText);
    }

    [Benchmark]
    [BenchmarkCategory("Single")]
    public void Single()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            OurMetricFactory.CreateSummary("testsummary", HelpText, ValueTuple.Create());
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Single_WithLabels")]
    public void SingleLabels_Baseline()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            TheirMetricFactory.CreateSummary("testsummary", HelpText, "foo", "bar", "baz");
    }

    [Benchmark]
    [BenchmarkCategory("Single_WithLabels")]
    public void SingleLabels_Array()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            OurMetricFactory.CreateSummary("testsummary", HelpText, "foo", "bar", "baz");
    }

    [Benchmark]
    [BenchmarkCategory("Single_WithLabels")]
    public void SingleLabels_Tuple()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            OurMetricFactory.CreateSummary("testsummary", HelpText, ("foo", "bar", "baz"));
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Single_WithSharedLabels")]
    public void SingleWithSharedLabels_Baseline()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            TheirMetricFactory.CreateSummary("testsummary", HelpText, _labelNames);
    }

    [Benchmark]
    [BenchmarkCategory("Single_WithSharedLabels")]
    public void SingleWithSharedLabels()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            OurMetricFactory.CreateSummary("testsummary", HelpText, _labelNames);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Many")]
    public void Many_Baseline()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            TheirMetricFactory.CreateSummary(_metricNames[i], HelpText);
    }

    [Benchmark]
    [BenchmarkCategory("Many")]
    public void Many()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            OurMetricFactory.CreateSummary(_metricNames[i], HelpText, ValueTuple.Create());
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Many_WithLabels")]
    public void ManyWithLabels_Baseline()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            TheirMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
    }

    [Benchmark]
    [BenchmarkCategory("Many_WithLabels")]
    public void ManyWithLabels_Array()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            OurMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
    }

    [Benchmark]
    [BenchmarkCategory("Many_WithLabels")]
    public void ManyWithLabels_Tuple()
    {
        for (var i = 0; i < _metricsPerIteration; i++)
            OurMetricFactory.CreateSummary(_metricNames[i], HelpText, ("foo", "bar", "baz"));
    }
}
