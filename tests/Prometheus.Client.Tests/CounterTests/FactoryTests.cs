using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.CounterTests;

public class FactoryTests
{
    private readonly IMetricFactory _factory;

    public FactoryTests()
    {
        _factory = new MetricFactory(new CollectorRegistry());
    }

    [Theory]
    [InlineData]
    [InlineData("label1")]
    [InlineData("label1", "testlabel")]
    [InlineData("label1", "label2", "label3")]
    public void ThrowOnNameConflict_Strings(params string[] labels)
    {
        const string name = "test_counter";
        var expectedLabels = new[] { "label1", "label2" };
        _factory.CreateCounter(name, string.Empty, expectedLabels);

        var ex = Assert.Throws<InvalidOperationException>(() => _factory.CreateCounter("test_counter", string.Empty, labels));
        Assert.Equal($"Metric name ({name}). Expected labels ({string.Join(", ", expectedLabels)}), but actual labels ({string.Join(", ", labels)})", ex.Message);
    }

    [Fact]
    public void ThrowOnTypeConflict_Strings()
    {
        const string name = "test_counter";
        var labels = new[] { "label1", "label2" };
        _factory.CreateCounter(name, string.Empty, labels);

        var ex = Assert.Throws<InvalidOperationException>(() => _factory.CreateGauge("test_counter", string.Empty, labels));
        Assert.Equal($"Metric name ({name}). Must have same Type. Expected labels ({string.Join(", ", labels)})", ex.Message);
    }

    [Fact]
    public void ThrowOnNameConflict_Tuple0()
    {
        const string name = "test_counter";
        var expectedLabels = ("label1", "label2");
        _factory.CreateCounter(name, string.Empty, expectedLabels);

        var labels = ValueTuple.Create();
        var ex = Assert.Throws<InvalidOperationException>(() => _factory.CreateCounter("test_counter", string.Empty, labels));

        Assert.Equal($"Metric name ({name}). Must have same Type. Expected labels {expectedLabels}", ex.Message);
    }

    [Fact]
    public void ThrowOnNameConflict_Tuple1()
    {
        const string name = "test_counter";
        var expectedLabels = ("label1", "label2");
        _factory.CreateCounter(name, string.Empty, expectedLabels);

        var labels = ValueTuple.Create("label1");
        var ex = Assert.Throws<InvalidOperationException>(() => _factory.CreateCounter("test_counter", string.Empty, labels));

        Assert.Equal($"Metric name ({name}). Must have same Type. Expected labels {expectedLabels}", ex.Message);
    }

    [Fact]
    public void ThrowOnNameConflict_Tuple2()
    {
        const string name = "test_counter";
        var expectedLabels = ("label1", "label2");
        _factory.CreateCounter(name, string.Empty, expectedLabels);

        var labels = ("label1", "testlabel");
        var ex = Assert.Throws<InvalidOperationException>(() => _factory.CreateCounter("test_counter", string.Empty, labels));

        Assert.Equal($"Metric name ({name}). Expected labels {expectedLabels.ToString()}, but actual labels {labels.ToString()}", ex.Message);
    }

    [Fact]
    public void ThrowOnNameConflict_Tuple3()
    {
        const string name = "test_counter";
        var expectedLabels = ("label1", "label2", "label3");
        _factory.CreateCounter(name, string.Empty, expectedLabels);

        var labels = ValueTuple.Create("label1");
        var ex = Assert.Throws<InvalidOperationException>(() => _factory.CreateCounter("test_counter", string.Empty, labels));

        Assert.Equal($"Metric name ({name}). Must have same Type. Expected labels {expectedLabels}", ex.Message);
    }

    [Fact]
    public void ThrowOnNameConflict_StringAndTuple()
    {
        _factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));

        Assert.Throws<InvalidOperationException>(() => _factory.CreateCounter("test_counter", string.Empty, ("label1", "testlabel")));
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Strings()
    {
        var counter1 = _factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
        var counter2 = _factory.CreateCounter("test_counter", string.Empty, "label1", "label2");

        Assert.Equal(counter1, counter2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Tuples()
    {
        var counter1 = _factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
        var counter2 = _factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));

        Assert.Equal(counter1, counter2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_StringAndTuple()
    {
        var counter1 = _factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
        counter1.Unlabelled.Inc();
        var counter2 = _factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
        counter2.Unlabelled.Inc();

        // Cannot compare metrics families, because of different contracts, should check if sample the same
        Assert.Equal(counter1.Unlabelled, counter2.Unlabelled);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Empty()
    {
        var counter1 = _factory.CreateCounter("test_counter", string.Empty);
        var counter2 = _factory.CreateCounter("test_counter", string.Empty);

        Assert.Equal(counter1, counter2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyStrings()
    {
        var counter1 = _factory.CreateCounter("test_counter", string.Empty, Array.Empty<string>());
        var counter2 = _factory.CreateCounter("test_counter", string.Empty, Array.Empty<string>());

        Assert.Equal(counter1, counter2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyTuples()
    {
        var counter1 = _factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create());
        var counter2 = _factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create());

        Assert.Equal(counter1, counter2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyStringAndTuple()
    {
        var counter1 = _factory.CreateCounter("test_counter", string.Empty, Array.Empty<string>());
        var counter2 = _factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create());

        // Cannot compare metrics families, because of different contracts, should check if sample the same
        Assert.Equal(counter1.Unlabelled, counter2.Unlabelled);
    }

    [Fact]
    public void SingleLabel_ConvertToTuple()
    {
        var metric = _factory.CreateCounter("metricname", "help", "label");
        Assert.Equal(typeof(ValueTuple<string>), metric.LabelNames.GetType());
    }
}
