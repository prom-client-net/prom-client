using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Summary
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class SummaryCreation
    {
        private MetricFactory _factory;

        [GlobalSetup]
        public void Setup()
        {
            _factory = new MetricFactory(new CollectorRegistry());
        }

        [Benchmark]
        public ISummary Creation()
        {
            return _factory.CreateSummary("summary", string.Empty);
        }

        [Benchmark]
        public IMetricFamily<ISummary> CreationWithLabels()
        {
             return _factory.CreateSummary("summary", "help", "label1", "label2");
        }
    }
}
