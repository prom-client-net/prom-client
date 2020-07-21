using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Prometheus.Client.Benchmarks.Comparison.Histogram
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class HistogramGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 1000;
        private const int _samplesCount = 10;

        [IterationSetup]
        public void Setup()
        {
            ResetFactories();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_NoLabels")]
        public void Histogram_NoLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = TheirMetricFactory.CreateHistogram($"testHistogram_{i}", HelpText);
                histogram.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_NoLabels")]
        public void Histogram_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = OurMetricFactory.CreateHistogram($"testHistogram_{i}", HelpText);
                histogram.Observe(i / 100d);
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_WithLabels")]
        public void Histogram_WithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = TheirMetricFactory.CreateHistogram($"testHistogram_{i}", HelpText, "foo", "bar", "baz");
                histogram.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_WithLabels")]
        public void Histogram_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = OurMetricFactory.CreateHistogram($"testHistogram_{i}", HelpText, "foo", "bar", "baz");
                histogram.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_WithLabels")]
        public void Histogram_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = OurMetricFactory.CreateHistogram($"testHistogram_{i}", HelpText, ("foo", "bar", "baz"));
                histogram.Observe(i / 100d);
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_WithLabelsAndSamples")]
        public void Histogram_WithLabelsAndSamplesBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = TheirMetricFactory.CreateHistogram($"testHistogram_{i}", HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    histogram.WithLabels($"a{j}", $"b{j}", $"c{j}").Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_WithLabelsAndSamples")]
        public void Histogram_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = OurMetricFactory.CreateHistogram($"testHistogram_{i}", HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    histogram.WithLabels($"a{j}", $"b{j}", $"c{j}").Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_WithLabelsAndSamples")]
        public void Histogram_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = OurMetricFactory.CreateHistogram($"testHistogram_{i}", HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _samplesCount; j++)
                    histogram.WithLabels(($"a{j}", $"b{j}", $"c{j}")).Observe(i / 100d);
            }
        }
    }
}
