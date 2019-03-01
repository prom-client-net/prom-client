using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class MetricFactoryTests
    {
        [Fact]
        public void ReturnsSameCounter()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test", "");
            Assert.Equal(counter, factory.CreateCounter("test", ""));
        }

        [Fact]
        public void ReturnsSameGauge()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test", "");
            Assert.Equal(gauge, factory.CreateGauge("test", ""));
        }

        [Fact]
        public void ReturnsSameHistogram()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var histogram = factory.CreateHistogram("test", "");
            Assert.Equal(histogram, factory.CreateHistogram("test", ""));
        }

        [Fact]
        public void ReturnsSameSummary()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var summary = factory.CreateSummary("test", "");
            Assert.Equal(summary, factory.CreateSummary("test", ""));
        }

        [Fact]
        public void ReturnsSameUntyped()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test", "");
            Assert.Equal(untyped, factory.CreateUntyped("test", ""));
        }

        [Fact]
        public void ThrowOnDuplicatedMetricNames()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateCounter("test", "");
            Assert.Throws<InvalidOperationException>(() => factory.CreateGauge("test", ""));
        }

        [Theory]
        [InlineData(new string[0], new[] { "label" })]
        [InlineData(new[] { "label" }, new string[0])]
        [InlineData(new[] { "label" }, new[] { "label", "label2"})]
        [InlineData(new[] { "label", "label2" }, new[] { "label" })]
        [InlineData(new[] { "label" }, null)]
        [InlineData(null, new[] { "label" })]
        public void ThrowOnLabelNamesMismatch(string[] labels1, string[] labels2)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateCounter("test", "", labels1);
            Assert.Throws<ArgumentException>(() => factory.CreateCounter("test", "", labels2));
        }
    }
}
