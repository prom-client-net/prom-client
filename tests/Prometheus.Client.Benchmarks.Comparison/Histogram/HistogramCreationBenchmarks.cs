extern alias Their;
using System;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Histogram
{
    public class HistogramCreationBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsPerIteration = 10_000;

        private readonly string[] _metricNames;
        private readonly string[] _labelNames = { "foo", "bar", "baz" };

        public HistogramCreationBenchmarks()
        {
            _metricNames = GenerateMetricNames(_metricsPerIteration);
        }

        [IterationSetup]
        public void Setup()
        {
            ResetFactories();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Single")]
        public void Single_Baseline()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram("testhistogram", HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Single")]
        public void Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testhistogram", HelpText, ValueTuple.Create());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Single_WithLabels")]
        public void SingleWithLabels_Baseline()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram("testhistogram", HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Single_WithLabels")]
        public void SingleWithLabels_Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testhistogram", HelpText, null, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Single_WithLabels")]
        public void SingleWithLabels_Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testhistogram", HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Single_WithSharedLabels")]
        public void SingleWithSharedLabels_Baseline()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram("testhistogram", HelpText, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Single_WithSharedLabels")]
        public void SingleWithSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testhistogram", HelpText, null, MetricFlags.Default, _labelNames);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Many")]
        public void Many_Baseline()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram(_metricNames[i], HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Many")]
        public void Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, ValueTuple.Create());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Many_WithLabels")]
        public void ManyWithLabels_Baseline()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram(_metricNames[i], HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Many_WithLabels")]
        public void ManyWithLabels_Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, null, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Many_WithLabels")]
        public void ManyWithLabels_Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, ("foo", "bar", "baz"));
        }
    }
}
