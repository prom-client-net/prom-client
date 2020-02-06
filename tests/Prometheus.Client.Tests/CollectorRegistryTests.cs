using System;
using NSubstitute;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class CollectorRegistryTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CollectorShouldHaveName(string collectorName)
        {
            var registry = new CollectorRegistry();
            var collector = Substitute.For<ICollector>();
            collector.MetricNames.Returns(new[] { "metric" });
            collector.Configuration.Name.Returns(collectorName);

            Assert.Throws<ArgumentNullException>(() => registry.Add(collector));
        }

        [Fact]
        public void CannotAddDuplicatedCollectors()
        {
            var registry = new CollectorRegistry();
            var collector = Substitute.For<ICollector>();
            collector.MetricNames.Returns(new[] { "metric" });
            collector.Configuration.Name.Returns("testName");

            var collector1 = Substitute.For<ICollector>();
            collector1.MetricNames.Returns(new[] { "metric2" });
            collector1.Configuration.Name.Returns("testName");

            registry.Add(collector);
            Assert.Throws<ArgumentException>(() => registry.Add(collector1));
        }

        [Fact]
        public void DoNotCallFactoryIfCollectorExists()
        {
            var registry = new CollectorRegistry();
            var originalCollector = Substitute.For<ICollector>();
            var cfg = new CollectorConfiguration("testName");
            originalCollector.MetricNames.Returns(new[] { "metric" });
            originalCollector.Configuration.Returns(cfg);
            var fn = Substitute.For<Func<CollectorConfiguration, ICollector>>();

            registry.Add(originalCollector);
            var result = registry.GetOrAdd(cfg, fn);

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
            var collector = Substitute.For<ICollector>();
            collector.MetricNames.Returns(metrics);
            collector.Configuration.Name.Returns(name);

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
            var collector = Substitute.For<ICollector>();
            collector.MetricNames.Returns(new[] { metricName });
            collector.Configuration.Name.Returns("tst");

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
            var collector1 = Substitute.For<ICollector>();
            collector1.MetricNames.Returns(first);
            collector1.Configuration.Name.Returns("collector1");

            var collector2 = Substitute.For<ICollector>();
            collector2.MetricNames.Returns(second);
            collector2.Configuration.Name.Returns("collector1");

            registry.Add(collector1);
            Assert.Throws<ArgumentException>(() => registry.Add(collector2));
        }

        [Fact]
        public void CanRemoveByNameCollector()
        {
            var registry = new CollectorRegistry();
            var collector = Substitute.For<ICollector>();
            collector.MetricNames.Returns(new[] { "metric" });
            collector.Configuration.Name.Returns("collector");
            registry.Add(collector);

            var collector1 = Substitute.For<ICollector>();
            collector1.MetricNames.Returns(new[] { "metric1" });
            collector1.Configuration.Name.Returns("metric1");
            registry.Add(collector1);

            var res = registry.Remove("metric1");

            Assert.Equal(collector1, res);
            Assert.False(registry.TryGet("metric1", out var _));
            Assert.True(registry.TryGet("collector", out var _));
        }

        [Fact]
        public void CanRemoveCollector()
        {
            var registry = new CollectorRegistry();
            var collector = Substitute.For<ICollector>();
            collector.MetricNames.Returns(new[] { "metric" });
            collector.Configuration.Name.Returns("collector");
            registry.Add(collector);

            var collector1 = Substitute.For<ICollector>();
            collector1.MetricNames.Returns(new[] { "metric1" });
            collector1.Configuration.Name.Returns("metric1");
            registry.Add(collector1);

            var res = registry.Remove(collector1);

            Assert.True(res);
            Assert.False(registry.TryGet("metric1", out var _));
            Assert.True(registry.TryGet("collector", out var _));
        }
    }
}
