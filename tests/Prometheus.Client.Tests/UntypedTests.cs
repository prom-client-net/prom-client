using System;
using System.IO;
using NSubstitute;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tests.Resources;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class UntypedTests : BaseTests
    {
        [Theory]
        [MemberData(nameof(GetLabels))]
        public void ThrowOnLabelsMismatch(params string[] labels)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");
            Assert.ThrowsAny<ArgumentException>(() => untyped.WithLabels(labels));
        }

        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.ThrowsAny<ArgumentException>(() => factory.CreateUntyped("test_untyped", string.Empty, label));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3.1)]
        public void MetricsWriterApiUsage(double value)
        {
            var writer = Substitute.For<IMetricsWriter>();
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);
            var untyped = factory.CreateUntyped("name1", "help1", "label1");

            untyped.Set(value + 1);
            untyped.WithLabels("abc").Set(value);

            untyped.Collect(writer);

            Received.InOrder(() =>
            {
                writer.StartMetric("name1");
                writer.WriteHelp("help1");
                writer.WriteType(MetricType.Untyped);

                var sample1 = writer.StartSample();
                sample1.WriteValue(value + 1);

                var sample2 = writer.StartSample();
                var lbl = sample2.StartLabels();
                lbl.WriteLabel("label1", "abc");
                lbl.EndLabels();
                sample2.WriteValue(value);
            });
        }

        [Fact]
        public void Collection()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test", "with help text", "category");
            untyped.Set(1);
            untyped.WithLabels("some").Set(2);

            var untyped2 = factory.CreateUntyped("nextuntyped", "with help text", "group", "type");
            untyped2.Set(10);
            untyped2.WithLabels("any", "2").Set(5);

            string formattedText = null;

            using (var stream = new MemoryStream())
            {
                using (var writer = new MetricsTextWriter(stream))
                {
                    untyped.Collect(writer);
                    untyped2.Collect(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                {
                    formattedText = streamReader.ReadToEnd();
                }
            }

            Assert.Equal(ResourcesHelper.GetFileContent("UntypedTests_Collection.txt"), formattedText);
        }

        [Fact]
        public void EmptyCollection()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test", "with help text", "category");
            
            string formattedText = null;

            using (var stream = new MemoryStream())
            {
                using (var writer = new MetricsTextWriter(stream))
                {
                    untyped.Collect(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                {
                    formattedText = streamReader.ReadToEnd();
                }
            }

            Assert.Equal(ResourcesHelper.GetFileContent("UntypedTests_Empty.txt"), formattedText);
        }

        [Fact]
        public void SameLabelReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty, "label");
            var labeled1 = untyped.WithLabels("value");
            var labeled2 = untyped.WithLabels("value");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");
            var labeled1 = untyped.WithLabels("value1", "value2");
            var labeled2 = untyped.WithLabels("value1", "value2");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void WithLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty, "label");
            untyped.Set(2);
            var labeled = untyped.WithLabels("value");
            labeled.Set(3);

            Assert.Equal(2, untyped.Value);
            Assert.Equal(3, labeled.Value);
        }

        [Fact]
        public void WithoutLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty);
            untyped.Set(2);

            Assert.Equal(2, untyped.Value);
        }
    }
}
