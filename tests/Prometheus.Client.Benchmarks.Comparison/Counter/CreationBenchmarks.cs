using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class CreationBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsPerIteration = 10000;

        private const string _help = "arbitrary help message for metric, not relevant for benchmarking";

        private static readonly string[] _metricNames;

        static CreationBenchmarks()
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
        [BenchmarkCategory("Counter_Single")]
        public void Counter_SingleBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter("testcounter", _help);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single")]
        public void Counter_Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", _help);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single")]
        public void CounterInt64_Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", _help);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void Counter_SingleLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter("testcounter", _help, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void Counter_SingleLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", _help, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void Counter_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", _help, ("foo", "bar", "baz"));
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void CounterInt64_SingleLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", _help, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void CounterInt64_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", _help, ("foo", "bar", "baz"));
        }

        private readonly string[] _labelNames = { "foo", "bar", "baz" };

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Single_WithSharedLabels")]
        public void Counter_SingleSharedLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter("testcounter", _help, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithSharedLabels")]
        public void Counter_SingleSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testcounter", _help, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithSharedLabels")]
        public void CounterInt64_SingleSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", _help, _labelNames);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void Counter_ManyBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter(_metricNames[i], _help, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void Counter_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], _help, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void Counter_ManyTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], _help, ("foo", "bar", "baz"));
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void CounterInt64_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], _help, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void CounterInt64_ManyTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], _help, ("foo", "bar", "baz"));
        }
    }
}
