using System;
using System.IO;
using NSubstitute;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tests.Resources;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class CounterTests : BaseTests
    {
        [Theory]
        [MemberData(nameof(GetLabels))]
        public void ThrowOnLabelsMismatch(params string[] labels)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            Assert.ThrowsAny<ArgumentException>(() => counter.WithLabels(labels));
        }

        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.ThrowsAny<ArgumentException>(() => factory.CreateCounter("test_counter", string.Empty, label));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3.1)]
        public void MetricsWriterApiUsage(double value)
        {
            var writer = Substitute.For<IMetricsWriter>();
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);
            var counter = factory.CreateCounter("name1", "help1", "label1");

            counter.Inc();
            counter.Inc(value);
            counter.WithLabels("abc").Inc(value);

            ((ICollector)counter).Collect(writer);

            Received.InOrder(() =>
            {
                writer.StartMetric("name1");
                writer.WriteHelp("help1");
                writer.WriteType(MetricType.Counter);

                var sample1 = writer.StartSample();
                sample1.WriteValue(value + 1);
                sample1.EndSample();

                var sample2 = writer.StartSample();
                var lbl = sample2.StartLabels();
                lbl.WriteLabel("label1", "abc");
                lbl.EndLabels();
                sample2.WriteValue(value);
                sample2.EndSample();

                writer.EndMetric();
            });
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
                    ((ICollector)counter).Collect(writer);
                    ((ICollector)counter2).Collect(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                {
                    formattedText = streamReader.ReadToEnd();
                }
            }

            Assert.Equal(ResourcesHelper.GetFileContent("CounterTests_Collection.txt"), formattedText);
        }

        [Fact]
        public void EmptyCollection()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test", "with help text", "category");
            
            string formattedText = null;

            using (var stream = new MemoryStream())
            {
                using (var writer = new MetricsTextWriter(stream))
                {
                    ((ICollector)counter).Collect(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                {
                    formattedText = streamReader.ReadToEnd();
                }
            }

            Assert.Equal(ResourcesHelper.GetFileContent("CounterTests_Empty.txt"), formattedText);
        }

        [Fact]
        public void DefaultIncValue()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty);
            counter.Inc();

            Assert.Equal(1, counter.Value);
        }

        [Fact]
        public void SameLabelReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label");
            var labeled1 = counter.WithLabels("value");
            var labeled2 = counter.WithLabels("value");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            var labeled1 = counter.WithLabels("value1", "value2");
            var labeled2 = counter.WithLabels("value1", "value2");

            Assert.Equal(labeled1, labeled2);
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

        [Fact]
        public void WithoutLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty);
            counter.Inc(2);

            Assert.Equal(2, counter.Value);
        }
    }
}
