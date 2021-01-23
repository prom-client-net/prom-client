extern alias Their;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Gauge
{
    public class GaugeSampleBenchmarks : ComparisonBenchmarkBase
    {
        private const int _opIterations = 10_000_000;

        private IGauge _gauge;
        private IGauge<long> _gaugeInt64;
        private Their.Prometheus.IGauge _theirGauge;

        [IterationSetup]
        public void Setup()
        {
            _gauge = OurMetricFactory.CreateGauge("testgauge", HelpText);
            _gaugeInt64 = OurMetricFactory.CreateGaugeInt64("testgaugeInt64", HelpText);

            _theirGauge = TheirMetricFactory.CreateGauge("testgauge", HelpText);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("IncDefault")]
        public void IncDefault_Baseline()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirGauge.Inc();
        }

        [Benchmark]
        [BenchmarkCategory("IncDefault")]
        public void IncDefault()
        {
            for (var i = 0; i < _opIterations; i++)
                _gauge.Inc();
        }

        [Benchmark]
        [BenchmarkCategory("IncDefault")]
        public void IncDefault_Int64()
        {
            for (var i = 0; i < _opIterations; i++)
                _gaugeInt64.Inc();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Inc")]
        public void Inc_Baseline()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirGauge.Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Inc")]
        public void Inc()
        {
            for (var i = 0; i < _opIterations; i++)
                _gauge.Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Inc")]
        public void Inc_Int64()
        {
            for (var i = 0; i < _opIterations; i++)
                _gaugeInt64.Inc(i);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("DecDefault")]
        public void DecDefault_Baseline()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirGauge.Dec();
        }

        [Benchmark]
        [BenchmarkCategory("DecDefault")]
        public void DecDefault()
        {
            for (var i = 0; i < _opIterations; i++)
                _gauge.Dec();
        }

        [Benchmark]
        [BenchmarkCategory("DecDefault")]
        public void DecDefault_Int64()
        {
            for (var i = 0; i < _opIterations; i++)
                _gaugeInt64.Dec();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Dec")]
        public void Dec_Baseline()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirGauge.Dec(i);
        }

        [Benchmark]
        [BenchmarkCategory("Dec")]
        public void Dec()
        {
            for (var i = 0; i < _opIterations; i++)
                _gauge.Dec(i);
        }

        [Benchmark]
        [BenchmarkCategory("Dec")]
        public void Dec_Int64()
        {
            for (var i = 0; i < _opIterations; i++)
                _gaugeInt64.Dec(i);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Set")]
        public void Set_Baseline()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirGauge.Set(i);
        }

        [Benchmark]
        [BenchmarkCategory("Set")]
        public void Set()
        {
            for (var i = 0; i < _opIterations; i++)
                _gauge.Set(i);
        }

        [Benchmark]
        [BenchmarkCategory("Set")]
        public void Set_Int64()
        {
            for (var i = 0; i < _opIterations; i++)
                _gaugeInt64.Set(i);
        }
    }
}
