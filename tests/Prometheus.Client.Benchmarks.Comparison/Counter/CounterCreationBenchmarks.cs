using System;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    public class CounterCreationBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsPerIteration = 10000;

        private readonly string[] _metricNames;
        private readonly string[] _labelNames = { "foo", "bar", "baz" };

        public CounterCreationBenchmarks()
        {
            _metricNames = GenerateMetricNames(_metricsPerIteration);
        }

        [IterationSetup]
        public void Setup()
        {
            ResetFactories();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Single")]
        public void Counter_SingleBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter("testcounter", HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single")]
        public void Counter_Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", HelpText, Array.Empty<string>());
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single")]
        public void CounterInt64_Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", HelpText, Array.Empty<string>());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void Counter_SingleLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter("testcounter", HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void Counter_SingleLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", HelpText, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void Counter_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void CounterInt64_SingleLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", HelpText, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void CounterInt64_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Single_WithSharedLabels")]
        public void Counter_SingleSharedLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter("testcounter", HelpText, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithSharedLabels")]
        public void Counter_SingleSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", HelpText,MetricFlags.Default, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithSharedLabels")]
        public void CounterInt64_SingleSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", HelpText, _labelNames);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Many")]
        public void Counter_ManyBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter(_metricNames[i], HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many")]
        public void Counter_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], HelpText, Array.Empty<string>());
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many")]
        public void CounterInt64_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, Array.Empty<string>());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void Counter_ManyWithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter(_metricNames[i], HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void Counter_ManyWithLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], HelpText,MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void Counter_ManyWithLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void CounterInt64_ManyWithLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void CounterInt64_ManyWithLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, ("foo", "bar", "baz"));
        }
    }
}
