using System;
using System.IO;
using System.Threading.Tasks;
using NSubstitute;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;
using Prometheus.Client.Tests.Resources;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class GaugeTests : BaseTests
    {
        [Theory]
        [MemberData(nameof(GetLabels))]
        public void ThrowOnLabelsMismatch(params string[] labels)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");
            Assert.ThrowsAny<ArgumentException>(() => gauge.WithLabels(labels));
        }

        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.ThrowsAny<ArgumentException>(() => factory.CreateGauge("test_gauge", string.Empty, label));
        }

        [Fact]
        public void CanDecrement()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty, "label");
            gauge.Dec(5);
            var labeled = gauge.WithLabels("value");
            labeled.Dec(1);

            Assert.Equal(-5, gauge.Value);
            Assert.Equal(-1, labeled.Value);
        }

        [Fact]
        public void CanSetValue()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty, "label");
            gauge.Inc(5);
            var labeled = gauge.WithLabels("value");
            labeled.Inc(1);

            gauge.Set(10);
            labeled.Set(20);

            Assert.Equal(10, gauge.Value);
            Assert.Equal(20, labeled.Value);
        }

        [Fact]
        public async Task Collection()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test", "with help text", "category");
            gauge.Inc();
            gauge.WithLabels("some").Inc(5);

            var gauge2 = factory.CreateGauge("nextgauge", "with help text", "group", "type");
            gauge2.Inc(1);
            gauge2.WithLabels("any", "2").Dec(5);

            string formattedText = null;

            using (var stream = new MemoryStream())
            {
                using (var writer = new MetricsTextWriter(stream))
                {
                    ((ICollector)gauge).Collect(writer);
                    ((ICollector)gauge2).Collect(writer);
                    await writer.CloseWriterAsync();
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                {
                    formattedText = streamReader.ReadToEnd();
                }
            }

            Assert.Equal(ResourcesHelper.GetFileContent("GaugeTests_Collection.txt"), formattedText);
        }

        [Fact]
        public async Task EmptyCollection()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test", "with help text", false, false, "category");

            string formattedText;

            using (var stream = new MemoryStream())
            {
                using (var writer = new MetricsTextWriter(stream))
                {
                    ((ICollector)gauge).Collect(writer);
                    await writer.CloseWriterAsync();
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                {
                    formattedText = streamReader.ReadToEnd();
                }
            }

            Assert.Equal(ResourcesHelper.GetFileContent("GaugeTests_Empty.txt"), formattedText);
        }

        [Fact]
        public async Task SuppressEmpty()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test", "with help text", "category");
            gauge.WithLabels("some").Inc(5);

            string formattedText = null;

            using (var stream = new MemoryStream())
            {
                using (var writer = new MetricsTextWriter(stream))
                {
                    ((ICollector)gauge).Collect(writer);
                    await writer.CloseWriterAsync();
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                {
                    formattedText = streamReader.ReadToEnd();
                }
            }

            Assert.Equal(ResourcesHelper.GetFileContent("GaugeTests_SuppressEmpty.txt"), formattedText);
        }

        [Fact]
        public void DefaultDecValue()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty);
            gauge.Dec();

            Assert.Equal(-1, gauge.Value);
        }

        [Fact]
        public void DefaultIncValue()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty);
            gauge.Inc();

            Assert.Equal(1, gauge.Value);
        }

        [Fact]
        public void MetricsWriterApiUsage()
        {
            var writer = Substitute.For<IMetricsWriter>();
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);
            var gauge = factory.CreateGauge("name1", "help1");

            gauge.Inc(3.2);

            ((ICollector)gauge).Collect(writer);

            Received.InOrder(() =>
            {
                writer.StartMetric("name1");
                writer.WriteHelp("help1");
                writer.WriteType(MetricType.Gauge);

                var sample1 = writer.StartSample();
                sample1.WriteValue(3.2);
                sample1.EndSample();

                writer.EndMetric();
            });
        }

        [Fact]
        public void SameLabelReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty, "label");
            var labeled1 = gauge.WithLabels("value");
            var labeled2 = gauge.WithLabels("value");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");
            var labeled1 = gauge.WithLabels("value1", "value2");
            var labeled2 = gauge.WithLabels("value1", "value2");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void WithLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty, "label");
            gauge.Inc(2);
            var labeled = gauge.WithLabels("value");
            labeled.Inc(3);

            Assert.Equal(2, gauge.Value);
            Assert.Equal(3, labeled.Value);
        }

        [Fact]
        public void WithoutLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty);
            gauge.Inc(2);

            Assert.Equal(2, gauge.Value);
        }
    }
}
