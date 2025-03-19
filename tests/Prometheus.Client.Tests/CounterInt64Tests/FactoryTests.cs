using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.CounterInt64Tests;

public class FactoryTests
{
    [Fact]
    public void ThrowOnNameConflict_Strings()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        factory.CreateCounterInt64("test_counter", string.Empty, "label1", "label2");

        Assert.Throws<InvalidOperationException>(() => factory.CreateCounterInt64("test_counter", string.Empty, Array.Empty<string>()));
        Assert.Throws<InvalidOperationException>(() => factory.CreateCounterInt64("test_counter", string.Empty, "label1", "testlabel"));
        Assert.Throws<InvalidOperationException>(() => factory.CreateCounterInt64("test_counter", string.Empty, new[] { "label1" }));
        Assert.Throws<InvalidOperationException>(() => factory.CreateCounterInt64("test_counter", string.Empty, "label1", "label2", "label3"));
        Assert.Throws<InvalidOperationException>(() => factory.CreateCounterInt64("test_counter", string.Empty, false, "label1", "label2", "label3"));
    }

    [Fact]
    public void ThrowOnNameConflict_Tuple()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        factory.CreateCounterInt64("test_counter", string.Empty, ("label1", "label2"));

        Assert.Throws<InvalidOperationException>(() => factory.CreateCounterInt64("test_counter", string.Empty, ValueTuple.Create()));
        Assert.Throws<InvalidOperationException>(() => factory.CreateCounterInt64("test_counter", string.Empty, ValueTuple.Create("label1")));
        Assert.Throws<InvalidOperationException>(() => factory.CreateCounterInt64("test_counter", string.Empty, ("label1", "testlabel")));
        Assert.Throws<InvalidOperationException>(() => factory.CreateCounterInt64("test_counter", string.Empty, ("label1", "label2", "label3")));
    }

    [Fact]
    public void ThrowOnNameConflict_StringAndTuple()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        factory.CreateCounterInt64("test_counter", string.Empty, ("label1", "label2"));

        Assert.Throws<InvalidOperationException>(() => factory.CreateCounterInt64("test_counter", string.Empty, ("label1", "testlabel")));
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Strings()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        var counter1 = factory.CreateCounterInt64("test_counter", string.Empty, "label1", "label2");
        var counter2 = factory.CreateCounterInt64("test_counter", string.Empty, "label1", "label2");

        Assert.Equal(counter1, counter2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Tuples()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        var counter1 = factory.CreateCounterInt64("test_counter", string.Empty, ("label1", "label2"));
        var counter2 = factory.CreateCounterInt64("test_counter", string.Empty, ("label1", "label2"));

        Assert.Equal(counter1, counter2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_StringAndTuple()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        var counter1 = factory.CreateCounterInt64("test_counter", string.Empty, "label1", "label2");
        counter1.Unlabelled.Inc();
        var counter2 = factory.CreateCounterInt64("test_counter", string.Empty, ("label1", "label2"));
        counter2.Unlabelled.Inc();

        // Cannot compare metrics families, because of different contracts, should check if sample the same
        Assert.Equal(counter1.Unlabelled, counter2.Unlabelled);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_Empty()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        var counter1 = factory.CreateCounterInt64("test_counter", string.Empty);
        var counter2 = factory.CreateCounterInt64("test_counter", string.Empty);

        Assert.Equal(counter1, counter2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyStrings()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        var counter1 = factory.CreateCounterInt64("test_counter", string.Empty, Array.Empty<string>());
        var counter2 = factory.CreateCounterInt64("test_counter", string.Empty, Array.Empty<string>());

        Assert.Equal(counter1, counter2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyTuples()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        var counter1 = factory.CreateCounterInt64("test_counter", string.Empty, ValueTuple.Create());
        var counter2 = factory.CreateCounterInt64("test_counter", string.Empty, ValueTuple.Create());

        Assert.Equal(counter1, counter2);
    }

    [Fact]
    public void SameLabelsProducesSameMetric_EmptyStringAndTuple()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        var counter1 = factory.CreateCounterInt64("test_counter", string.Empty, Array.Empty<string>());
        var counter2 = factory.CreateCounterInt64("test_counter", string.Empty, ValueTuple.Create());

        // Cannot compare metrics families, because of different contracts, should check if sample the same
        Assert.Equal(counter1.Unlabelled, counter2.Unlabelled);
    }

    [Fact]
    public void SingleLabel_ConvertToTuple()
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        var gauge = factory.CreateCounterInt64("metricname", "help", "label");
        Assert.Equal(typeof(ValueTuple<string>), gauge.LabelNames.GetType());
    }
}
