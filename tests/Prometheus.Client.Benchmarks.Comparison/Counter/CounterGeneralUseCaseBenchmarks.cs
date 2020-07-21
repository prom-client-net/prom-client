using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class CounterGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 1000;
        private const int _samplesCount = 10;

        [IterationSetup]
        public void Setup()
        {
            ResetFactories();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_NoLabels")]
        public void Counter_NoLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = TheirMetricFactory.CreateCounter($"testcounter_{i}", HelpText);
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_NoLabels")]
        public void Counter_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter($"testcounter_{i}", HelpText);
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_NoLabels")]
        public void CounterInt64_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64($"testcounter_{i}", HelpText);
                counter.Inc();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_WithLabels")]
        public void Counter_WithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = TheirMetricFactory.CreateCounter($"testcounter_{i}", HelpText, "foo", "bar", "baz");
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabels")]
        public void Counter_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter($"testcounter_{i}", HelpText, "foo", "bar", "baz");
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabels")]
        public void Counter_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter($"testcounter_{i}", HelpText, ("foo", "bar", "baz"));
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabels")]
        public void CounterInt64_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64($"testcounter_{i}", HelpText, "foo", "bar", "baz");
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabels")]
        public void CounterInt64_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64($"testcounter_{i}", HelpText, ("foo", "bar", "baz"));
                counter.Inc();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_WithLabelsAndSamples")]
        public void Counter_WithLabelsAndSamplesBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = TheirMetricFactory.CreateCounter($"testcounter_{i}", HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    counter.WithLabels($"a{j}", $"b{j}", $"c{j}").Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabelsAndSamples")]
        public void Counter_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter($"testcounter_{i}", HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    counter.WithLabels($"a{j}", $"b{j}", $"c{j}").Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabelsAndSamples")]
        public void Counter_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter($"testcounter_{i}", HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _samplesCount; j++)
                    counter.WithLabels(($"a{j}", $"b{j}", $"c{j}")).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabelsAndSamples")]
        public void CounterInt64_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64($"testcounter_{i}", HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _samplesCount; j++)
                    counter.WithLabels($"a{j}", $"b{j}", $"c{j}").Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabelsAndSamples")]
        public void CounterInt64_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64($"testcounter_{i}", HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _samplesCount; j++)
                    counter.WithLabels(($"a{j}", $"b{j}", $"c{j}")).Inc();
            }
        }
    }
}
