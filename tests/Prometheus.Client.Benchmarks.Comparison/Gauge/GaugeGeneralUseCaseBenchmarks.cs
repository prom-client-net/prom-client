using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Gauge
{
    public class GaugeGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 10000;
        private const double _metricsDuplicates = 0.1;
        private const int _samplesCount = 100;
        private const double _samplesDuplicates = 0.1;

        private readonly string[] _metricNames;
        private readonly string[][] _labelValues;

        public GaugeGeneralUseCaseBenchmarks()
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
        [BenchmarkCategory("Gauge_NoLabels")]
        public void Gauge_NoLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = TheirMetricFactory.CreateGauge(_metricNames[i], HelpText);
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_NoLabels")]
        public void Gauge_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGauge(_metricNames[i], HelpText);
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_NoLabels")]
        public void GaugeInt64_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText);
                gauge.Inc();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_WithLabels")]
        public void Gauge_WithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = TheirMetricFactory.CreateGauge(_metricNames[i], HelpText, "foo", "bar", "baz");
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabels")]
        public void Gauge_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGauge(_metricNames[i], HelpText, "foo", "bar", "baz");
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabels")]
        public void Gauge_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGauge(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabels")]
        public void GaugeInt64_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, "foo", "bar", "baz");
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabels")]
        public void GaugeInt64_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                gauge.Inc();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_WithLabelsAndSamples")]
        public void Gauge_WithLabelsAndSamplesBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = TheirMetricFactory.CreateGauge(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    gauge.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabelsAndSamples")]
        public void Gauge_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGauge(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    gauge.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabelsAndSamples")]
        public void Gauge_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGauge(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _samplesCount; j++)
                    gauge.WithLabels((_labelValues[j][0], _labelValues[j][1], _labelValues[j][2])).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabelsAndSamples")]
        public void GaugeInt64_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    gauge.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabelsAndSamples")]
        public void GaugeInt64_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _samplesCount; j++)
                    gauge.WithLabels((_labelValues[j][0], _labelValues[j][1], _labelValues[j][2])).Inc();
            }
        }
    }
}
