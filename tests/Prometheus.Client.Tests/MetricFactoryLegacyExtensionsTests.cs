using NSubstitute;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class MetricFactoryLegacyExtensionsTests
    {
        [Fact]
        public void CreateCounter()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateCounter("testName", "testHelp", "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateCounterWithTs()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateCounter("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.SupressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateCounterWithTsWithSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateCounter("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.SupressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateCounterWithTsWithoutSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateCounter("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateCounterWithoutTsWithoutSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateCounter("testName", "testHelp", false, false, "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.None, "label1", "label2");
        }

        [Fact]
        public void CreateGauge()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateGauge("testName", "testHelp", "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeWithTs()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateGauge("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", MetricFlags.SupressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeWithTsWithSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateGauge("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", MetricFlags.SupressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeWithTsWithoutSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateGauge("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeWithoutTsWithoutSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateGauge("testName", "testHelp", false, false, "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", MetricFlags.None, "label1", "label2");
        }

        [Fact]
        public void CreateHistogram()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateHistogram("testName", "testHelp", "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", null, MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithTs()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateHistogram("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", null, MetricFlags.SupressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithBuckets()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());
            var buckets = new[] { 0.1, 5 };

            factory.CreateHistogram("testName", "testHelp", buckets, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", buckets, MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithBucketsAndTs()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());
            var buckets = new[] { 0.1, 5 };

            factory.CreateHistogram("testName", "testHelp", true, buckets, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", buckets, MetricFlags.IncludeTimestamp | MetricFlags.SupressEmptySamples, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithBucketsAndTsWithoutSuppress()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());
            var buckets = new[] { 0.1, 5 };

            factory.CreateHistogram("testName", "testHelp", true, false, buckets, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", buckets, MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithBucketsWithoutTsAndSuppress()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());
            var buckets = new[] { 0.1, 5 };

            factory.CreateHistogram("testName", "testHelp", false, false, buckets, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", buckets, MetricFlags.None, "label1", "label2");
        }

        [Fact]
        public void CreateUntyped()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateUntyped("testName", "testHelp", "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateUntypedWithTs()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateUntyped("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", MetricFlags.SupressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateUntypedWithTsWithSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateUntyped("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", MetricFlags.SupressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateUntypedWithTsWithoutSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateUntyped("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateUntypedWithoutTsWithoutSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateUntyped("testName", "testHelp", false, false, "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", MetricFlags.None, "label1", "label2");
        }

        // TODO: Tests for Summary
    }
}
