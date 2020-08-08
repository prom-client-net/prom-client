extern alias Their;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client.Benchmarks.Comparison.Histogram
{
    public class HistogramSampleResolvingBenchmarks : ComparisonBenchmarkBase
    {
        private IMetricFamily<IHistogram> _histogramFamily;
        private IMetricFamily<IHistogram, (string, string, string, string, string)> _histogramTuplesFamily;
        private Their.Prometheus.Histogram _theirHistogram;

        private string[][] _labels;

        [GlobalSetup]
        public void Setup()
        {
            _labels = GenerateLabelValues(10_000, 5, 0.1);

            _histogramTuplesFamily = OurMetricFactory.CreateHistogram("_histogramFamilyTuples", HelpText, ("label1", "label2", "label3", "label4", "label5" ));
            _histogramFamily = OurMetricFactory.CreateHistogram("_histogramFamily", HelpText, "label1", "label2", "label3", "label4", "label5");
            _theirHistogram = TheirMetricFactory.CreateHistogram("_histogram", HelpText, "label1", "label2", "label3", "label4", "label5");

            foreach (var lbls in _labels)
            {
                _theirHistogram.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
                _histogramFamily.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
                _histogramTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]));
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Baseline()
        {
            foreach (var lbls in _labels)
                _theirHistogram.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Array()
        {
            foreach (var lbls in _labels)
                _histogramFamily.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Tuples()
        {
            foreach (var lbls in _labels)
                _histogramTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]));
        }
    }
}
