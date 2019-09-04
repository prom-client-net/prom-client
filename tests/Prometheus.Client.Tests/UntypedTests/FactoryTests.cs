using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.UntypedTests
{
    public class FactoryTests : MetricTestBase
    {
        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.Throws<ArgumentException>(() => factory.CreateUntyped("test_untyped", string.Empty, "label1", label));
        }

        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels_Tuple(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.Throws<ArgumentException>(() => factory.CreateUntyped("test_untyped", string.Empty, ValueTuple.Create(label)));
        }

        [Fact]
        public void ThrowOnNameConflict_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");

            Assert.Throws<InvalidOperationException>(() => factory.CreateUntyped("test_untyped", string.Empty, "label1", "testlabel"));
            Assert.Throws<InvalidOperationException>(() => factory.CreateUntyped("test_untyped", string.Empty, new[] { "label1" }));
            Assert.Throws<InvalidOperationException>(() => factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2", "label3"));
        }

        [Fact]
        public void ThrowOnNameConflict_Tuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2"));

            Assert.Throws<InvalidOperationException>(() => factory.CreateUntyped("test_untyped", string.Empty, ValueTuple.Create("label1")));
            Assert.Throws<InvalidOperationException>(() => factory.CreateUntyped("test_untyped", string.Empty, ("label1", "testlabel")));
            Assert.Throws<InvalidOperationException>(() => factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2", "label3")));
        }

        [Fact]
        public void ThrowOnNameConflict_StringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2"));

            Assert.Throws<InvalidOperationException>(() => factory.CreateUntyped("test_untyped", string.Empty, ("label1", "testlabel")));
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped1 = factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");
            var counter2 = factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");

            Assert.Equal(untyped1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Tuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped1 = factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2"));
            var counter2 = factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2"));

            Assert.Equal(untyped1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_StringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped1 = factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");
            var counter2 = factory.CreateUntyped("test_untyped", string.Empty, ("label1", "label2"));

            // Cannot compare metrics families, because of different contracts, should check if sample the same
            Assert.Equal(untyped1.Unlabelled, counter2.Unlabelled);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Empty()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped1 = factory.CreateUntyped("test_untyped", string.Empty);
            var counter2 = factory.CreateUntyped("test_untyped", string.Empty);

            Assert.Equal(untyped1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyStrings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped1 = factory.CreateUntyped("test_untyped", string.Empty, Array.Empty<string>());
            var counter2 = factory.CreateUntyped("test_untyped", string.Empty, Array.Empty<string>());

            Assert.Equal(untyped1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyTuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped1 = factory.CreateUntyped("test_untyped", string.Empty, ValueTuple.Create());
            var counter2 = factory.CreateUntyped("test_untyped", string.Empty, ValueTuple.Create());

            Assert.Equal(untyped1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyStringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped1 = factory.CreateUntyped("test_untyped", string.Empty, Array.Empty<string>());
            var counter2 = factory.CreateUntyped("test_untyped", string.Empty, ValueTuple.Create());

            // Cannot compare metrics families, because of different contracts, should check if sample the same
            Assert.Equal(untyped1.Unlabelled, counter2.Unlabelled);
        }
    }
}
