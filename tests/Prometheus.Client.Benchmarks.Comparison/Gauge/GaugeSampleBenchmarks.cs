extern alias Their;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Gauge
{
    public class GaugeSampleBenchmarks : ComparisonBenchmarkBase
    {
        private const int _opIterations = 10000000;

        private Abstractions.IGauge _gauge;
        private Abstractions.IGauge<long> _gaugeInt64;
        private Their.Prometheus.IGauge _theirGauge;

        [IterationSetup]
        public void Setup()
        {
            _gauge = OurMetricFactory.CreateGauge("testgauge", HelpText);
            _gaugeInt64 = OurMetricFactory.CreateGaugeInt64("testgaugeInt64", HelpText);

            _theirGauge = TheirMetricFactory.CreateGauge("testgauge", HelpText);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_IncDefault")]
        public void Gauge_IncDefaultBaseLine()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirGauge.Inc();
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_IncDefault")]
        public void Gauge_IncDefault()
        {
            for (var i = 0; i < _opIterations; i++)
                _gauge.Inc();
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_IncDefault")]
        public void GaugeInt64_IncDefault()
        {
            for (var i = 0; i < _opIterations; i++)
                _gaugeInt64.Inc();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Inc")]
        public void Gauge_IncBaseLine()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirGauge.Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Inc")]
        public void Gauge_Inc()
        {
            for (var i = 0; i < _opIterations; i++)
                _gauge.Inc(i);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Inc")]
        public void GaugeInt64_Inc()
        {
            for (var i = 0; i < _opIterations; i++)
                _gaugeInt64.Inc(i);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_DecDefault")]
        public void Gauge_DecDefaultBaseLine()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirGauge.Dec();
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_DecDefault")]
        public void Gauge_DecDefault()
        {
            for (var i = 0; i < _opIterations; i++)
                _gauge.Dec();
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_DecDefault")]
        public void GaugeInt64_DecDefault()
        {
            for (var i = 0; i < _opIterations; i++)
                _gaugeInt64.Dec();
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Dec")]
        public void Gauge_DecBaseLine()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirGauge.Dec(i);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Dec")]
        public void Gauge_Dec()
        {
            for (var i = 0; i < _opIterations; i++)
                _gauge.Dec(i);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Dec")]
        public void GaugeInt64_Dec()
        {
            for (var i = 0; i < _opIterations; i++)
                _gaugeInt64.Dec(i);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Gauge_Set")]
        public void Gauge_SetBaseLine()
        {
            for (var i = 0; i < _opIterations; i++)
                _theirGauge.Set(i);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Set")]
        public void Gauge_Set()
        {
            for (var i = 0; i < _opIterations; i++)
                _gauge.Set(i);
        }

        [Benchmark]
        [BenchmarkCategory("Gauge_Set")]
        public void GaugeInt64_Set()
        {
            for (var i = 0; i < _opIterations; i++)
                _gaugeInt64.Set(i);
        }
    }
}
