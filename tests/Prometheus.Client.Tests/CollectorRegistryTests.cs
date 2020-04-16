using System;
using NSubstitute;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.Tests.Mocks;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class CollectorRegistryTests
    {
        [Fact]
        public void CannotAddDuplicatedCollectors()
        {
            var registry = new CollectorRegistry();
            var collector = new DummyCollector("testName", "metric" );
            var collector1 = new DummyCollector("testName", "metric2");

            registry.Add(collector);
            Assert.Throws<ArgumentException>(() => registry.Add(collector1));
        }

        [Fact]
        public void DoNotCallFactoryIfCollectorExists()
        {
            var registry = new CollectorRegistry();
            var originalCollector = new DummyCollector("testName", "metric" );
            var fn = Substitute.For<Func<CollectorConfiguration, ICollector>>();

            registry.Add(originalCollector);
            var result = registry.GetOrAdd(originalCollector.Configuration, fn);

            Assert.Equal(originalCollector, result);
            fn.DidNotReceiveWithAnyArgs();
        }

        [Theory]
        [InlineData(null, "test")]
        [InlineData(new string[0], "test")]
        public void CollectorShouldDefineMetricNames(string[] metrics, string name)
        {
            // parameter "name" is useless in the test, but it's needed to avoid CS0182 error
            var registry = new CollectorRegistry();
            var collector = new DummyCollector(name, metrics);

            Assert.Throws<ArgumentNullException>(() => registry.Add(collector));
        }

        [Theory]
        [InlineData("my-metric")]
        [InlineData("my!metric")]
        [InlineData("my metric")]
        [InlineData("my%metric")]
        [InlineData(@"my/metric")]
        [InlineData("5a")]
        public void MetricNameShouldBeValid(string metricName)
        {
            var registry = new CollectorRegistry();
            var collector = new DummyCollector("testName", metricName);

            Assert.Throws<ArgumentException>(() => registry.Add(collector));
        }

        [Theory]
        [InlineData(new[] { "metric" }, new[] { "metric" })]
        [InlineData(new[] { "metric" }, new[] { "metric1", "metric" })]
        [InlineData(new[] { "metric1", "metric" }, new[] { "metric" })]
        [InlineData(new[] { "metric1", "metric" }, new[] { "metric2", "metric" })]
        public void CannotAddWithDuplicatedMetricNames(string[] first, string[] second)
        {
            var registry = new CollectorRegistry();
            var collector1 = new DummyCollector("testName1", first);
            var collector2 = new DummyCollector("testName2", second);

            registry.Add(collector1);
            Assert.Throws<ArgumentException>(() => registry.Add(collector2));
        }

        [Fact]
        public void CanRemoveCollectorByName()
        {
            var registry = new CollectorRegistry();
            var collector = new DummyCollector("collector", "metric");
            registry.Add(collector);

            var collector1 = new DummyCollector("collector1", "metric1");
            registry.Add(collector1);

            var res = registry.Remove("collector1");

            Assert.Equal(collector1, res);
            Assert.False(registry.TryGet("collector1", out var _));
            Assert.True(registry.TryGet("collector", out var _));
        }

        [Fact]
        public void CanRemoveCollector()
        {
            var registry = new CollectorRegistry();
            var collector = new DummyCollector("collector", "metric");
            registry.Add(collector);

            var collector1 = new DummyCollector("collector1", "metric1");
            registry.Add(collector1);

            var res = registry.Remove(collector1);

            Assert.True(res);
            Assert.False(registry.TryGet("collector1", out var _));
            Assert.True(registry.TryGet("collector", out var _));
        }
    }
}
