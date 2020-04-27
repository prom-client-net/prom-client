using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Prometheus.Client.Benchmarks.Comparison.Gauge
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class GaugeCreationBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsPerIteration = 10000;

        private static readonly string[] _metricNames;
        private readonly string[] _labelNames = { "foo", "bar", "baz" };

        static GaugeCreationBenchmarks()
        {
            _metricNames = new string[_metricsPerIteration];

            for (var i = 0; i < _metricsPerIteration; i++)
                _metricNames[i] = $"metric_{i:D4}";
        }

        [IterationSetup]
        public void Setup()
        {
            ResetFactories();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Single")]
        public void Gauge_SingleBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge("testgauge", HelpText, Array.Empty<string>());
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single")]
        public void Gauge_Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge("testgauge", HelpText, Array.Empty<string>());
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single")]
        public void GaugeInt64_Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGaugeInt64("testgauge", HelpText);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Single_WithLabels")]
        public void Gauge_SingleLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge("testgauge", HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single_WithLabels")]
        public void Gauge_SingleLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge("testgauge", HelpText, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single_WithLabels")]
        public void Gauge_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge("testgauge", HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single_WithLabels")]
        public void GaugeInt64_SingleLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGaugeInt64("testgauge", HelpText, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single_WithLabels")]
        public void GaugeInt64_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGaugeInt64("testgauge", HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Single_WithSharedLabels")]
        public void Gauge_SingleSharedLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge("testgauge", HelpText, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single_WithSharedLabels")]
        public void Gauge_SingleSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge("testgauge", HelpText,MetricFlags.Default, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single_WithSharedLabels")]
        public void GaugeInt64_SingleSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGaugeInt64("testgauge", HelpText, _labelNames);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Many")]
        public void Gauge_ManyBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge(_metricNames[i], HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Many")]
        public void Gauge_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge(_metricNames[i], HelpText, Array.Empty<string>());
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Many")]
        public void GaugeInt64_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, Array.Empty<string>());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Many_WithLabels")]
        public void Gauge_ManyWithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge(_metricNames[i], HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Many_WithLabels")]
        public void Gauge_ManyWithLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge(_metricNames[i], HelpText,MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Many_WithLabels")]
        public void Gauge_ManyWithLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge(_metricNames[i], HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Many_WithLabels")]
        public void GaugeInt64_ManyWithLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Many_WithLabels")]
        public void GaugeInt64_ManyWithLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, ("foo", "bar", "baz"));
        }
    }
}
