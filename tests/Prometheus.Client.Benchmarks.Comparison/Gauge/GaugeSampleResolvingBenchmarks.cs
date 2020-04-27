extern alias Their;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client.Benchmarks.Comparison.Gauge
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class GaugeSampleResolvingBenchmarks : ComparisonBenchmarkBase
    {
        private const int _labelsCount = 100;

        private IMetricFamily<IGauge> _gaugeFamily;
        private IMetricFamily<IGauge, (string, string, string, string, string)> _gaugeTuplesFamily;
        private Their.Prometheus.Gauge _theirGauge;

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

            _gaugeTuplesFamily = OurMetricFactory.CreateGauge("_gaugeFamilyTuples", HelpText, ("label1", "label2", "label3", "label4", "label5" ));
            _gaugeFamily = OurMetricFactory.CreateGauge("_gaugeFamily", HelpText, new [] { "label1", "label2", "label3", "label4", "label5" });
            _theirGauge = TheirMetricFactory.CreateGauge("_gauge", HelpText, new [] { "label1", "label2", "label3", "label4", "label5" });
       }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_ResolveLabeled")]
        public void Gauge_ResolveLabeledBaseLine()
        {
            foreach (var lbls in _labels)
                _theirGauge.WithLabels(lbls);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_ResolveLabeled")]
        public void Gauge_ResolveLabeled()
        {
            foreach (var lbls in _labels)
                _gaugeFamily.WithLabels(lbls);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_ResolveLabeled")]
        public void Gauge_ResolveLabeledTuples()
        {
            foreach (var lbls in _tupleLabels)
                _gaugeTuplesFamily.WithLabels(lbls);
        }
    }
}
