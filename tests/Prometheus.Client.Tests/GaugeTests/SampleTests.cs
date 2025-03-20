using System;
using Xunit;

namespace Prometheus.Client.Tests.GaugeTests;

public class SampleTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3.1)]
    public void CanIncrement(double inc)
    {
        var gauge = CreateGauge();
        gauge.Inc(inc);

        Assert.Equal(inc, gauge.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3.1)]
    public void CanDecrement(double dec)
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
    public void CanSetValue(double value)
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
    public void ShouldAllowNaN()
    {
        var gauge = CreateGauge();

        gauge.Set(double.NaN);

        Assert.Equal(double.NaN, gauge.Value);
    }

    [Fact]
    public void IncShouldIgnoreNaN()
    {
        var gauge = CreateGauge();
        gauge.Set(42);

        gauge.Inc(double.NaN);

        Assert.Equal(42, gauge.Value);
    }

    [Fact]
    public void DecShouldIgnoreNaN()
    {
        var gauge = CreateGauge();
        gauge.Set(42);

        gauge.Dec(double.NaN);

        Assert.Equal(42, gauge.Value);
    }

    [Fact]
    public void ShouldThrowOnIncIfNaN()
    {
        var gauge = CreateGauge();
        gauge.Set(double.NaN);

        Assert.Throws<InvalidOperationException>(() => gauge.Inc());
    }

    [Fact]
    public void ShouldThrowOnDecIfNaN()
    {
        var gauge = CreateGauge();
        gauge.Set(double.NaN);

        Assert.Throws<InvalidOperationException>(() => gauge.Dec());
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
    public void IncTo(double initial, double value, double expected)
    {
        var gauge = CreateGauge();
        gauge.Set(initial);

        gauge.IncTo(value);

        Assert.Equal(expected, gauge.Value);
    }

    [Fact]
    public void IncToThrowsOnNaN()
    {
        var gauge = CreateGauge();

        Assert.Throws<InvalidOperationException>(() => gauge.IncTo(double.NaN));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(2, 10, 2)]
    [InlineData(10, 2, 2)]
    [InlineData(-10, 10, -10)]
    [InlineData(-10, -2, -10)]
    [InlineData(-10, -20, -20)]
    public void DecTo(double initial, double value, double expected)
    {
        var gauge = CreateGauge();
        gauge.Set(initial);

        gauge.DecTo(value);

        Assert.Equal(expected, gauge.Value);
    }

    [Fact]
    public void DecToThrowsOnNaN()
    {
        var gauge = CreateGauge();

        Assert.Throws<InvalidOperationException>(() => gauge.DecTo(double.NaN));
    }

    private IGauge CreateGauge()
    {
        var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), false, TimeSpan.Zero);
        return new Gauge(config, Array.Empty<string>());
    }
}
