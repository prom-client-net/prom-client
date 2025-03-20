using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.HistogramTests;

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
        _factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2");

        Assert.Throws<InvalidOperationException>(() => _factory.CreateHistogram("test_histogram", string.Empty, "label1", "testlabel"));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateHistogram("test_histogram", string.Empty, new[] { "label1" }));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2", "label3"));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateHistogram("test_histogram", string.Empty, false, "label1", "label2", "label3"));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateHistogram("test_histogram", string.Empty, false, TimeSpan.Zero, "label1", "label2", "label3"));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateHistogram("test_histogram", string.Empty, false, Array.Empty<double>(), "label1", "label2", "label3"));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateHistogram("test_histogram", string.Empty, false, TimeSpan.Zero, Array.Empty<double>(), "label1", "label2", "label3"));
    }

    [Fact]
    public void ThrowOnNameConflict_Tuple()
    {
        _factory.CreateHistogram("test_histogram", string.Empty, ("label1", "label2"));

        Assert.Throws<InvalidOperationException>(() => _factory.CreateHistogram("test_histogram", string.Empty, ValueTuple.Create("label1")));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateHistogram("test_histogram", string.Empty, ("label1", "testlabel")));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateHistogram("test_histogram", string.Empty, ("label1", "label2", "label3")));
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Strings()
    {
        var metric1 = _factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2");
        var metric2 = _factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2");

        Assert.Equal(metric1, metric2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Tuples()
    {
        var metric1 = _factory.CreateHistogram("test_histogram", string.Empty, ("label1", "label2"));
        var metric2 = _factory.CreateHistogram("test_histogram", string.Empty, ("label1", "label2"));

        Assert.Equal(metric1, metric2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_StringAndTuple()
    {
        var metric1 = _factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2");
        var metric2 = _factory.CreateHistogram("test_histogram", string.Empty, ("label1", "label2"));

        // Cannot compare metrics families, because of different contracts, should check if sample the same
        Assert.Equal(metric1.Unlabelled, metric2.Unlabelled);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Empty()
    {
        var metric1 = _factory.CreateHistogram("test_histogram", string.Empty);
        var metric2 = _factory.CreateHistogram("test_histogram", string.Empty);

        Assert.Equal(metric1, metric2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyStrings()
    {
        var metric1 = _factory.CreateHistogram("test_histogram", string.Empty, Array.Empty<string>());
        var metric2 = _factory.CreateHistogram("test_histogram", string.Empty, Array.Empty<string>());

        Assert.Equal(metric1, metric2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyTuples()
    {
        var metric1 = _factory.CreateHistogram("test_histogram", string.Empty, ValueTuple.Create());
        var metric2 = _factory.CreateHistogram("test_histogram", string.Empty, ValueTuple.Create());

        Assert.Equal(metric1, metric2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyStringAndTuple()
    {
        var metric1 = _factory.CreateHistogram("test_histogram", string.Empty, Array.Empty<string>());
        var metric2 = _factory.CreateHistogram("test_histogram", string.Empty, ValueTuple.Create());

        // Cannot compare metrics families, because of different contracts, should check if sample the same
        Assert.Equal(metric1.Unlabelled, metric2.Unlabelled);
    }

    [Theory]
    [InlineData("le")]
    [InlineData("le", "label")]
    [InlineData("label", "le")]
    public void ThrowOnReservedLabelNames_Strings(params string[] labels)
    {
        Assert.Throws<ArgumentException>(() => _factory.CreateHistogram("test_Histogram", string.Empty, labels));
    }

    [Fact]
    public void ThrowOnReservedLabelNames_Tuple()
    {
        Assert.Throws<ArgumentException>(() => _factory.CreateHistogram("test_Histogram", string.Empty, ValueTuple.Create("le")));
        Assert.Throws<ArgumentException>(() => _factory.CreateHistogram("test_Histogram", string.Empty, ("le", "label")));
    }

    [Fact]
    public void SingleLabel_ConvertToTuple()
    {
        var metric = _factory.CreateHistogram("metricname", "help", "label");
        Assert.Equal(typeof(ValueTuple<string>), metric.LabelNames.GetType());
    }
}
