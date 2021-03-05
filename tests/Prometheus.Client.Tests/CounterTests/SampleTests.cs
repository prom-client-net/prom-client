using System;
using Xunit;

namespace Prometheus.Client.Tests.CounterTests
{
    public class SampleTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3.1)]
        public void CanIncrement(double inc)
        {
            var counter = CreateCounter();
            counter.Inc(inc);

            Assert.Equal(inc, counter.Value);
        }

        [Fact]
        public void ShouldIgnoreNaN()
        {
            var counter = CreateCounter();
            counter.Inc(42);

            counter.Inc(double.NaN);

            Assert.Equal(42, counter.Value);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-3.1)]
        public void CannotDecrement(double inc)
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
        public void IncTo(double initial, double value, double expected)
        {
            var counter = CreateCounter();
            counter.Inc(initial);

            counter.IncTo(value);

            Assert.Equal(expected, counter.Value);
        }

        [Fact]
        public void IncToThrowsOnNaN()
        {
            var counter = CreateCounter();
            Assert.Throws<InvalidOperationException>(() => counter.IncTo(double.NaN));
        }

        private Counter CreateCounter()
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), false);
            return new Counter(config, Array.Empty<string>());
        }
    }
}
