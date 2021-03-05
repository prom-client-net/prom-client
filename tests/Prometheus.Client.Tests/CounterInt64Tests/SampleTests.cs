using System;
using Xunit;

namespace Prometheus.Client.Tests.CounterInt64Tests
{
    public class SampleTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        public void CanIncrement(long inc)
        {
            var counter = CreateCounter();
            counter.Inc(inc);

            Assert.Equal(inc, counter.Value);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-3)]
        public void CannotDecrement(long inc)
        {
            var counter = CreateCounter();
            Assert.Throws<ArgumentOutOfRangeException>(() => counter.Inc(inc));
        }

        [Fact]
        public void DefaultIncrement()
        {
            var counter = CreateCounter();
            counter.Inc();

            Assert.Equal(1, counter.Value);
        }

        [Fact]
        public void ShouldResetValue()
        {
            var counter = CreateCounter();
            counter.Inc();

            counter.Reset();
            Assert.Equal(0, counter.Value);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(2, 10, 10)]
        [InlineData(10, 2, 10)]
        public void IncTo(long initial, long value, long expected)
        {
            var counter = CreateCounter();
            counter.Inc(initial);

            counter.IncTo(value);

            Assert.Equal(expected, counter.Value);
        }

        private CounterInt64 CreateCounter()
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), false);
            return new CounterInt64(config, Array.Empty<string>());
        }
    }
}
