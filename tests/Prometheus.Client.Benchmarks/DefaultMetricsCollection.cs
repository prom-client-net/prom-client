using System.IO;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class DefaultMetricsCollection
    {
        private CollectorRegistry _registry;
        private MemoryStream _stream;

        [GlobalSetup]
        public void Setup()
        {
            _registry = new CollectorRegistry();
            _registry.UseDefaultCollectors();

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
