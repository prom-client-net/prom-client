using System.IO;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Summary
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class SummaryCollection
    {
        private CollectorRegistry _registry;
        private MemoryStream _stream;

        [GlobalSetup]
        public void Setup()
        {
            _registry = new CollectorRegistry();
            var factory = new MetricFactory(_registry);
            var summary = factory.CreateSummary("summary", string.Empty, "label");
            summary.Observe(1);
            summary.Observe(10);
            summary.Observe(20);
            summary.WithLabels("test").Observe(2);

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
}
