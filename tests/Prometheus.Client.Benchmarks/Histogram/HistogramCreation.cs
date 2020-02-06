using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Histogram
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class HistogramCreation
    {
        private MetricFactory _factory;

        [GlobalSetup]
        public void Setup()
        {
            _factory = new MetricFactory(new CollectorRegistry());
        }

        [Benchmark]
        public IHistogram Creation()
        {
            return _factory.CreateHistogram("histogram", string.Empty);
        }

        [Benchmark]
        public IMetricFamily<IHistogram> CreationWithLabels()
        {
             return _factory.CreateHistogram("histogram", "help", "label1", "label2");
        }
    }
}
