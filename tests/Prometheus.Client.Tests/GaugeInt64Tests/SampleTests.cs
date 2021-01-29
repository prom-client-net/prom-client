using System;
using Xunit;

namespace Prometheus.Client.Tests.GaugeInt64Tests
{
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

        private IGauge<long> CreateGauge()
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), false);
            return new GaugeInt64(config, Array.Empty<string>());
        }
    }
}
