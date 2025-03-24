using System;
using Xunit;

namespace Prometheus.Client.Tests.GaugeInt64Tests;

public class SampleTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    public void CanIncrement(long inc)
    {
        var gauge = CreateGauge();
        gauge.Inc(inc);

        Assert.Equal(inc, gauge.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    public void CanDecrement(long dec)
    {
        var gauge = CreateGauge();
        gauge.Dec(dec);

        Assert.Equal(-dec, gauge.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(-42)]
    public void CanSetValue(long value)
    {
        var gauge = CreateGauge();
        gauge.Set(value);

        Assert.Equal(value, gauge.Value);
    }

    [Fact]
    public void DefaultDecValue()
    {
        var gauge = CreateGauge();
        gauge.Dec();

        Assert.Equal(-1, gauge.Value);
    }

    [Fact]
    public void DefaultIncValue()
    {
        var gauge = CreateGauge();
        gauge.Inc();

        Assert.Equal(1, gauge.Value);
    }

    [Fact]
    public void ShouldResetValue()
    {
        var gauge = CreateGauge();
        gauge.Inc();

        gauge.Reset();
        Assert.Equal(0, gauge.Value);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(2, 10, 10)]
    [InlineData(10, 2, 10)]
    [InlineData(-10, 10, 10)]
    [InlineData(-10, -2, -2)]
    [InlineData(-10, -20, -10)]
    public void IncTo(long initial, long value, long expected)
    {
        var gauge = CreateGauge();
        gauge.Set(initial);

        gauge.IncTo(value);

        Assert.Equal(expected, gauge.Value);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(2, 10, 2)]
    [InlineData(10, 2, 2)]
    [InlineData(-10, 10, -10)]
    [InlineData(-10, -2, -10)]
    [InlineData(-10, -20, -20)]
    public void DecTo(long initial, long value, long expected)
    {
        var gauge = CreateGauge();
        gauge.Set(initial);

        gauge.DecTo(value);

        Assert.Equal(expected, gauge.Value);
    }

    private IGauge<long> CreateGauge()
    {
        var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), false);
        return new GaugeInt64(config, Array.Empty<string>());
    }
}
