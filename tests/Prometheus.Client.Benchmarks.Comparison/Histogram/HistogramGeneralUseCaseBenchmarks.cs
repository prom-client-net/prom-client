using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Histogram
{
    public class HistogramGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 10000;
        private const double _metricsDuplicates = 0.1;
        private const int _samplesCount = 100;
        private const double _samplesDuplicates = 0.1;

        private readonly string[] _metricNames;
        private readonly string[][] _labelValues;

        public HistogramGeneralUseCaseBenchmarks()
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
        [BenchmarkCategory("Histogram_NoLabels")]
        public void Histogram_NoLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = TheirMetricFactory.CreateHistogram(_metricNames[i], HelpText);
                histogram.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_NoLabels")]
        public void Histogram_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = OurMetricFactory.CreateHistogram(_metricNames[i], HelpText);
                histogram.Observe(i / 100d);
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_WithLabels")]
        public void Histogram_WithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = TheirMetricFactory.CreateHistogram(_metricNames[i], HelpText, "foo", "bar", "baz");
                histogram.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_WithLabels")]
        public void Histogram_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, "foo", "bar", "baz");
                histogram.Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_WithLabels")]
        public void Histogram_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                histogram.Observe(i / 100d);
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_WithLabelsAndSamples")]
        public void Histogram_WithLabelsAndSamplesBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = TheirMetricFactory.CreateHistogram(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    histogram.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_WithLabelsAndSamples")]
        public void Histogram_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    histogram.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Observe(i / 100d);
            }
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_WithLabelsAndSamples")]
        public void Histogram_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var histogram = OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _samplesCount; j++)
                    histogram.WithLabels((_labelValues[j][0], _labelValues[j][1], _labelValues[j][2])).Observe(i / 100d);
            }
        }
    }
}
