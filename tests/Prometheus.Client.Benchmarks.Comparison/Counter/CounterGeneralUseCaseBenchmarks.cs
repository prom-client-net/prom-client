using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    public class CounterGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 10_000;
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
        [BenchmarkCategory("NoLabels")]
        public void NoLabels_Baseline()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = TheirMetricFactory.CreateCounter(_metricNames[i], HelpText);
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("NoLabels")]
        public void NoLabels()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter(_metricNames[i], HelpText);
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("NoLabels")]
        public void NoLabels_Int64()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText);
                counter.Inc();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("WithLabels")]
        public void WithLabels_Baseline()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = TheirMetricFactory.CreateCounter(_metricNames[i], HelpText, "foo", "bar", "baz");
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabels")]
        public void WithLabels_Array()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter(_metricNames[i], HelpText, "foo", "bar", "baz");
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabels")]
        public void WithLabels_Tuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabels")]
        public void WithLabels_Int64Array()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, "foo", "bar", "baz");
                counter.Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabels")]
        public void WithLabels_Int64Tuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                counter.Inc();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("WithLabelsAndSamples")]
        public void WithLabelsAndSamples_Baseline()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = TheirMetricFactory.CreateCounter(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _labelValues.Length; j++)
                    counter.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabelsAndSamples")]
        public void WithLabelsAndSamples_Array()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _labelValues.Length; j++)
                    counter.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabelsAndSamples")]
        public void WithLabelsAndSamples_Tuple()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounter(_metricNames[i], HelpText, ("foo", "bar", "baz"));
                for(var j = 0; j < _labelValues.Length; j++)
                    counter.WithLabels((_labelValues[j][0], _labelValues[j][1], _labelValues[j][2])).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabelsAndSamples")]
        public void WithLabelsAndSamples_Int64Array()
        {
            for (var i = 0; i < _metricsCount; i++)
            {
                var counter = OurMetricFactory.CreateCounterInt64(_metricNames[i], HelpText, "foo", "bar", "baz");
                for(var j = 0; j < _labelValues.Length; j++)
                    counter.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
            }
        }

        [Benchmark]
        [BenchmarkCategory("WithLabelsAndSamples")]
        public void WithLabelsAndSamples_Int64Tuple()
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
