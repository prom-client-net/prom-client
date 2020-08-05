extern alias Their;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client.Benchmarks.Comparison.Summary
{
    public class SummarySampleResolvingBenchmarks : ComparisonBenchmarkBase
    {
        private const int _labelsCount = 100;

        private IMetricFamily<ISummary> _summaryFamily;
        private IMetricFamily<ISummary, (string, string, string, string, string)> _summaryTuplesFamily;
        private Their.Prometheus.Summary _theirSummary;

        private string[][] _labels;

        [GlobalSetup]
        public void Setup()
        {
            _labels = GenerateLabelValues(_labelsCount, 5);

            _summaryTuplesFamily = OurMetricFactory.CreateSummary("_summaryFamilyTuples", string.Empty, ("label1", "label2", "label3", "label4", "label5" ));
            _summaryFamily = OurMetricFactory.CreateSummary("_summaryFamily", string.Empty, new [] { "label1", "label2", "label3", "label4", "label5" });
            _theirSummary = TheirMetricFactory.CreateSummary("_summary", string.Empty, new [] { "label1", "label2", "label3", "label4", "label5" });
       }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_ResolveLabeled")]
        public void Summary_ResolveLabeledBaseLine()
        {
            foreach (var lbls in _labels)
                _theirSummary.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("Summary_ResolveLabeled")]
        public void Summary_ResolveLabeled()
        {
            foreach (var lbls in _labels)
                _summaryFamily.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("Summary_ResolveLabeled")]
        public void Summary_ResolveLabeledTuples()
        {
            foreach (var lbls in _labels)
                _summaryTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]));
        }
    }
}
