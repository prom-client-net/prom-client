using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.GaugeTests
{
    public class FactoryTests : MetricTestBase
    {
        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.Throws<ArgumentException>(() => factory.CreateGauge("test_gauge", string.Empty, "label1", label));
        }

        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels_Tuple(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.Throws<ArgumentException>(() => factory.CreateGauge("test_gauge", string.Empty, ValueTuple.Create(label)));
        }

        [Fact]
        public void ThrowOnNameConflict_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");

            Assert.Throws<InvalidOperationException>(() => factory.CreateGauge("test_gauge", string.Empty, "label1", "testlabel"));
            Assert.Throws<InvalidOperationException>(() => factory.CreateGauge("test_gauge", string.Empty, new[] { "label1" }));
            Assert.Throws<InvalidOperationException>(() => factory.CreateGauge("test_gauge", string.Empty, "label1", "label2", "label3"));
        }

        [Fact]
        public void ThrowOnNameConflict_Tuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateGauge("test_gauge", string.Empty, ("label1", "label2"));

            Assert.Throws<InvalidOperationException>(() => factory.CreateGauge("test_gauge", string.Empty, ValueTuple.Create("label1")));
            Assert.Throws<InvalidOperationException>(() => factory.CreateGauge("test_gauge", string.Empty, ("label1", "testlabel")));
            Assert.Throws<InvalidOperationException>(() => factory.CreateGauge("test_gauge", string.Empty, ("label1", "label2", "label3")));
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge1 = factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");
            var gauge2 = factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");

            Assert.Equal(gauge1, gauge2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Tuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge1 = factory.CreateGauge("test_gauge", string.Empty, ("label1", "label2"));
            var gauge2 = factory.CreateGauge("test_gauge", string.Empty, ("label1", "label2"));

            Assert.Equal(gauge1, gauge2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_StringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge1 = factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");
            var gauge2 = factory.CreateGauge("test_gauge", string.Empty, ("label1", "label2"));

            // Cannot compare metrics families, because of different contracts, should check if sample the same
            Assert.Equal(gauge1.Unlabelled, gauge2.Unlabelled);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Empty()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge1 = factory.CreateGauge("test_gauge", string.Empty);
            var gauge2 = factory.CreateGauge("test_gauge", string.Empty);

            Assert.Equal(gauge1, gauge2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyStrings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge1 = factory.CreateGauge("test_gauge", string.Empty, Array.Empty<string>());
            var gauge2 = factory.CreateGauge("test_gauge", string.Empty, Array.Empty<string>());

            Assert.Equal(gauge1, gauge2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyTuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge1 = factory.CreateGauge("test_gauge", string.Empty, ValueTuple.Create());
            var gauge2 = factory.CreateGauge("test_gauge", string.Empty, ValueTuple.Create());

            Assert.Equal(gauge1, gauge2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyStringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge1 = factory.CreateGauge("test_gauge", string.Empty, Array.Empty<string>());
            var gauge2 = factory.CreateGauge("test_gauge", string.Empty, ValueTuple.Create());

            // Cannot compare metrics families, because of different contracts, should check if sample the same
            Assert.Equal(gauge1.Unlabelled, gauge2.Unlabelled);
        }
    }
}
