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
    }
}
