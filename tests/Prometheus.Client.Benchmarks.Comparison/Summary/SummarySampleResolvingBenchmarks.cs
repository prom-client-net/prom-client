extern alias Their;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client.Benchmarks.Comparison.Summary
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class SummarySampleResolvingBenchmarks : ComparisonBenchmarkBase
    {
        private const int _labelsCount = 100;

        private IMetricFamily<ISummary> _summaryFamily;
        private IMetricFamily<ISummary, (string, string, string, string, string)> _summaryTuplesFamily;
        private Their.Prometheus.Summary _theirSummary;

        private List<string[]> _labels;
        private List<(string, string, string, string, string)> _tupleLabels;

        [GlobalSetup]
        public void Setup()
        {
            _labels = new List<string[]>();
            _tupleLabels = new List<(string, string, string, string, string)>();

            for (var i = 0; i < _labelsCount; i++)
            {
                _tupleLabels.Add(($"lbl1_{i}", $"lbl2_{i}", $"lbl3_{i}", $"lbl4_{i}", $"lbl5_{i}"));
                _labels.Add(new [] { $"lbl1_{i}", $"lbl2_{i}", $"lbl3_{i}", $"lbl4_{i}", $"lbl5_{i}"});
            }

            _summaryTuplesFamily = OurMetricFactory.CreateSummary("_summaryFamilyTuples", string.Empty, ("label1", "label2", "label3", "label4", "label5" ));
            _summaryFamily = OurMetricFactory.CreateSummary("_summaryFamily", string.Empty, new [] { "label1", "label2", "label3", "label4", "label5" });
            _theirSummary = TheirMetricFactory.CreateSummary("_summary", string.Empty, new [] { "label1", "label2", "label3", "label4", "label5" });
       }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_ResolveLabeled")]
        public void Summary_ResolveLabeledBaseLine()
        {
            foreach (var lbls in _labels)
                _theirSummary.WithLabels(lbls);
        }

        [Benchmark]
        [BenchmarkCategory("Summary_ResolveLabeled")]
        public void Summary_ResolveLabeled()
        {
            foreach (var lbls in _labels)
                _summaryFamily.WithLabels(lbls);
        }

        [Benchmark]
        [BenchmarkCategory("Summary_ResolveLabeled")]
        public void Summary_ResolveLabeledTuples()
        {
            foreach (var lbls in _tupleLabels)
                _summaryTuplesFamily.WithLabels(lbls);
        }
    }
}
