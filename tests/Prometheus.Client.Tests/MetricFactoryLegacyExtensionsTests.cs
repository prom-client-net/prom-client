using NSubstitute;
using Prometheus.Client.Abstractions;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class IMetricFactoryLegacyExtensionsTests
    {
        [Fact]
        public void CreateCounter()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounter("testName", "testHelp", "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", false, "label1", "label2");
        }

        [Fact]
        public void CreateCounterInt64()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounter("testName", "testHelp", "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", false, "label1", "label2");
        }

        [Fact]
        public void CreateGauge()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGauge("testName", "testHelp", "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", false, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeInt64()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGaugeInt64("testName", "testHelp", "label1", "label2");

            factory.Received().CreateGaugeInt64("testName", "testHelp", false, "label1", "label2");
        }

        [Fact]
        public void CreateHistogram()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateHistogram("testName", "testHelp", "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", false,null, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithTs()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateHistogram("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", true, null, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithBuckets()
        {
            var factory = Substitute.For<IMetricFactory>();
            var buckets = new[] { 0.1, 5 };

            factory.CreateHistogram("testName", "testHelp", buckets, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", false, buckets, "label1", "label2");
        }

        [Fact]
        public void CreateUntyped()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateUntyped("testName", "testHelp", "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", false, "label1", "label2");
        }
    }
}
