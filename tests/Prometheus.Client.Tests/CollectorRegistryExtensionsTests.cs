using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class CollectorRegistryExtensionsTests
    {
        [Fact]
        public void MoveToThrowsOnNullDestination()
        {
            var source = new CollectorRegistry();
            var factory = new MetricFactory(source);

            factory.CreateCounter("test_counter", string.Empty).Inc();

            Assert.Throws<ArgumentNullException>(() => source.MoveTo("test_counter", null));
        }

        [Fact]
        public void MoveToThrowsOnNonExistingCollector()
        {
            var source = new CollectorRegistry();
            var destination = new CollectorRegistry();

            Assert.Throws<ArgumentOutOfRangeException>(() => source.MoveTo("test_counter", destination));
        }

        [Fact]
        public void MoveToRemovesCollectorFromSource()
        {
            var source = new CollectorRegistry();
            var factory = new MetricFactory(source);
            var destination = new CollectorRegistry();
            factory.CreateCounter("test_counter", string.Empty).Inc();

            source.MoveTo("test_counter", destination);
            Assert.False(source.TryGet("test_counter", out _));
        }

        [Fact]
        public void MoveToAddsCollectorToDestination()
        {
            var source = new CollectorRegistry();
            var factory = new MetricFactory(source);
            var destination = new CollectorRegistry();
            factory.CreateCounter("test_counter", string.Empty).Inc();

            source.MoveTo("test_counter", destination);
            Assert.True(destination.TryGet("test_counter", out _));
        }

        [Fact]
        public void CopyToThrowsOnNullDestination()
        {
            var source = new CollectorRegistry();
            var factory = new MetricFactory(source);

            factory.CreateCounter("test_counter", string.Empty).Inc();

            Assert.Throws<ArgumentNullException>(() => source.CopyTo("test_counter", null));
        }

        [Fact]
        public void CopyToThrowsOnNonExistingCollector()
        {
            var source = new CollectorRegistry();
            var destination = new CollectorRegistry();

            Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo("test_counter", destination));
        }

        [Fact]
        public void CopyToRetainsCollectorInSource()
        {
            var source = new CollectorRegistry();
            var factory = new MetricFactory(source);
            var destination = new CollectorRegistry();
            factory.CreateCounter("test_counter", string.Empty).Inc();

            source.CopyTo("test_counter", destination);
            Assert.True(source.TryGet("test_counter", out _));
        }

        [Fact]
        public void CopyToAddsCollectorToDestination()
        {
            var source = new CollectorRegistry();
            var factory = new MetricFactory(source);
            var destination = new CollectorRegistry();
            factory.CreateCounter("test_counter", string.Empty).Inc();

            source.CopyTo("test_counter", destination);
            Assert.True(destination.TryGet("test_counter", out _));
        }
    }
}
