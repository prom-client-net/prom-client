using System;
using Xunit;

namespace Prometheus.Client.Tests.UntypedTests;

public class SampleTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3.1)]
    [InlineData(-42)]
    [InlineData(double.NaN)]
    public void CanSet(double val)
    {
        var untyped = CreateUntyped();
        untyped.Set(val);

        Assert.Equal(val, untyped.Value);
    }

    [Fact]
    public void ShouldResetValue()
    {
        var untyped = CreateUntyped();
        untyped.Set(12);

        untyped.Reset();
        Assert.Equal(0, untyped.Value);
    }

    private IUntyped CreateUntyped()
    {
        var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), false);
        return new Untyped(config, Array.Empty<string>());
    }
}
