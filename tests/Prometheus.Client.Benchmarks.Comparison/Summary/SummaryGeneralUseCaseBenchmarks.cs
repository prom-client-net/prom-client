using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Summary
{
    public class SummaryGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 10_000;
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
        [BenchmarkCategory("NoLabels")]
        public void NoLabels_Baseline()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = TheirMetricFactory.CreateSummary(_metricNames[i], HelpText);
                summary.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("NoLabels")]
        public void NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary(_metricNames[i], HelpText);
                summary.Observe(i / 100d);
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("WithLabels")]
        public void WithLabels_Baseline()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = TheirMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
                summary.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabels")]
        public void WithLabels_Array()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
                summary.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabels")]
        public void WithLabels_Tuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                summary.Observe(i / 100d);
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("WithLabelsAndSamples")]
        public void WithLabelsAndSamples_Baseline()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = TheirMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    summary.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabelsAndSamples")]
        public void WithLabelsAndSamples_Array()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var summary = OurMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    summary.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabelsAndSamples")]
        public void WithLabelsAndSamples_Tuple()
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
