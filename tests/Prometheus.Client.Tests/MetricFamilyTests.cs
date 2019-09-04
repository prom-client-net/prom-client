using System;
using System.Linq;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class MetricFamilyTests : MetricTestBase
    {
        [Fact]
        public void SameLabelReturnsSameSample()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label");
            var labeled1 = counter.WithLabels("value");
            var labeled2 = counter.WithLabels("value");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameSample_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            var labeled1 = counter.WithLabels("value1", "value2");
            var labeled2 = counter.WithLabels("value1", "value2");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameSample_Tuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
            var labeled1 = counter.WithLabels(("value1", "value2"));
            var labeled2 = counter.WithLabels(("value1", "value2"));

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameSample_StringsAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            var labeled1 = counter.WithLabels("value1", "value2");

            var counterTuple = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
            var labeled2 = counterTuple.WithLabels(("value1", "value2"));

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void ShouldNotAllowWrongNumberOfLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            Assert.Throws<ArgumentException>(() => counter.WithLabels("value1"));
            Assert.Throws<ArgumentException>(() => counter.WithLabels("value1", "value2", "value3"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldNotAllowEmptyLabelValue_Strings(string wrongLabel)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            Assert.Throws<ArgumentException>(() => counter.WithLabels("value1", wrongLabel));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldNotAllowEmptyLabelValue_Tuple(string wrongLabel)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
            Assert.Throws<ArgumentException>(() => counter.WithLabels(("value1", wrongLabel)));
        }

        [Fact]
        public void ShouldThrowIfNoLabels_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, new string[0]);
            Assert.Throws<InvalidOperationException>(() => counter.WithLabels("value1"));
        }

        [Fact]
        public void ShouldThrowIfNoLabels_Tuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create());
            Assert.Throws<InvalidOperationException>(() => counter.WithLabels(ValueTuple.Create()));
        }

        [Fact]
        public void ShouldEnumerateLabeled_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            var labeled = new[]
            {
                new[] { "value1", "value1" },
                new[] { "value2", "value2" },
                new[] { "value3", "value3" },
            };

            foreach (var item in labeled)
                counter.WithLabels(item).Inc();

            var items = counter.Labelled.Select(l => l.Key).ToArray();

            Assert.True(labeled.All(l => items.Any(i => l.SequenceEqual(i))));
        }

        [Fact]
        public void ShouldEnumerateLabeled_Tuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
            var labeled = new[]
            {
                ("value1", "value1"),
                ("value2", "value2"),
                ("value3", "value3"),
            };

            foreach (var item in labeled)
                counter.WithLabels(item).Inc();

            var items = counter.Labelled.Select(l => l.Key).ToArray();

            Assert.True(labeled.All(items.Contains));
        }
    }
}
