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

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateCounterWithTs()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounter("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateCounterWithTsWithSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounter("testName", "testHelp", true, true, "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateCounterWithTsWithoutSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounter("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateCounterWithoutTsWithoutSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounter("testName", "testHelp", false, false, "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.None, "label1", "label2");
        }
        
        [Fact]
        public void CreateCounterInt64()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounter("testName", "testHelp", "label1", "label2");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateCounterInt64WithTs()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounterInt64("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateCounterInt64("testName", "testHelp", MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateCounterInt64WithTsWithSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounterInt64("testName", "testHelp", true, true, "label1", "label2");

            factory.Received().CreateCounterInt64("testName", "testHelp", MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateCounterInt64WithTsWithoutSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounterInt64("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateCounterInt64("testName", "testHelp", MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateCounterInt64WithoutTsWithoutSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateCounterInt64("testName", "testHelp", false, false, "label1", "label2");

            factory.Received().CreateCounterInt64("testName", "testHelp", MetricFlags.None, "label1", "label2");
        }
        
        [Fact]
        public void CreateGauge()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGauge("testName", "testHelp", "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeWithTs()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGauge("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeWithTsWithSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGauge("testName", "testHelp", true, true, "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeWithTsWithoutSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGauge("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeWithoutTsWithoutSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGauge("testName", "testHelp", false, false, "label1", "label2");

            factory.Received().CreateGauge("testName", "testHelp", MetricFlags.None, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeInt64()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGaugeInt64("testName", "testHelp", "label1", "label2");

            factory.Received().CreateGaugeInt64("testName", "testHelp", MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeInt64WithTs()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGaugeInt64("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateGaugeInt64("testName", "testHelp", MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeInt64WithTsWithSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGaugeInt64("testName", "testHelp", true, true, "label1", "label2");

            factory.Received().CreateGaugeInt64("testName", "testHelp", MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeInt64WithTsWithoutSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGaugeInt64("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateGaugeInt64("testName", "testHelp", MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateGaugeInt64WithoutTsWithoutSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateGaugeInt64("testName", "testHelp", false, false, "label1", "label2");

            factory.Received().CreateGaugeInt64("testName", "testHelp", MetricFlags.None, "label1", "label2");
        }

        [Fact]
        public void CreateHistogram()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateHistogram("testName", "testHelp", "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", null, MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithTs()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateHistogram("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", null, MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithBuckets()
        {
            var factory = Substitute.For<IMetricFactory>();
            var buckets = new[] { 0.1, 5 };

            factory.CreateHistogram("testName", "testHelp", buckets, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", buckets, MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithBucketsAndTs()
        {
            var factory = Substitute.For<IMetricFactory>();
            var buckets = new[] { 0.1, 5 };

            factory.CreateHistogram("testName", "testHelp", true, buckets, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", buckets, MetricFlags.IncludeTimestamp | MetricFlags.SuppressEmptySamples, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithBucketsAndTsWithoutSuppress()
        {
            var factory = Substitute.For<IMetricFactory>();
            var buckets = new[] { 0.1, 5 };

            factory.CreateHistogram("testName", "testHelp", true, false, buckets, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", buckets, MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateHistogramWithBucketsWithoutTsAndSuppress()
        {
            var factory = Substitute.For<IMetricFactory>();
            var buckets = new[] { 0.1, 5 };

            factory.CreateHistogram("testName", "testHelp", false, false, buckets, "label1", "label2");

            factory.Received().CreateHistogram("testName", "testHelp", buckets, MetricFlags.None, "label1", "label2");
        }

        [Fact]
        public void CreateUntyped()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateUntyped("testName", "testHelp", "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", MetricFlags.Default, "label1", "label2");
        }

        [Fact]
        public void CreateUntypedWithTs()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateUntyped("testName", "testHelp", true, "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateUntypedWithTsWithSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateUntyped("testName", "testHelp", true, true, "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", MetricFlags.SuppressEmptySamples | MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateUntypedWithTsWithoutSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateUntyped("testName", "testHelp", true, false, "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", MetricFlags.IncludeTimestamp, "label1", "label2");
        }

        [Fact]
        public void CreateUntypedWithoutTsWithoutSuppressEmpty()
        {
            var factory = Substitute.For<IMetricFactory>();

            factory.CreateUntyped("testName", "testHelp", false, false, "label1", "label2");

            factory.Received().CreateUntyped("testName", "testHelp", MetricFlags.None, "label1", "label2");
        }
    }
}
