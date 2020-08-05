extern alias Their;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Histogram
{
    public class HistogramSampleBenchmarks : ComparisonBenchmarkBase
    {
        private const int _opIterations = 1000000;
        private readonly double[] _bucketsMany;

        private Abstractions.IHistogram _histogramDefaultBuckets;
        private Abstractions.IHistogram _histogramManyBuckets;
        private Their.Prometheus.IHistogram _theirHistogramDefaultBuckets;
        private Their.Prometheus.IHistogram _theirHistogramManyBuckets;

        public HistogramSampleBenchmarks()
        {
            _bucketsMany = Enumerable.Range(0, 100).Select(i => (double)(i * 10)).ToArray();
        }

        [IterationSetup]
        public void Setup()
        {
            _histogramDefaultBuckets = OurMetricFactory.CreateHistogram("testhistogram1", HelpText);
            _histogramManyBuckets = OurMetricFactory.CreateHistogram("testhistogram2", HelpText, _bucketsMany);

            _theirHistogramDefaultBuckets = TheirMetricFactory.CreateHistogram("testhistogram1", HelpText);
            _theirHistogramManyBuckets = TheirMetricFactory.CreateHistogram("testhistogram2", HelpText, new Their.Prometheus.HistogramConfiguration() { Buckets = _bucketsMany});
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_Observe")]
        public void Histogram_ObserveBaseLine()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirHistogramDefaultBuckets.Observe(i);
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_Observe")]
        public void Histogram_Observe()
        {
            for (var i = 0; i < _opIterations; i++)
                _histogramDefaultBuckets.Observe(i);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("HistogramManyBuckets_Observe")]
        public void HistogramManyBuckets_ObserveBaseLine()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirHistogramManyBuckets.Observe(i);
        }

        [Benchmark]
        [BenchmarkCategory("HistogramManyBuckets_Observe")]
        public void HistogramManyBuckets_Observe()
        {
            for (var i = 0; i < _opIterations; i++)
                _histogramManyBuckets.Observe(i);
        }
    }
}
