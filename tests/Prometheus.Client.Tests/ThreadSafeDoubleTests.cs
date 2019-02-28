using Prometheus.Client.Tools;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class ThreadSafeDoubleTests
    {
        [Fact]
        public void ThreadSafeDouble_Add()
        {
            var tsdouble = new ThreadSafeDouble(3.10);
            tsdouble.Add(0.50);
            tsdouble.Add(2.00);
            Assert.Equal(5.6, tsdouble.Value);
        }

        [Fact]
        public void ThreadSafeDouble_Constructors()
        {
            var tsdouble = new ThreadSafeDouble(0.0);
            Assert.Equal(0.0, tsdouble.Value);

            tsdouble = new ThreadSafeDouble(1.42);
            Assert.Equal(1.42, tsdouble.Value);
        }

        [Fact]
        public void ThreadSafeDouble_Overrides()
        {
            var tsdouble = new ThreadSafeDouble(9.15);
            var equaltsdouble = new ThreadSafeDouble(9.15);
            var notequaltsdouble = new ThreadSafeDouble(10.11);

            Assert.Equal("9.15", tsdouble.ToString());
            Assert.True(tsdouble.Equals(equaltsdouble));
            Assert.False(tsdouble.Equals(notequaltsdouble));
            Assert.False(tsdouble.Equals(null));
            Assert.True(tsdouble.Equals(9.15));
            Assert.False(tsdouble.Equals(10.11));

            Assert.Equal(9.15.GetHashCode(), tsdouble.GetHashCode());
        }

        [Fact]
        public void ThreadSafeDouble_ValueSet()
        {
            var tsdouble = new ThreadSafeDouble(3.14);
            Assert.Equal(3.14, tsdouble.Value);
        }
    }
}
