using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Prometheus.Client.Benchmarks.Comparison.Gauge
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class GaugeGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 1000;
        private const int _samplesCount = 10;

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
                var gauge = TheirMetricFactory.CreateGauge($"testgauge_{i}", HelpText);
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_NoLabels")]
        public void Gauge_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGauge($"testgauge_{i}", HelpText);
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_NoLabels")]
        public void GaugeInt64_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGaugeInt64($"testgauge_{i}", HelpText);
                gauge.Inc();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_WithLabels")]
        public void Gauge_WithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = TheirMetricFactory.CreateGauge($"testgauge_{i}", HelpText, "foo", "bar", "baz");
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabels")]
        public void Gauge_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGauge($"testgauge_{i}", HelpText, "foo", "bar", "baz");
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabels")]
        public void Gauge_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGauge($"testgauge_{i}", HelpText, ("foo", "bar", "baz"));
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabels")]
        public void GaugeInt64_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGaugeInt64($"testgauge_{i}", HelpText, "foo", "bar", "baz");
                gauge.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabels")]
        public void GaugeInt64_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGaugeInt64($"testgauge_{i}", HelpText, ("foo", "bar", "baz"));
                gauge.Inc();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_WithLabelsAndSamples")]
        public void Gauge_WithLabelsAndSamplesBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = TheirMetricFactory.CreateGauge($"testgauge_{i}", HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    gauge.WithLabels($"a{j}", $"b{j}", $"c{j}").Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabelsAndSamples")]
        public void Gauge_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGauge($"testgauge_{i}", HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    gauge.WithLabels($"a{j}", $"b{j}", $"c{j}").Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabelsAndSamples")]
        public void Gauge_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGauge($"testgauge_{i}", HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _samplesCount; j++)
                    gauge.WithLabels(($"a{j}", $"b{j}", $"c{j}")).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabelsAndSamples")]
        public void GaugeInt64_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGaugeInt64($"testgauge_{i}", HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    gauge.WithLabels($"a{j}", $"b{j}", $"c{j}").Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_WithLabelsAndSamples")]
        public void GaugeInt64_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var gauge = OurMetricFactory.CreateGaugeInt64($"testgauge_{i}", HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _samplesCount; j++)
                    gauge.WithLabels(($"a{j}", $"b{j}", $"c{j}")).Inc();
            }
        }
    }
}
