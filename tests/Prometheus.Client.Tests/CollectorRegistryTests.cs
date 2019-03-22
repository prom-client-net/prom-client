using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tests.Resources;
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

            Assert.Throws<ArgumentNullException>(() => registry.Add(collectorName, collector));
        }

        [Fact]
        public void CannotAddDuplicatedCollectors()
        {
            var registry = new CollectorRegistry();
            var collector = Substitute.For<ICollector>();
            collector.MetricNames.Returns(new[] { "metric" });

            var collector1 = Substitute.For<ICollector>();
            collector1.MetricNames.Returns(new[] { "metric" });

            registry.Add("testName", collector);
            Assert.Throws<ArgumentException>(() => registry.Add("testName", collector1));
        }

        [Fact]
        public void DoNotCallFactoryIfCollectorExists()
        {
            var registry = new CollectorRegistry();
            var originalCollector = Substitute.For<ICollector>();
            originalCollector.MetricNames.Returns(new[] { "metric" });
            var fn = Substitute.For<Func<CollectorConfiguration, ICollector>>();
            var cfg = new CollectorConfiguration("testName");

            registry.Add("testName", originalCollector);
            var result = registry.GetOrAdd(cfg, fn);

            Assert.Equal(originalCollector, result);
            fn.DidNotReceiveWithAnyArgs();
        }

        [Theory]
        [InlineData(null, "test")]
        [InlineData(new string[0], "test")]
        public void CollectorShouldDefineMetrics(string[] metrics, string name)
        {
            // parameter "name" is useless in the test, but it's needed to avoid CS0182 error
            var registry = new CollectorRegistry();
            var collector = Substitute.For<ICollector>();
            collector.MetricNames.Returns(metrics);

            Assert.Throws<ArgumentNullException>(() => registry.Add(name, collector));
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

            Assert.Throws<ArgumentException>(() => registry.Add("tst", collector));
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

            var collector2 = Substitute.For<ICollector>();
            collector2.MetricNames.Returns(second);

            registry.Add("collector1", collector1);
            Assert.Throws<ArgumentException>(() => registry.Add("collector1", collector2));
        }

        [Fact]
        public void CanRemoveByNameCollector()
        {
            var registry = new CollectorRegistry();
            var collector = Substitute.For<ICollector>();
            collector.MetricNames.Returns(new[] { "metric" });
            registry.Add("collector", collector);

            var collector1 = Substitute.For<ICollector>();
            collector1.MetricNames.Returns(new[] { "metric1" });
            registry.Add("metric1", collector1);

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
            registry.Add("collector", collector);

            var collector1 = Substitute.For<ICollector>();
            collector1.MetricNames.Returns(new[] { "metric1" });
            registry.Add("metric1", collector1);

            var res = registry.Remove(collector1);

            Assert.True(res);
            Assert.False(registry.TryGet("metric1", out var _));
            Assert.True(registry.TryGet("collector", out var _));
        }

        [Fact]
        public void CanCollectAll()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateCounter("test", "with help text").Inc();
            var gauge = factory.CreateGauge("gauge", "with help text", "group", "type");
            gauge.Inc();
            gauge.WithLabels("any", "2").Dec(5);

            string formattedText = null;

            using (var stream = new MemoryStream())
            {
                using (var writer = new MetricsTextWriter(stream))
                {
                    registry.CollectTo(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                {
                    formattedText = streamReader.ReadToEnd();
                }
            }

            Assert.Equal(ResourcesHelper.GetFileContent("CollectorRegistryTests_Collection.txt"), formattedText);
        }
    }
}
