extern alias Their;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client.Benchmarks.Comparison.Gauge
{
    public class GaugeSampleResolvingBenchmarks : ComparisonBenchmarkBase
    {
        private const int _labelsCount = 100;

        private IMetricFamily<IGauge> _gaugeFamily;
        private IMetricFamily<IGauge, (string, string, string, string, string)> _gaugeTuplesFamily;
        private Their.Prometheus.Gauge _theirGauge;

        private string[][] _labels;

        [GlobalSetup]
        public void Setup()
        {
            _labels = GenerateLabelValues(_labelsCount, 5);

            _gaugeTuplesFamily = OurMetricFactory.CreateGauge("_gaugeFamilyTuples", HelpText, ("label1", "label2", "label3", "label4", "label5" ));
            _gaugeFamily = OurMetricFactory.CreateGauge("_gaugeFamily", HelpText, new [] { "label1", "label2", "label3", "label4", "label5" });
            _theirGauge = TheirMetricFactory.CreateGauge("_gauge", HelpText, new [] { "label1", "label2", "label3", "label4", "label5" });
       }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_ResolveLabeled")]
        public void Gauge_ResolveLabeledBaseLine()
        {
            foreach (var lbls in _labels)
                _theirGauge.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_ResolveLabeled")]
        public void Gauge_ResolveLabeled()
        {
            foreach (var lbls in _labels)
                _gaugeFamily.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_ResolveLabeled")]
        public void Gauge_ResolveLabeledTuples()
        {
            foreach (var lbls in _labels)
                _gaugeTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]));
        }
    }
}
