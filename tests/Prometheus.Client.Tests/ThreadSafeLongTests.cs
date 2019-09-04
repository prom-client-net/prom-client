using Xunit;

namespace Prometheus.Client.Tests
{
    public class ThreadSafeLongTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(10)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        public void CanInitConstructor(long value)
        {
            var tslong = new ThreadSafeLong(value);
            Assert.Equal(value, tslong.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(10)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        public void CanSetValue(long value)
        {
            var tslong = new ThreadSafeLong(0);
            tslong.Add(value);
            Assert.Equal(value, tslong.Value);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(-10, 10, 0)]
        [InlineData(10, 2, 12)]
        public void CanAddValue(long initial, long added, long expected)
        {
            var tslong = new ThreadSafeLong(initial);
            tslong.Add(added);
            Assert.Equal(expected, tslong.Value);
        }
    }
}
