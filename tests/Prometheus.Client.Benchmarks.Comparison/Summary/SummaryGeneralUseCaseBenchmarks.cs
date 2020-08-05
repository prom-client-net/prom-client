using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Summary
{
    public class SummaryGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 10000;
        private const double _metricsDuplicates = 0.1;
        private const int _samplesCount = 100;
        private const double _samplesDuplicates = 0.1;

        private readonly string[] _metricNames;
        private readonly string[][] _labelValues;

        public SummaryGeneralUseCaseBenchmarks()
        {
            _metricNames = GenerateMetricNames(_metricsCount, _metricsDuplicates);
            _labelValues = GenerateLabelValues(_samplesCount, 3, _samplesDuplicates);
        }

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
                var summary = TheirMetricFactory.CreateSummary(_metricNames[i], HelpText);
                summary.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Summary_NoLabels")]
        public void Summary_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary(_metricNames[i], HelpText);
                summary.Observe(i / 100d);
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_WithLabels")]
        public void Summary_WithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = TheirMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
                summary.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Summary_WithLabels")]
        public void Summary_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
                summary.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Summary_WithLabels")]
        public void Summary_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                summary.Observe(i / 100d);
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_WithLabelsAndSamples")]
        public void Summary_WithLabelsAndSamplesBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = TheirMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    summary.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Summary_WithLabelsAndSamples")]
        public void Summary_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    summary.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Summary_WithLabelsAndSamples")]
        public void Summary_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _samplesCount; j++)
                    summary.WithLabels((_labelValues[j][0], _labelValues[j][1], _labelValues[j][2])).Observe(i / 100d);
            }
        }
    }
}
