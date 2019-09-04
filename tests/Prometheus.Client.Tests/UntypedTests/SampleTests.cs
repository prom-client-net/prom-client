using System;
using Prometheus.Client.Abstractions;
using Xunit;

namespace Prometheus.Client.Tests.UntypedTests
{
    public class SampleTests : MetricTestBase
    {
        // TODO: add timestamp test

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3.1)]
        [InlineData(-42)]
        [InlineData(double.NaN)]
        public void CanSet(double val)
        {
            var untyped = CreateUntyped();
            untyped.Set(val);

            Assert.Equal(val, untyped.Value);
        }

        private IUntyped CreateUntyped(MetricFlags options = MetricFlags.Default)
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), options);
            return new Untyped(config, Array.Empty<string>());
        }
    }
}
