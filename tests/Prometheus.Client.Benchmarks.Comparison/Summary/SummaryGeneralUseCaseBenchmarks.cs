using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Prometheus.Client.Benchmarks.Comparison.Summary
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class SummaryGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 1000;
        private const int _samplesCount = 10;

        [IterationSetup]
        public void Setup()
        {
            ResetFactories();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_NoLabels")]
        public void Summary_NoLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = TheirMetricFactory.CreateSummary($"testSummary_{i}", HelpText);
                summary.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Summary_NoLabels")]
        public void Summary_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary($"testSummary_{i}", HelpText);
                summary.Observe(i / 100d);
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_WithLabels")]
        public void Summary_WithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = TheirMetricFactory.CreateSummary($"testSummary_{i}", HelpText, "foo", "bar", "baz");
                summary.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Summary_WithLabels")]
        public void Summary_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary($"testSummary_{i}", HelpText, "foo", "bar", "baz");
                summary.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Summary_WithLabels")]
        public void Summary_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary($"testSummary_{i}", HelpText, ("foo", "bar", "baz"));
                summary.Observe(i / 100d);
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_WithLabelsAndSamples")]
        public void Summary_WithLabelsAndSamplesBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = TheirMetricFactory.CreateSummary($"testSummary_{i}", HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    summary.WithLabels($"a{j}", $"b{j}", $"c{j}").Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Summary_WithLabelsAndSamples")]
        public void Summary_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary($"testSummary_{i}", HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    summary.WithLabels($"a{j}", $"b{j}", $"c{j}").Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Summary_WithLabelsAndSamples")]
        public void Summary_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary($"testSummary_{i}", HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _samplesCount; j++)
                    summary.WithLabels(($"a{j}", $"b{j}", $"c{j}")).Observe(i / 100d);
            }
        }
    }
}
