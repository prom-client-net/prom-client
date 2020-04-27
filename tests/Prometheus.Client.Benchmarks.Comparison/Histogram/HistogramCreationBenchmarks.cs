extern alias Their;
using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Prometheus.Client.Benchmarks.Comparison.Histogram
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class HistogramCreationBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsPerIteration = 10000;

        private static readonly string[] _metricNames;
        private readonly string[] _labelNames = { "foo", "bar", "baz" };
        private readonly double[] _customBuckets = { -1, 0, 1 };

        static HistogramCreationBenchmarks()
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
        [BenchmarkCategory("Histogram_Single")]
        public void Histogram_SingleBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram("testhistogram", HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_Single")]
        public void Histogram_Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testhistogram", HelpText, Array.Empty<string>());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_Single_WithLabels")]
        public void Histogram_SingleLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram("testhistogram", HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_Single_WithLabels")]
        public void Histogram_SingleLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testhistogram", HelpText, null, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_Single_WithLabels")]
        public void Histogram_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testhistogram", HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_Single_WithSharedLabels")]
        public void Histogram_SingleSharedLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram("testhistogram", HelpText, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_Single_WithSharedLabels")]
        public void Histogram_SingleSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testhistogram", HelpText, null, MetricFlags.Default, _labelNames);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_Many")]
        public void Histogram_ManyBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram(_metricNames[i], HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_Many")]
        public void Histogram_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, Array.Empty<string>());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_Many_WithLabels")]
        public void Histogram_ManyWithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram(_metricNames[i], HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_Many_WithLabels")]
        public void Histogram_ManyWithLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, null, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_Many_WithLabels")]
        public void Histogram_ManyWithLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_SingleWithBuckets")]
        public void Histogram_SingleWithBucketsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram("testhistogram", HelpText, new Their.Prometheus.HistogramConfiguration() { Buckets = _customBuckets});
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_SingleWithBuckets")]
        public void Histogram_SingleWithBuckets()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testhistogram", HelpText, _customBuckets);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram_ManyWithBuckets")]
        public void Histogram_ManyWithBucketsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram(_metricNames[i], HelpText, new Their.Prometheus.HistogramConfiguration() { Buckets = _customBuckets});
        }

        [Benchmark]
        [BenchmarkCategory("Histogram_ManyWithBuckets")]
        public void Histogram_ManyWithBuckets()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram(_metricNames[i], HelpText, _customBuckets);
        }
    }
}
