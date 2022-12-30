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
    public void SingleLabel_ConvertToTuple()
    {
        var metric = _factory.CreateSummary("metricname", "help", "label");
        Assert.Equal(typeof(ValueTuple<string>), metric.LabelNames.GetType());
    }
}
