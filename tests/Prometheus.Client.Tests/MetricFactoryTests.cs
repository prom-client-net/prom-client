using System;
using NSubstitute;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class MetricFactoryTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(16)]
        public void FactoryProxyUsesCache(int labelsCount)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var fn1 = factory.GetCounterFactory(labelsCount);
            var fn2 = factory.GetCounterFactory(labelsCount);

            Assert.True(fn1 == fn2);
        }

        [Fact]
        public void ReleaseCallRegistry()
        {
            var metricName = "some-metric";
            var registry = Substitute.For<ICollectorRegistry>();
            var factory = new MetricFactory(registry);

            factory.Release(metricName);

            registry.Received().Remove(metricName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ReleaseThrowsOnNull(string metricName)
        {
            var registry = Substitute.For<ICollectorRegistry>();
            var factory = new MetricFactory(registry);

            Assert.Throws<ArgumentException>(() => factory.Release(metricName));
        }

        [Fact]
        public void Release2CallRegistry()
        {
            var metricName = "some-metric";
            var metricFamily = Substitute.For<IMetricFamily<IMetric>>();
            metricFamily.Name.Returns(metricName);

            var registry = Substitute.For<ICollectorRegistry>();
            var factory = new MetricFactory(registry);

            factory.Release(metricFamily);

            registry.Received().Remove(metricName);
        }

        [Fact]
        public void Release2ThrowsOnNull()
        {
            var registry = Substitute.For<ICollectorRegistry>();
            var factory = new MetricFactory(registry);
            IMetricFamily<IMetric> family = null;

            Assert.Throws<ArgumentNullException>(() => factory.Release(family));
        }

        [Fact]
        public void Release3CallRegistry()
        {
            var metricName = "some-metric";
            var metricFamily = Substitute.For<IMetricFamily<IMetric, (string label1, string label2)>>();
            metricFamily.Name.Returns(metricName);

            var registry = Substitute.For<ICollectorRegistry>();
            var factory = new MetricFactory(registry);

            factory.Release(metricFamily);

            registry.Received().Remove(metricName);
        }

        [Fact]
        public void Release3ThrowsOnNull()
        {
            var registry = Substitute.For<ICollectorRegistry>();
            var factory = new MetricFactory(registry);
            IMetricFamily<IMetric, (string label1, string label2)> family = null;

            Assert.Throws<ArgumentNullException>(() => factory.Release(family));
        }
    }
}
