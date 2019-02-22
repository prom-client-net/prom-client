using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NSubstitute;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tests.Resources;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class CounterTests
    {
        [Fact]
        public void WithoutLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty);
            counter.Inc(2);

            Assert.Equal(2, counter.Value);
        }

        [Fact]
        public void WithLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label");
            counter.Inc(2);
            var labeled = counter.WithLabels("value");
            labeled.Inc(3);

            Assert.Equal(2, counter.Value);
            Assert.Equal(3, labeled.Value);
        }

        [Theory]
        [InlineData()]
        [InlineData(null)]
        [InlineData(null, null)]
        [InlineData("onlyone")]
        [InlineData("onlyone", null)]
        public void ShouldThrowOnLabelsMismatch(params string[] labels)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            Assert.ThrowsAny<ArgumentException>(() => counter.WithLabels(labels));
        }

        [Fact]
        public void CannotDecrement()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label");
            Assert.Throws<ArgumentOutOfRangeException>(() => counter.Inc(-1));
            var labeled = counter.WithLabels("value");
            Assert.Throws<ArgumentOutOfRangeException>(() => labeled.Inc(-1));
        }

        [Fact]
        public void CanReset()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label");
            counter.Inc(1);
            var labeled = counter.WithLabels("value");
            labeled.Inc(2);

            counter.Reset();

            Assert.Equal(0, counter.Value);
            Assert.Equal(0, labeled.Value);
        }

        [Fact]
        public void Collection()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test", "with help text", "category");
            counter.Inc();
            counter.WithLabels("some").Inc(2);

            var counter2 = factory.CreateCounter("nextcounter", "with help text", "group", "type");
            counter2.Inc(10);
            counter2.WithLabels("any", "2").Inc(5);

            string formattedText = null;

            using (var stream = new MemoryStream())
            {
                using (var writer = new MetricsTextWriter(stream))
                {
                    counter.Collect(writer);
                    counter2.Collect(writer);
                }
                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                {
                    formattedText = streamReader.ReadToEnd();
                }
            }

            Assert.Equal(ResourcesHelper.GetFileContent("CounterTests_Collection.txt"), formattedText);
        }
    }
}
