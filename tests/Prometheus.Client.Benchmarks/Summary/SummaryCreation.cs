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
        private IMetricFactory _factory;

        [GlobalSetup]
        public void Setup()
        {
            _factory = new MetricFactory(new CollectorRegistry());
        }

        [Benchmark]
        public ISummary Creation(int i)
        {
            return _factory.CreateSummary($"summary1_{i.ToString()}", string.Empty);
        }

        [Benchmark]
        public IMetricFamily<ISummary> CreationWithLabels()
        {
             return _factory.CreateSummary("summary2", "help", "label1", "label2");
        }
    }
}
