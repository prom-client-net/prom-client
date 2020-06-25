using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Prometheus.Client.Benchmarks.Comparison
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class GeneralBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsPerIteration = 10000;

        [IterationSetup]
        public void Setup()
        {
            ResetFactories();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter")]
        public void Counter_BaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter("testCounter", HelpText, "foo", "bar", "baz").Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Counter")]
        public void Counter_Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testCounter", HelpText, MetricFlags.Default, "foo", "bar", "baz").Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Counter")]
        public void Counter_Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter("testCounter", HelpText, ("foo", "bar", "baz")).Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Counter")]
        public void CounterInt64_Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testCounter", HelpText, MetricFlags.Default, "foo", "bar", "baz").Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Counter")]
        public void CounterInt64_Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testCounter", HelpText, ("foo", "bar", "baz")).Inc(i);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge")]
        public void Gauge_BaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge("testGauge", HelpText, "foo", "bar", "baz").Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge")]
        public void Gauge_Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge("testGauge", HelpText, MetricFlags.Default, "foo", "bar", "baz").Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge")]
        public void Gauge_Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge("testGauge", HelpText, ("foo", "bar", "baz")).Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge")]
        public void GaugeInt64_Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGaugeInt64("testGauge", HelpText, MetricFlags.Default, "foo", "bar", "baz").Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge")]
        public void GaugeInt64_Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGaugeInt64("testGauge", HelpText, ("foo", "bar", "baz")).Inc(i);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Histogram")]
        public void Histogram_BaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateHistogram("testHistogram", HelpText, "foo", "bar", "baz").Observe(i);
        }

        [Benchmark]
        [BenchmarkCategory("Histogram")]
        public void Histogram_Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testHistogram", HelpText, null, MetricFlags.Default, "foo", "bar", "baz").Observe(i);
        }

        [Benchmark]
        [BenchmarkCategory("Histogram")]
        public void Histogram_Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateHistogram("testHistogram", HelpText, ("foo", "bar", "baz")).Observe(i);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Summary")]
        public void Summary_BaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateSummary("testSummary", HelpText, "foo", "bar", "baz").Observe(i);
        }

        [Benchmark]
        [BenchmarkCategory("Summary")]
        public void Summary_Array()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateSummary("testSummary", HelpText, "foo", "bar", "baz").Observe(i);
        }

        [Benchmark]
        [BenchmarkCategory("Summary")]
        public void Summary_Tuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateSummary("testSummary", HelpText, ("foo", "bar", "baz")).Observe(i);
        }
    }
}
