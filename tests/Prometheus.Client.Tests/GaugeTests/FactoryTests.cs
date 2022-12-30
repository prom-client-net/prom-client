using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.GaugeTests;

public class FactoryTests
{
    private readonly IMetricFactory _factory;

    public FactoryTests()
    {
        _factory = new MetricFactory(new CollectorRegistry());
    }

    [Fact]
    public void ThrowOnNameConflict_Strings()
    {
        _factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");

        Assert.Throws<InvalidOperationException>(() => _factory.CreateGauge("test_gauge", string.Empty, Array.Empty<string>()));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateGauge("test_gauge", string.Empty, "label1", "testlabel"));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateGauge("test_gauge", string.Empty, new[] { "label1" }));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateGauge("test_gauge", string.Empty, "label1", "label2", "label3"));
    }

    [Fact]
    public void ThrowOnNameConflict_Tuple()
    {
        _factory.CreateGauge("test_gauge", string.Empty, ("label1", "label2"));

        Assert.Throws<InvalidOperationException>(() => _factory.CreateGauge("test_gauge", string.Empty, ValueTuple.Create()));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateGauge("test_gauge", string.Empty, ValueTuple.Create("label1")));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateGauge("test_gauge", string.Empty, ("label1", "testlabel")));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateGauge("test_gauge", string.Empty, ("label1", "label2", "label3")));
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Strings()
    {
        var gauge1 = _factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");
        var gauge2 = _factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");

        Assert.Equal(gauge1, gauge2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Tuples()
    {
        var gauge1 = _factory.CreateGauge("test_gauge", string.Empty, ("label1", "label2"));
        var gauge2 = _factory.CreateGauge("test_gauge", string.Empty, ("label1", "label2"));

        Assert.Equal(gauge1, gauge2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_StringAndTuple()
    {
        var gauge1 = _factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");
        var gauge2 = _factory.CreateGauge("test_gauge", string.Empty, ("label1", "label2"));

        // Cannot compare metrics families, because of different contracts, should check if sample the same
        Assert.Equal(gauge1.Unlabelled, gauge2.Unlabelled);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Empty()
    {
        var gauge1 = _factory.CreateGauge("test_gauge", string.Empty);
        var gauge2 = _factory.CreateGauge("test_gauge", string.Empty);

        Assert.Equal(gauge1, gauge2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyStrings()
    {
        var gauge1 = _factory.CreateGauge("test_gauge", string.Empty, Array.Empty<string>());
        var gauge2 = _factory.CreateGauge("test_gauge", string.Empty, Array.Empty<string>());

        Assert.Equal(gauge1, gauge2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyTuples()
    {
        var gauge1 = _factory.CreateGauge("test_gauge", string.Empty, ValueTuple.Create());
        var gauge2 = _factory.CreateGauge("test_gauge", string.Empty, ValueTuple.Create());

        Assert.Equal(gauge1, gauge2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyStringAndTuple()
    {
        var gauge1 = _factory.CreateGauge("test_gauge", string.Empty, Array.Empty<string>());
        var gauge2 = _factory.CreateGauge("test_gauge", string.Empty, ValueTuple.Create());

        // Cannot compare metrics families, because of different contracts, should check if sample the same
        Assert.Equal(gauge1.Unlabelled, gauge2.Unlabelled);
    }

    [Fact]
    public void SingleLabel_ConvertToTuple()
    {
        var gauge = _factory.CreateGauge("metricname", "help", "label");
        Assert.Equal(typeof(ValueTuple<string>), gauge.LabelNames.GetType());
    }
}
