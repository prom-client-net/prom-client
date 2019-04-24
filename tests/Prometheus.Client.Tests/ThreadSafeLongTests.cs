using Xunit;

namespace Prometheus.Client.Tests
{
    public class ThreadSafeLongTests
    {
        [Fact]
        public void ThreadSafeLong_Add()
        {
            var tsdouble = new ThreadSafeLong(3L);
            tsdouble.Add(2L);
            tsdouble.Add(5L);
            Assert.Equal(10L, tsdouble.Value);
        }

        [Fact]
        public void ThreadSafeLong_Constructors()
        {
            var tsdouble = new ThreadSafeLong(0L);
            Assert.Equal(0L, tsdouble.Value);

            tsdouble = new ThreadSafeLong(1L);
            Assert.Equal(1L, tsdouble.Value);
        }

        [Fact]
        public void ThreadSafeLong_Overrides()
        {
            var tsdouble = new ThreadSafeLong(9L);
            var equaltsdouble = new ThreadSafeLong(9L);
            var notequaltsdouble = new ThreadSafeLong(10L);

            Assert.Equal("9", tsdouble.ToString());
            Assert.True(tsdouble.Equals(equaltsdouble));
            Assert.False(tsdouble.Equals(notequaltsdouble));
            Assert.False(tsdouble.Equals(null));
            Assert.True(tsdouble.Equals(9L));
            Assert.False(tsdouble.Equals(10L));

            Assert.Equal(9L.GetHashCode(), tsdouble.GetHashCode());
        }

        [Fact]
        public void ThreadSafeLong_ValueSet()
        {
            var tsdouble = new ThreadSafeLong(3L);
            Assert.Equal(3L, tsdouble.Value);
        }
    }
}
