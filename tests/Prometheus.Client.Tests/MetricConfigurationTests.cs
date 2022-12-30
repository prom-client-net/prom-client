using System;
using Xunit;

namespace Prometheus.Client.Tests;

public class MetricConfigurationTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ThrowOnInvalidCollectorName(string collectorName)
    {
        Assert.Throws<ArgumentNullException>(() => new MetricConfiguration(collectorName, string.Empty, null, false));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("my-metric")]
    [InlineData("my!metric")]
    [InlineData("my%metric")]
    [InlineData("123label")]
    [InlineData("__")]
    [InlineData("__label")]
    public void ThrowOnInvalidLabels(string label)
    {
        Assert.Throws<ArgumentException>(() => new MetricConfiguration("test_metric", string.Empty, new[] { label }, false));
    }
}
