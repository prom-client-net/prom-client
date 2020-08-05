using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    public class CounterGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 10000;
        private const double _metricsDuplicates = 0.1;
        private const int _samplesCount = 100;
        private const double _samplesDuplicates = 0.1;

        private readonly string[] _metricNames;
        private readonly string[][] _labelValues;

        public CounterGeneralUseCaseBenchmarks()
        {
            _metricNames = GenerateMetricNames(_metricsCount, _metricsDuplicates);
            _labelValues = GenerateLabelValues(_samplesCount, 3, _samplesDuplicates);
        }

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
                var counter = TheirMetricFactory.CreateCounter(_metricNames[i], HelpText);
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_NoLabels")]
        public void Counter_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter(_metricNames[i], HelpText);
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_NoLabels")]
        public void CounterInt64_NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText);
                counter.Inc();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_WithLabels")]
        public void Counter_WithLabelsBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = TheirMetricFactory.CreateCounter(_metricNames[i], HelpText, "foo", "bar", "baz");
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabels")]
        public void Counter_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter(_metricNames[i], HelpText, "foo", "bar", "baz");
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabels")]
        public void Counter_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabels")]
        public void CounterInt64_WithLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, "foo", "bar", "baz");
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabels")]
        public void CounterInt64_WithLabelsTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                counter.Inc();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Counter_WithLabelsAndSamples")]
        public void Counter_WithLabelsAndSamplesBaseLine()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = TheirMetricFactory.CreateCounter(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _labelValues.Length; j++)
                    counter.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabelsAndSamples")]
        public void Counter_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _labelValues.Length; j++)
                    counter.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabelsAndSamples")]
        public void Counter_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _labelValues.Length; j++)
                    counter.WithLabels((_labelValues[j][0], _labelValues[j][1], _labelValues[j][2])).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabelsAndSamples")]
        public void CounterInt64_WithLabelsAndSamples()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _labelValues.Length; j++)
                    counter.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("Counter_WithLabelsAndSamples")]
        public void CounterInt64_WithLabelsAndSamplesTuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _labelValues.Length; j++)
                    counter.WithLabels((_labelValues[j][0], _labelValues[j][1], _labelValues[j][2])).Inc();
            }
        }
    }
}
