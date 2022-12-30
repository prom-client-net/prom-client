using System.IO;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Histogram;

[MemoryDiagnoser]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class HistogramCollection
{
    private CollectorRegistry _registry;
    private MemoryStream _stream;

    [GlobalSetup]
    public void Setup()
    {
        _registry = new CollectorRegistry();
        var factory = new MetricFactory(_registry);
        var histogram = factory.CreateHistogram("histogram", string.Empty, "label");
        histogram.Observe(1);
        histogram.Observe(10);
        histogram.Observe(20);
        histogram.WithLabels("test").Observe(2);

        _stream = new MemoryStream();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _stream.Seek(0, SeekOrigin.Begin);
    }

    [Benchmark]
    public void Collect()
    {
        ScrapeHandler.ProcessAsync(_registry, _stream).GetAwaiter().GetResult();
    }
}
