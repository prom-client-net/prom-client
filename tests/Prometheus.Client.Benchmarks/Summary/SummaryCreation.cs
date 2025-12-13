using BenchmarkDotNet.Attributes;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Summary;

[MemoryDiagnoser]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class SummaryCreation
{
    private IMetricFactory _factory;

    [GlobalSetup]
    public void Setup()
    {
        _factory = new MetricFactory(new CollectorRegistry());
    }

    [Benchmark]
    public ISummary Creation()
    {
        return _factory.CreateSummary("summary1", string.Empty);
    }

    [Benchmark]
    public IMetricFamily<ISummary> CreationWithLabels()
    {
        return _factory.CreateSummary("summary2", "help", "label1", "label2");
    }
}
