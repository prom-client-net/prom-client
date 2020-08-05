extern alias Their;
using System;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Summary
{
    public class SummaryCreationBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsPerIteration = 10000;

        private readonly string[] _metricNames;
        private readonly string[] _labelNames = { "foo", "bar", "baz" };

        public SummaryCreationBenchmarks()
        {
            _metricNames = GenerateMetricNames(_metricsPerIteration);
        }

        [IterationSetup]
        public void Setup()
        {
            ResetFactories();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_Single")]
        public void Summary_SingleBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateSummary("testsummary", HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Summary_Single")]
        public void Summary_Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateSummary("testsummary", HelpText, Array.Empty<string>());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_Single_WithLabels")]
        public void Summary_SingleLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateSummary("testsummary", HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Summary_Single_WithLabels")]
        public void Summary_SingleLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateSummary("testsummary", HelpText,null, null, null, null,MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Summary_Single_WithLabels")]
        public void Summary_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateSummary("testsummary", HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_Single_WithSharedLabels")]
        public void Summary_SingleSharedLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateSummary("testsummary", HelpText, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Summary_Single_WithSharedLabels")]
        public void Summary_SingleSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateSummary("testsummary", HelpText, null, null, null, null, MetricFlags.Default, _labelNames);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_Many")]
        public void Summary_ManyBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateSummary(_metricNames[i], HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Summary_Many")]
        public void Summary_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateSummary(_metricNames[i], HelpText, Array.Empty<string>());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary_Many_WithLabels")]
        public void Summary_ManyWithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateSummary(_metricNames[i], HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Summary_Many_WithLabels")]
        public void Summary_ManyWithLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateSummary(_metricNames[i], HelpText, null, null, null, null, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Summary_Many_WithLabels")]
        public void Summary_ManyWithLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateSummary(_metricNames[i], HelpText, ("foo", "bar", "baz"));
        }
    }
}
