using System;
using Prometheus.Client.Abstractions;
using Xunit;

namespace Prometheus.Client.Tests.GaugeTests
{
    public class SampleTests : MetricTestBase
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

        private IGauge CreateGauge(MetricFlags options = MetricFlags.Default)
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), options);
            return new Gauge(config, Array.Empty<string>());
        }
    }
}
