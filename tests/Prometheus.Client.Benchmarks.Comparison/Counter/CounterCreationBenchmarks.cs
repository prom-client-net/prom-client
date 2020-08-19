using System;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    public class CounterCreationBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsPerIteration = 10_000;

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
        [BenchmarkCategory("Single")]
        public void Single_Baseline()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter("testcounter", HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Single")]
        public void Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", HelpText, ValueTuple.Create());
        }

        [Benchmark]
        [BenchmarkCategory("Single")]
        public void Single_Int64()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", HelpText, ValueTuple.Create());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Single_WithLabels")]
        public void SingleWithLabels_Baseline()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter("testcounter", HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Single_WithLabels")]
        public void SingleWithLabels_Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", HelpText, false, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Single_WithLabels")]
        public void SingleWithLabels_Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark]
        [BenchmarkCategory("Single_WithLabels")]
        public void SingleWithLabels_Int64Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", HelpText, false, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Single_WithLabels")]
        public void SingleWithLabels_Int64Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Single_WithSharedLabels")]
        public void SingleWithSharedLabels_Baseline()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter("testcounter", HelpText, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Single_WithSharedLabels")]
        public void SingleWithSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", HelpText, false, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Single_WithSharedLabels")]
        public void SingleWithSharedLabels_Int64()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", HelpText, _labelNames);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Many")]
        public void Many_Baseline()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter(_metricNames[i], HelpText);
        }

        [Benchmark]
        [BenchmarkCategory("Many")]
        public void Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], HelpText, ValueTuple.Create());
        }

        [Benchmark]
        [BenchmarkCategory("Many")]
        public void Many_Int64()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, ValueTuple.Create());
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Many_WithLabels")]
        public void ManyWithLabels_Baseline()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter(_metricNames[i], HelpText, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Many_WithLabels")]
        public void ManyWithLabels_Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], HelpText, false, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Many_WithLabels")]
        public void ManyWithLabels_Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], HelpText, ("foo", "bar", "baz"));
        }

        [Benchmark]
        [BenchmarkCategory("Many_WithLabels")]
        public void ManyWithLabels_Int64Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, false, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Many_WithLabels")]
        public void ManyWithLabels_Int64Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, ("foo", "bar", "baz"));
        }
    }
}
