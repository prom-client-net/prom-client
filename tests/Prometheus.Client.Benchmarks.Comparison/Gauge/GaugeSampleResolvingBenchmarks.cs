extern alias Their;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Gauge;

public class GaugeSampleResolvingBenchmarks : ComparisonBenchmarkBase
{
    private IMetricFamily<IGauge> _gaugeFamily;
    private IMetricFamily<IGauge, (string, string, string, string, string)> _gaugeTuplesFamily;
    private IMetricFamily<IGauge<long>> _gaugeInt64Family;
    private IMetricFamily<IGauge<long>, (string, string, string, string, string)> _gaugeInt64TuplesFamily;
    private Their.Prometheus.Gauge _theirGauge;

    private string[][] _labels;

    [GlobalSetup]
    public void Setup()
    {
        _labels = GenerateLabelValues(10_000, 5, 0.1);

        _gaugeTuplesFamily = OurMetricFactory.CreateGauge("_gaugeFamilyTuples", HelpText, ("label1", "label2", "label3", "label4", "label5" ));
        _gaugeFamily = OurMetricFactory.CreateGauge("_gaugeFamily", HelpText, "label1", "label2", "label3", "label4", "label5");
        _gaugeInt64TuplesFamily = OurMetricFactory.CreateGaugeInt64("_gaugeInt64FamilyTuples", HelpText, ("label1", "label2", "label3", "label4", "label5" ));
        _gaugeInt64Family = OurMetricFactory.CreateGaugeInt64("_gaugeInt64Family", HelpText, "label1", "label2", "label3", "label4", "label5");
        _theirGauge = TheirMetricFactory.CreateGauge("_gauge", HelpText, "label1", "label2", "label3", "label4", "label5");

        foreach (var lbls in _labels)
        {
            _theirGauge.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
            _gaugeFamily.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
            _gaugeTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]));
            _gaugeInt64Family.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
            _gaugeInt64TuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]));
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ResolveLabeled")]
    public void ResolveLabeled_Baseline()
    {
        foreach (var lbls in _labels)
            _theirGauge.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
    }

    [Benchmark]
    [BenchmarkCategory("ResolveLabeled")]
    public void ResolveLabeled_Array()
    {
        foreach (var lbls in _labels)
            _gaugeFamily.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
    }

    [Benchmark]
    [BenchmarkCategory("ResolveLabeled")]
    public void ResolveLabeled_Tuple()
    {
        foreach (var lbls in _labels)
            _gaugeTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]));
    }

    [Benchmark]
    [BenchmarkCategory("ResolveLabeled")]
    public void ResolveLabeled_Int64Array()
    {
        foreach (var lbls in _labels)
            _gaugeInt64Family.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
    }

    [Benchmark]
    [BenchmarkCategory("ResolveLabeled")]
    public void ResolveLabeled_Int64Tuple()
    {
        foreach (var lbls in _labels)
            _gaugeInt64TuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]));
    }
}
