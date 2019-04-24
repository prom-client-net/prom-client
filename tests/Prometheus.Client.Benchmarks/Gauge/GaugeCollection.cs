using System.IO;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Gauge
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class GaugeCollection
    {
        private CollectorRegistry _registry;
        private MemoryStream _stream;

        [GlobalSetup]
        public void Setup()
        {
            _registry = new CollectorRegistry();
            var factory = new MetricFactory(_registry);
            var gauge = factory.CreateGauge("gauge", string.Empty, "label");
            gauge.Inc();
            gauge.WithLabels("test").Inc(2);

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
