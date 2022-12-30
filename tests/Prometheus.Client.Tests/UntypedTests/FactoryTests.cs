using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.UntypedTests;

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
        _factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");

        Assert.Throws<InvalidOperationException>(() => _factory.CreateUntyped("test_untyped", string.Empty, "label1", "testlabel"));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateUntyped("test_untyped", string.Empty, new[] { "label1" }));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2", "label3"));
    }

    [Fact]
    public void ThrowOnNameConflict_Tuple()
    {
        _factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2"));

        Assert.Throws<InvalidOperationException>(() => _factory.CreateUntyped("test_untyped", string.Empty, ValueTuple.Create("label1")));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateUntyped("test_untyped", string.Empty, ("label1", "testlabel")));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2", "label3")));
    }

    [Fact]
    public void ThrowOnNameConflict_StringAndTuple()
    {
        _factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2"));

        Assert.Throws<InvalidOperationException>(() => _factory.CreateUntyped("test_untyped", string.Empty, ("label1", "testlabel")));
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Strings()
    {
        var metric1 = _factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");
        var metric2 = _factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");

        Assert.Equal(metric1, metric2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Tuples()
    {
        var metric1 = _factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2"));
        var metric2 = _factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2"));

        Assert.Equal(metric1, metric2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_StringAndTuple()
    {
        var metric1 = _factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");
        var metric2 = _factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2"));

        // Cannot compare metrics families, because of different contracts, should check if sample the same
        Assert.Equal(metric1.Unlabelled, metric2.Unlabelled);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Empty()
    {
        var metric1 = _factory.CreateUntyped("test_untyped", string.Empty);
        var metric2 = _factory.CreateUntyped("test_untyped", string.Empty);

        Assert.Equal(metric1, metric2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyStrings()
    {
        var metric1 = _factory.CreateUntyped("test_untyped", string.Empty, Array.Empty<string>());
        var metric2 = _factory.CreateUntyped("test_untyped", string.Empty, Array.Empty<string>());

        Assert.Equal(metric1, metric2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyTuples()
    {
        var metric1 = _factory.CreateUntyped("test_untyped", string.Empty, ValueTuple.Create());
        var metric2 = _factory.CreateUntyped("test_untyped", string.Empty, ValueTuple.Create());

        Assert.Equal(metric1, metric2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyStringAndTuple()
    {
        var metric1 = _factory.CreateUntyped("test_untyped", string.Empty, Array.Empty<string>());
        var metric2 = _factory.CreateUntyped("test_untyped", string.Empty, ValueTuple.Create());

        // Cannot compare metrics families, because of different contracts, should check if sample the same
        Assert.Equal(metric1.Unlabelled, metric2.Unlabelled);
    }

    [Fact]
    public void SingleLabel_ConvertToTuple()
    {
        var metric = _factory.CreateUntyped("metricname", "help", "label");
        Assert.Equal(typeof(ValueTuple<string>), metric.LabelNames.GetType());
    }
}
