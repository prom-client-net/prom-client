extern alias Their;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client.Benchmarks.Comparison
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class SampleResolvingBenchmarks : ComparisonBenchmarkBase
    {
        private const int _labelsCount = 100;

        private IMetricFamily<ICounter> _counterFamily;
        private IMetricFamily<ICounter, (string, string, string, string, string)> _counterTuplesFamily;
        private Their.Prometheus.Counter _theirCounter;

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
                _labels.Add(new string[] { $"lbl1_{i}", $"lbl2_{i}", $"lbl3_{i}", $"lbl4_{i}", $"lbl5_{i}"});
            }

            _counterTuplesFamily = OurMetricFactory.CreateCounter("_counterFamilyTuples", string.Empty, ("label1", "label2", "label3", "label4", "label5" ));
            _counterFamily = OurMetricFactory.CreateCounter("_counterFamily", string.Empty, new string[] { "label1", "label2", "label3", "label4", "label5" });
            _theirCounter = TheirMetricFactory.CreateCounter("_counter", string.Empty, new string[] { "label1", "label2", "label3", "label4", "label5" });

            _gaugeTuplesFamily = OurMetricFactory.CreateGauge("_gaugeFamilyTuples", string.Empty, ("label1", "label2", "label3", "label4", "label5" ));
            _gaugeFamily = OurMetricFactory.CreateGauge("_gaugeFamily", string.Empty, new string[] { "label1", "label2", "label3", "label4", "label5" });
            _theirGauge = TheirMetricFactory.CreateGauge("_gauge", string.Empty, new string[] { "label1", "label2", "label3", "label4", "label5" });
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_ResolveLabeled")]
        public void Counter_ResolveLabeledBaseLine()
        {
            foreach (var lbls in _labels)
                _theirCounter.WithLabels(lbls);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_ResolveLabeled")]
        public void Counter_ResolveLabeled()
        {
            foreach (var lbls in _labels)
                _counterFamily.WithLabels(lbls);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_ResolveLabeled")]
        public void Counter_ResolveLabeledTuples()
        {
            foreach (var lbls in _tupleLabels)
                _counterTuplesFamily.WithLabels(lbls);
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
