extern alias Their;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Abstractions;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    public class CounterSampleResolvingBenchmarks : ComparisonBenchmarkBase
    {
        private const int _samplesCount = 100;

        private IMetricFamily<ICounter> _counterFamily;
        private IMetricFamily<ICounter, (string, string, string, string, string)> _counterTuplesFamily;
        private Their.Prometheus.Counter _theirCounter;

        private string[][] _labels;

        [GlobalSetup]
        public void Setup()
        {
            _labels = GenerateLabelValues(_samplesCount, 5);

            _counterTuplesFamily = OurMetricFactory.CreateCounter("_counterFamilyTuples", string.Empty, ("label1", "label2", "label3", "label4", "label5" ));
            _counterFamily = OurMetricFactory.CreateCounter("_counterFamily", string.Empty, new [] { "label1", "label2", "label3", "label4", "label5" });
            _theirCounter = TheirMetricFactory.CreateCounter("_counter", string.Empty, new [] { "label1", "label2", "label3", "label4", "label5" });
       }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_ResolveLabeled")]
        public void Counter_ResolveLabeledBaseLine()
        {
            foreach (var lbls in _labels)
                _theirCounter.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_ResolveLabeled")]
        public void Counter_ResolveLabeled()
        {
            foreach (var lbls in _labels)
                _counterFamily.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_ResolveLabeled")]
        public void Counter_ResolveLabeledTuples()
        {
            foreach (var lbls in _labels)
                _counterTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]));
        }
    }
}
