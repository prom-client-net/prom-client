using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.CounterTests
{
    public class FactoryTests
    {
        [Theory]
        [InlineData]
        [InlineData("label1")]
        [InlineData("label1", "testlabel")]
        [InlineData("label1", "label2", "label3")]
        public void ThrowOnNameConflict_Strings(params string[] labels)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            const string name = "test_counter";
            var expectedLabels = new[] { "label1", "label2" };
            factory.CreateCounter(name, string.Empty, expectedLabels);

            var ex = Assert.Throws<InvalidOperationException>(() => factory.CreateCounter("test_counter", string.Empty, labels));
            Assert.Equal($"Metric name ({name}). Expected labels ({string.Join(", ", expectedLabels)}), but actual labels ({string.Join(", ", labels)})", ex.Message);
        }

        [Fact]
        public void ThrowOnTypeConflict_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            const string name = "test_counter";
            var labels = new[] { "label1", "label2" };
            factory.CreateCounter(name, string.Empty, labels);

            var ex = Assert.Throws<InvalidOperationException>(() => factory.CreateGauge("test_counter", string.Empty, labels));
            Assert.Equal($"Metric name ({name}). Must have same Type. Expected labels ({string.Join(", ", labels)})", ex.Message);
        }

        [Fact]
        public void ThrowOnNameConflict_Tuple0()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            const string name = "test_counter";
            var expectedLabels = ("label1", "label2");
            factory.CreateCounter(name, string.Empty, expectedLabels);

            var labels = ValueTuple.Create();
            var ex = Assert.Throws<InvalidOperationException>(() => factory.CreateCounter("test_counter", string.Empty, labels));

            Assert.Equal($"Metric name ({name}). Must have same Type. Expected labels {expectedLabels}", ex.Message);
        }

        [Fact]
        public void ThrowOnNameConflict_Tuple1()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            const string name = "test_counter";
            var expectedLabels = ("label1", "label2");
            factory.CreateCounter(name, string.Empty, expectedLabels);

            var labels = ValueTuple.Create("label1");
            var ex = Assert.Throws<InvalidOperationException>(() => factory.CreateCounter("test_counter", string.Empty, labels));

            Assert.Equal($"Metric name ({name}). Must have same Type. Expected labels {expectedLabels}", ex.Message);
        }

        [Fact]
        public void ThrowOnNameConflict_Tuple2()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            const string name = "test_counter";
            var expectedLabels = ("label1", "label2");
            factory.CreateCounter(name, string.Empty, expectedLabels);

            var labels = ("label1", "testlabel");
            var ex = Assert.Throws<InvalidOperationException>(() => factory.CreateCounter("test_counter", string.Empty, labels));

            Assert.Equal($"Metric name ({name}). Expected labels {expectedLabels.ToString()}, but actual labels {labels.ToString()}", ex.Message);
        }

        [Fact]
        public void ThrowOnNameConflict_Tuple3()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            const string name = "test_counter";
            var expectedLabels = ("label1", "label2", "label3");
            factory.CreateCounter(name, string.Empty, expectedLabels);

            var labels = ValueTuple.Create("label1");
            var ex = Assert.Throws<InvalidOperationException>(() => factory.CreateCounter("test_counter", string.Empty, labels));

            Assert.Equal($"Metric name ({name}). Must have same Type. Expected labels {expectedLabels}", ex.Message);
        }

        [Fact]
        public void ThrowOnNameConflict_StringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));

            Assert.Throws<InvalidOperationException>(() => factory.CreateCounter("test_counter", string.Empty, ("label1", "testlabel")));
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            var counter2 = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Tuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
            var counter2 = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_StringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            counter1.Unlabelled.Inc();
            var counter2 = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
            counter2.Unlabelled.Inc();

            // Cannot compare metrics families, because of different contracts, should check if sample the same
            Assert.Equal(counter1.Unlabelled, counter2.Unlabelled);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Empty()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty);
            var counter2 = factory.CreateCounter("test_counter", string.Empty);

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyStrings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, Array.Empty<string>());
            var counter2 = factory.CreateCounter("test_counter", string.Empty, Array.Empty<string>());

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyTuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create());
            var counter2 = factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create());

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyStringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, Array.Empty<string>());
            var counter2 = factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create());

            // Cannot compare metrics families, because of different contracts, should check if sample the same
            Assert.Equal(counter1.Unlabelled, counter2.Unlabelled);
        }
    }
}
