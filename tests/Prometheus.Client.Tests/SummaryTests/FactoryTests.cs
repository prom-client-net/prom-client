using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.SummaryTests;

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

        Assert.Throws<InvalidOperationException>(() => _factory.CreateSummary("test_histogram", string.Empty, "label1", "testlabel"));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateSummary("test_histogram", string.Empty, new[] { "label1" }));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateSummary("test_histogram", string.Empty, "label1", "label2", "label3"));
        Assert.Throws<InvalidOperationException>(() => _factory.CreateSummary("test_histogram", string.Empty, false, TimeSpan.Zero, "label1", "label2", "label3"));
    }

    [Fact]
    public void SingleLabel_ConvertToTuple()
    {
        var metric = _factory.CreateSummary("metricname", "help", "label");
        Assert.Equal(typeof(ValueTuple<string>), metric.LabelNames.GetType());
    }
}
