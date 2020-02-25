using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Prometheus.Client.Benchmarks.Comparison
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class MetricCreationBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsPerIteration = 10000;

        private const string _help = "arbitrary help message for metric, not relevant for benchmarking";

        private static readonly string[] _metricNames;
        private readonly string[] _labelNames = { "foo", "bar", "baz" };

        static MetricCreationBenchmarks()
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
                OurMetricFactory.CreateCounter("testcounter", _help, MetricFlags.Default, "foo", "bar", "baz");
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
                OurMetricFactory.CreateCounterInt64("testcounter", _help, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithLabels")]
        public void CounterInt64_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", _help, ("foo", "bar", "baz"));
        }



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
                OurMetricFactory.CreateCounter("testcounter", _help,MetricFlags.Default, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Single_WithSharedLabels")]
        public void CounterInt64_SingleSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64("testcounter", _help, _labelNames);
        }




        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Many")]
        public void Counter_ManyBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter(_metricNames[i], _help);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many")]
        public void Counter_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], _help);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many")]
        public void Counter_ManyTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], _help);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many")]
        public void CounterInt64_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], _help);
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many")]
        public void CounterInt64_ManyTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], _help);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void Counter_ManyWithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateCounter(_metricNames[i], _help, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void Counter_ManyWithLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], _help,MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void Counter_ManyWithLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounter(_metricNames[i], _help, ("foo", "bar", "baz"));
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void CounterInt64_ManyWithLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], _help, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Counter_Many_WithLabels")]
        public void CounterInt64_ManyWithLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateCounterInt64(_metricNames[i], _help, ("foo", "bar", "baz"));
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Single")]
        public void Gauge_SingleBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge("testgauge", _help);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single")]
        public void Gauge_Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge("testgauge", _help);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Single_WithLabels")]
        public void Gauge_SingleLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge("testgauge", _help, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single_WithLabels")]
        public void Gauge_SingleLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge("testgauge", _help, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single_WithLabels")]
        public void Gauge_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge("testgauge", _help, ("foo", "bar", "baz"));
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Single_WithSharedLabels")]
        public void Gauge_SingleSharedLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge("testgauge", _help, _labelNames);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Single_WithSharedLabels")]
        public void Gauge_SingleSharedLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge("testgauge", _help, MetricFlags.Default, _labelNames);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Many")]
        public void Gauge_ManyBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge(_metricNames[i], _help);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Many")]
        public void Gauge_Many()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge(_metricNames[i], _help);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Many")]
        public void Gauge_ManyTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge(_metricNames[i], _help);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Many_WithLabels")]
        public void Gauge_ManyWithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                TheirMetricFactory.CreateGauge(_metricNames[i], _help, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Many_WithLabels")]
        public void Gauge_ManyWithLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge(_metricNames[i], _help, MetricFlags.Default, "foo", "bar", "baz");
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Many_WithLabels")]
        public void Gauge_ManyWithLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                OurMetricFactory.CreateGauge(_metricNames[i], _help, ("foo", "bar", "baz"));
        }
    }
}
