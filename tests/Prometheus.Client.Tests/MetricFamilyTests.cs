using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Prometheus.Client.Tests.Mocks;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class MetricFamilyTests
    {
        [Fact]
        public void SameLabelsReturnsSameSample_Strings()
        {
            var metricFamily = CreateMetricFamily("label1", "label2");
            var labeled1 = metricFamily.WithLabels("value1", "value2");
            var labeled2 = metricFamily.WithLabels("value1", "value2");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameSample_Tuples()
        {
            var metricFamily = CreateMetricFamily(("label1", "label2"));
            var labeled1 = metricFamily.WithLabels(("value1", "value2"));
            var labeled2 = metricFamily.WithLabels(("value1", "value2"));

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameSample_StringsAndTuple()
        {
            var metricFamily = CreateMetricFamily(("label1", "label2"));
            var labeled1 = ((IMetricFamily<IDummyMetric>)metricFamily).WithLabels("value1", "value2");
            var labeled2 = metricFamily.WithLabels(("value1", "value2"));

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void ShouldNotAllowWrongNumberOfLabels()
        {
            var metricFamily = CreateMetricFamily("label1", "label2");
            Assert.Throws<ArgumentException>(() => metricFamily.WithLabels("value1"));
            Assert.Throws<ArgumentException>(() => metricFamily.WithLabels("value1", "value2", "value3"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldNotAllowEmptyLabelValue_Strings(string wrongLabel)
        {
            var metricFamily = CreateMetricFamily("label1", "label2");
            Assert.Throws<ArgumentException>(() => metricFamily.WithLabels("value1", wrongLabel));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldNotAllowEmptyLabelValue_Tuple(string wrongLabel)
        {
            var metricFamily = CreateMetricFamily(("label1", "label2"));
            Assert.Throws<ArgumentException>(() => metricFamily.WithLabels(("value1", wrongLabel)));
        }

        [Fact]
        public void ShouldThrowIfNoLabels_Strings()
        {
            var metricFamily = CreateMetricFamily();
            Assert.Throws<InvalidOperationException>(() => metricFamily.WithLabels("value1"));
        }

        [Fact]
        public void ShouldThrowIfNoLabels_Tuple()
        {
            var metricFamily = CreateMetricFamily(ValueTuple.Create());
            Assert.Throws<InvalidOperationException>(() => metricFamily.WithLabels(ValueTuple.Create()));
        }

        [Fact]
        public void ShouldRemoveSample_Tuple()
        {
            var metricFamily = CreateMetricFamily(("label1", "label2"));
            var sample = metricFamily.WithLabels(("a", "b"));

            var removed = metricFamily.RemoveLabelled(("a", "b"));

            Assert.Empty(metricFamily.Labelled);
            Assert.Equal(sample, removed);
        }

        [Fact]
        public void ShouldReturnNullOnRemoveNonExistingSample_Tuple()
        {
            var metricFamily = CreateMetricFamily(("label1", "label2"));
            metricFamily.WithLabels(("a", "b"));

            var removed = metricFamily.RemoveLabelled(("b", "c"));

            Assert.Single(metricFamily.Labelled);
            Assert.Null(removed);
        }

        [Fact]
        public void ShouldRemoveSample_Array()
        {
            var metricFamily = CreateMetricFamily("label1", "label2");
            var sample = metricFamily.WithLabels("a", "b");

            var removed = metricFamily.RemoveLabelled("a", "b");

            Assert.Empty(metricFamily.Labelled);
            Assert.Equal(sample, removed);
        }

        [Fact]
        public void ShouldReturnNullOnRemoveNonExistingSample_Array()
        {
            var metricFamily = CreateMetricFamily("label1", "label2");
            metricFamily.WithLabels("a", "b");

            var removed = metricFamily.RemoveLabelled("b", "c");

            Assert.Single(metricFamily.Labelled);
            Assert.Null(removed);
        }

        [Fact]
        public void RemoveThrowIfNoLabels_Array()
        {
            var metricFamily = CreateMetricFamily();
            Assert.Throws<InvalidOperationException>(() => metricFamily.RemoveLabelled("value1"));
        }

        [Fact]
        public void RemoveThrowIfNoLabels_Tuple()
        {
            var metricFamily = CreateMetricFamily(ValueTuple.Create());
            Assert.Throws<InvalidOperationException>(() => metricFamily.RemoveLabelled(ValueTuple.Create()));
        }

        [Fact]
        public void RemoveThrowsOnLabelsMismatch_Array()
        {
            var metricFamily = CreateMetricFamily("label1", "label2");
            metricFamily.WithLabels("a", "b");

            Assert.Throws<ArgumentException>(() => metricFamily.RemoveLabelled("a"));
        }

        [Fact]
        public void ShouldEnumerateLabeledEmpty_Strings()
        {
            var metricFamily = CreateMetricFamily("label1", "label2");
            Assert.False(metricFamily.Labelled.Any());
        }

        [Fact]
        public void ShouldEnumerateLabeledEmpty_Tuple()
        {
            var metricFamily = CreateMetricFamily(("label1", "label2"));
            Assert.False(metricFamily.Labelled.Any());
        }

        [Fact]
        public void ShouldEnumerateLabeled_Strings()
        {
            var metricFamily = CreateMetricFamily("label1", "label2");
            var labeled = new[]
            {
                new[] { "value1", "value1" },
                new[] { "value2", "value2" },
                new[] { "value3", "value3" },
            };

            foreach (var item in labeled)
                metricFamily.WithLabels(item);

            var items = metricFamily.Labelled.Select(l => l.Key).ToArray();

            Assert.True(labeled.All(l => items.Any(i => l.SequenceEqual(i))));
        }

        [Fact]
        public void ShouldEnumerateLabeled_Tuple()
        {
            var metricFamily = CreateMetricFamily(("label1", "label2"));
            var labeled = new[]
            {
                ("value1", "value1"),
                ("value2", "value2"),
                ("value3", "value3"),
            };

            foreach (var item in labeled)
                metricFamily.WithLabels(item);

            var items = metricFamily.Labelled.Select(l => l.Key).ToArray();

            Assert.True(labeled.All(items.Contains));
        }

        [Fact]
        public void ShoulNotAllowIntLabels1()
        {
            Assert.Throws<NotSupportedException>(() => CreateMetricFamily(ValueTuple.Create(1)));
        }

        [Fact]
        public void ShoulNotAllowIntLabels2()
        {
            Assert.Throws<NotSupportedException>(() => CreateMetricFamily(("1", 2)));
        }

        [Fact]
        public void ShoulNotAllowIntLabels8()
        {
            Assert.Throws<NotSupportedException>(() => CreateMetricFamily(("1", "2", "3", "4", "5", "6", "7", 8)));
        }

        private IMetricFamily<IDummyMetric> CreateMetricFamily()
        {
            var config = new MetricConfiguration("dummy_metric", string.Empty, new string[0], false);
            return new MetricFamily<IDummyMetric, DummyMetric, ValueTuple, MetricConfiguration>(
                config, MetricType.Untyped,
                (configuration, list) => new DummyMetric(configuration, list, null));
        }

        private IMetricFamily<IDummyMetric> CreateMetricFamily(string label1, string label2)
        {
            var config = new MetricConfiguration("dummy_metric", string.Empty, new[] {label1, label2}, false);
            return new MetricFamily<IDummyMetric, DummyMetric, (string, string), MetricConfiguration>(
                config, MetricType.Untyped,
                (configuration, list) => new DummyMetric(configuration, list, null));
        }

        private MetricFamily<IDummyMetric, DummyMetric, TLabels, MetricConfiguration> CreateMetricFamily<TLabels>(TLabels labels)
            where TLabels : struct, ITuple, IEquatable<TLabels>
        {
            var config = new MetricConfiguration("dummy_metric", string.Empty, LabelsHelper.ToArray(labels), false);
            return new MetricFamily<IDummyMetric, DummyMetric, TLabels, MetricConfiguration>(
                config, MetricType.Untyped,
                (configuration, list) => new DummyMetric(configuration, list, null));
        }
    }
}
