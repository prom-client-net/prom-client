using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Gauge;

public class GaugeGeneralUseCaseBenchmarks : ComparisonBenchmarkBase
{
    private const int _metricsCount = 10_000;
    private const double _metricsDuplicates = 0.1;
    private const int _samplesCount = 100;
    private const double _samplesDuplicates = 0.1;

    private readonly string[] _metricNames;
    private readonly string[][] _labelValues;

    public GaugeGeneralUseCaseBenchmarks()
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
            var gauge = TheirMetricFactory.CreateGauge(_metricNames[i], HelpText);
            gauge.Inc();
        }
    }

    [Benchmark]
    [BenchmarkCategory("NoLabels")]
    public void NoLabels()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = OurMetricFactory.CreateGauge(_metricNames[i], HelpText);
            gauge.Inc();
        }
    }

    [Benchmark]
    [BenchmarkCategory("NoLabels")]
    public void NoLabels_Int64()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText);
            gauge.Inc();
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("WithLabels")]
    public void WithLabels_Baseline()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = TheirMetricFactory.CreateGauge(_metricNames[i], HelpText, "foo", "bar", "baz");
            gauge.Inc();
        }
    }

    [Benchmark]
    [BenchmarkCategory("WithLabels")]
    public void WithLabels_Array()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = OurMetricFactory.CreateGauge(_metricNames[i], HelpText, false, "foo", "bar", "baz");
            gauge.Inc();
        }
    }

    [Benchmark]
    [BenchmarkCategory("WithLabels")]
    public void WithLabels_Tuple()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = OurMetricFactory.CreateGauge(_metricNames[i], HelpText, ("foo", "bar", "baz"));
            gauge.Inc();
        }
    }

    [Benchmark]
    [BenchmarkCategory("WithLabels")]
    public void WithLabels_Int64Array()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, false, "foo", "bar", "baz");
            gauge.Inc();
        }
    }

    [Benchmark]
    [BenchmarkCategory("WithLabels")]
    public void WithLabels_Int64Tuple()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, ("foo", "bar", "baz"));
            gauge.Inc();
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("WithLabelsAndSamples")]
    public void WithLabelsAndSamples_Baseline()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = TheirMetricFactory.CreateGauge(_metricNames[i], HelpText, "foo", "bar", "baz");
            for(var j = 0; j < _samplesCount; j++)
                gauge.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
        }
    }

    [Benchmark]
    [BenchmarkCategory("WithLabelsAndSamples")]
    public void WithLabelsAndSamples_Array()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = OurMetricFactory.CreateGauge(_metricNames[i], HelpText, false, "foo", "bar", "baz");
            for(var j = 0; j < _samplesCount; j++)
                gauge.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
        }
    }

    [Benchmark]
    [BenchmarkCategory("WithLabelsAndSamples")]
    public void WithLabelsAndSamples_Tuple()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = OurMetricFactory.CreateGauge(_metricNames[i], HelpText, ("foo", "bar", "baz"));
            for(var j = 0; j < _samplesCount; j++)
                gauge.WithLabels((_labelValues[j][0], _labelValues[j][1], _labelValues[j][2])).Inc();
        }
    }

    [Benchmark]
    [BenchmarkCategory("WithLabelsAndSamples")]
    public void WithLabelsAndSamples_Int64Array()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, false, "foo", "bar", "baz");
            for(var j = 0; j < _samplesCount; j++)
                gauge.WithLabels(_labelValues[j][0], _labelValues[j][1], _labelValues[j][2]).Inc();
        }
    }

    [Benchmark]
    [BenchmarkCategory("WithLabelsAndSamples")]
    public void WithLabelsAndSamples_Int64Tuple()
    {
        for (var i = 0; i < _metricsCount; i++)
        {
            var gauge = OurMetricFactory.CreateGaugeInt64(_metricNames[i], HelpText, ("foo", "bar", "baz"));
            for(var j = 0; j < _samplesCount; j++)
                gauge.WithLabels((_labelValues[j][0], _labelValues[j][1], _labelValues[j][2])).Inc();
        }
    }
}
