extern alias Their;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client.Benchmarks.Comparison.Histogram
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class HistogramSampleResolvingBenchmarks : ComparisonBenchmarkBase
    {
        private const int _labelsCount = 100;

        private IMetricFamily<IHistogram> _histogramFamily;
        private IMetricFamily<IHistogram, (string, string, string, string, string)> _histogramTuplesFamily;
        private Their.Prometheus.Histogram _theirHistogram;

        private List<string[]> _labels;

        [GlobalSetup]
        public void Setup()
        {
            _labels = new List<string[]>();

            for (var i = 0; i < _labelsCount; i++)
            {
                _labels.Add(new [] { $"lbl1_{i}", $"lbl2_{i}", $"lbl3_{i}", $"lbl4_{i}", $"lbl5_{i}"});
            }

            _histogramTuplesFamily = OurMetricFactory.CreateHistogram("_histogramFamilyTuples", HelpText, ("label1", "label2", "label3", "label4", "label5" ));
            _histogramFamily = OurMetricFactory.CreateHistogram("_histogramFamily", HelpText, new [] { "label1", "label2", "label3", "label4", "label5" });
            _theirHistogram = TheirMetricFactory.CreateHistogram("_histogram", HelpText, new [] { "label1", "label2", "label3", "label4", "label5" });
       }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_ResolveLabeled")]
        public void Histogram_ResolveLabeledBaseLine()
        {
            foreach (var lbls in _labels)
                _theirHistogram.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_ResolveLabeled")]
        public void Histogram_ResolveLabeled()
        {
            foreach (var lbls in _labels)
                _histogramFamily.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_ResolveLabeled")]
        public void Histogram_ResolveLabeledTuples()
        {
            foreach (var lbls in _labels)
                _histogramTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]));
        }
    }
}
