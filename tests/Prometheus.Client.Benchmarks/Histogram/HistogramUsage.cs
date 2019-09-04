using System;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Histogram
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class HistogramUsage
    {
        private IMetricFamily<IHistogram> _histogram;
        private Random _rnd;

        [GlobalSetup]
        public void Setup()
        {
            var factory = new MetricFactory(new CollectorRegistry());
            _histogram = factory.CreateHistogram("histogram", string.Empty, "label1", "label2");
            _rnd = new Random();
        }

        [Benchmark]
        public IHistogram LabelledCreation()
        {
            return _histogram.WithLabels("test label");
        }

        [Benchmark]
        public void Observe()
        {
            _histogram.Observe(_rnd.Next(100));
        }
    }
}
