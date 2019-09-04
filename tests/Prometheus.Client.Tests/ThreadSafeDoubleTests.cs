using System;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class ThreadSafeDoubleTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(10)]
        [InlineData(-3.14)]
        [InlineData(3.14)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.Epsilon)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NaN)]
        public void CanInitConstructor(double value)
        {
            var tsdouble = new ThreadSafeDouble(value);
            Assert.Equal(value, tsdouble.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(10)]
        [InlineData(-3.14)]
        [InlineData(3.14)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.Epsilon)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NaN)]
        public void CanSetValue(double value)
        {
            var tsdouble = new ThreadSafeDouble(0);
            tsdouble.Value = value;
            Assert.Equal(value, tsdouble.Value);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(-10, false)]
        [InlineData(10, false)]
        [InlineData(double.MinValue, false)]
        [InlineData(double.MaxValue, false)]
        [InlineData(double.Epsilon, false)]
        [InlineData(double.NegativeInfinity, false)]
        [InlineData(double.PositiveInfinity, false)]
        [InlineData(double.NaN, true)]
        public void IsNaN(double value, bool result)
        {
            var isNaN = ThreadSafeDouble.IsNaN(value);
            Assert.Equal(result, isNaN);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(-10, 10, 0)]
        [InlineData(3.10, 2, 5.10)]
        public void CanAddValue(double initial, double added, double expected)
        {
            var tsdouble = new ThreadSafeDouble(initial);
            tsdouble.Add(added);
            Assert.Equal(expected, tsdouble.Value);
        }

        [Theory]
        [InlineData(double.NaN, 0)]
        [InlineData(0, double.NaN)]
        [InlineData(3.14, double.NaN)]
        [InlineData(double.NaN, double.NaN)]
        public void AddThrowsOnNaN(double initial, double added)
        {
            var tsdouble = new ThreadSafeDouble(initial);
            Assert.Throws<InvalidOperationException>(() => tsdouble.Add(added));
        }
    }
}
