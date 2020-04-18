using System;
using System.Linq;
using Xunit;

namespace Prometheus.Client.Tests.HistogramTests
{
    public class HistogramConfigurationTests
    {
        [Fact]
        public void ShouldNotAllowReservedLabelNames()
        {
            HistogramConfiguration Create()
            {
                return new HistogramConfiguration(
                    "test_name",
                    string.Empty,
                    new[] {"le"},
                    null,
                    MetricFlags.Default);
            }

            Assert.Throws<ArgumentException>(Create);
        }

        [Fact]
        public void ShouldNotAllowEmptyBuckets()
        {
            HistogramConfiguration Create()
            {
                return new HistogramConfiguration(
                    "test_name",
                    string.Empty,
                    Array.Empty<string>(),
                    new double[0],
                    MetricFlags.Default);
            }

            Assert.Throws<ArgumentException>(Create);
        }

        [Fact]
        public void ShouldNotAllowWrongBuckets()
        {
            HistogramConfiguration Create()
            {
                return new HistogramConfiguration(
                    "test_name",
                    string.Empty,
                    Array.Empty<string>(),
                    new [] { 0d, -1d },
                    MetricFlags.Default);
            }

            Assert.Throws<ArgumentException>(Create);
        }

        [Theory]
        [InlineData(new[] { 0d, 1d })]
        [InlineData(new[] { 0d, 1d, double.PositiveInfinity })]
        public void ShouldAppendInfIfNotExists(double[] buckets)
        {
            var config = new HistogramConfiguration(
                "test_name",
                string.Empty,
                Array.Empty<string>(),
                buckets,
                MetricFlags.Default);

            Assert.True(double.IsPositiveInfinity(config.Buckets.Last()));
            Assert.Equal(double.IsPositiveInfinity(buckets.Last()) ? buckets.Length : buckets.Length + 1, config.Buckets.Count);
        }
    }
}
