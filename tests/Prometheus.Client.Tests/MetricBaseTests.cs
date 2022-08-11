using System;
using Prometheus.Client.Tests.Mocks;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class MetricBaseTests
    {
        [Theory]
        [InlineData(1586594808L, null, 1586594808L)]
        [InlineData(1586594808L, 1586594808L, 1586594808L)]
        [InlineData(1586594808L, 1586594900L, 1586594808L)]
        [InlineData(1586594808L, 1586594700L, 1586594700L)]
        public void TimestampTests(long now, long? ts, long expectedTs)
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), true);

            var metric = new DummyMetric(config, Array.Empty<string>(), () => DateTimeOffset.FromUnixTimeMilliseconds(now));
            metric.Observe(ts);

            Assert.Equal(expectedTs, metric.Timestamp);
        }

        [Fact]
        public void ShouldIgnoreTsIfDisabled()
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), false);

            var metric = new DummyMetric(config, Array.Empty<string>(), null);
            metric.Observe(1586594808);

            Assert.False(metric.Timestamp.HasValue);
        }

        [Fact]
        public void ShouldIgnoreTsIfCurrentIsMore()
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), true);

            var metric = new DummyMetric(config, Array.Empty<string>(), null);
            metric.Observe(1586594808);

            metric.Observe(1586594700);

            Assert.Equal(1586594808, metric.Timestamp);
        }
    }
}
