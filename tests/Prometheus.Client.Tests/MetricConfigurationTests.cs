using System;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class MetricConfigurationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ThrowOnInvalidCollectorName(string collectorName)
        {
            Assert.Throws<ArgumentNullException>(() => new MetricConfiguration(collectorName, string.Empty, null, MetricFlags.Default));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("my-metric")]
        [InlineData("my!metric")]
        [InlineData("my%metric")]
        [InlineData("123label")]
        [InlineData("__")]
        [InlineData("__label")]
        public void ThrowOnInvalidLabels(string label)
        {
            Assert.Throws<ArgumentException>(() => new MetricConfiguration("test_metric", string.Empty, new[] { label }, MetricFlags.Default));
        }

        [Theory]
        [InlineData(MetricFlags.None, false, false)]
        [InlineData(MetricFlags.IncludeTimestamp, true, false)]
        [InlineData(MetricFlags.SuppressEmptySamples, false, true)]
        [InlineData(MetricFlags.IncludeTimestamp | MetricFlags.SuppressEmptySamples, true, true)]
        public void CanReadOptions(MetricFlags options, bool includeTimestamp, bool suppressEmptySamples)
        {
            var config = new MetricConfiguration("test_name", string.Empty, null, options);
            Assert.Equal(includeTimestamp, config.IncludeTimestamp);
            Assert.Equal(suppressEmptySamples, config.SuppressEmptySamples);
        }
    }
}
