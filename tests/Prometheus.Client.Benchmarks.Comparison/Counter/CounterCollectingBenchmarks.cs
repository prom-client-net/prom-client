extern alias Their;
using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Counter;

public class CounterCollectingBenchmarks : ComparisonBenchmarkBase
{
    private const int _metricsCount = 100;
    private const int _labelsCount = 5;
    private const int _variantsCount = 100;

    public CounterCollectingBenchmarks()
    {
        var labelNames = GenerateLabelNames(_labelsCount).ToArray();
        var labelVariants = GenerateLabelValues(_variantsCount, _labelsCount);
        var rnd = new Random();

        foreach (var metric in GenerateMetricNames(_metricsCount))
        {
            var ourMetric = OurMetricFactory.CreateCounter(metric, HelpText, labelNames);
            var theirMetric = TheirMetricFactory.CreateCounter(metric, HelpText, labelNames);

            foreach (var labels in labelVariants)
            {
                var val = rnd.Next(10000);
                ourMetric.WithLabels(labels).Inc(val);
                theirMetric.WithLabels(labels).Inc(val);
            }
        }
    }

    [Benchmark(Baseline = true)]
    public void Collecting_Baseline()
    {
        using var stream = Stream.Null;
        TheirCollectorRegistry.CollectAndExportAsTextAsync(stream).GetAwaiter().GetResult();
    }

    [Benchmark]
    public void Collecting()
    {
        using var stream = Stream.Null;
        ScrapeHandler.ProcessAsync(OurCollectorRegistry , stream).GetAwaiter().GetResult();
    }
}
