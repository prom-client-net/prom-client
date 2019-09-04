using System;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Summary
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class SummaryUsage
    {
        private IMetricFamily<ISummary> _summary;
        private Random _rnd;

        [GlobalSetup]
        public void Setup()
        {
            var factory = new MetricFactory(new CollectorRegistry());
            _summary = factory.CreateSummary("summary", string.Empty, "label1", "label2");
            _rnd = new Random();
        }

        [Benchmark]
        public ISummary LabelledCreation()
        {
            return _summary.WithLabels("test label");
        }

        [Benchmark]
        public void Observe()
        {
            _summary.Observe(_rnd.Next(100));
        }
    }
}
