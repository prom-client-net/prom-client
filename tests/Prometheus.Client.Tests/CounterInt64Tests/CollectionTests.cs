using System;
using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.CounterInt64Tests;

public class CollectionTests
{
    private const string _resourcesNamespace = "Prometheus.Client.Tests.CounterInt64Tests.Resources";

    [Fact]
    public Task EmptyCollection()
    {
        return CollectionTestHelper.TestCollectionAsync(factory => {
            factory.CreateCounterInt64("test", "with help text");
        }, $"{_resourcesNamespace}.CounterTests_Empty.txt");
    }

    [Fact]
    public Task SuppressEmptySamples()
    {
        return CollectionTestHelper.TestCollectionAsync(factory => {
            var counter = factory.CreateCounterInt64("test", "with help text", "category");
            counter.WithLabels("some").Inc(5);
        }, $"{_resourcesNamespace}.CounterTests_SuppressEmpty.txt");
    }

    [Fact]
    public Task Collection()
    {
        return CollectionTestHelper.TestCollectionAsync(factory => {
            var counter = factory.CreateCounterInt64("test", "with help text", false, TimeSpan.Zero, "category");
            counter.Unlabelled.Inc();
            counter.WithLabels("some").Inc(2);

            var counter2 = factory.CreateCounterInt64("nextcounter", "with help text", ("group", "type"));
            counter2.Unlabelled.Inc(10);
            counter2.WithLabels(("any", "2")).Inc(5);
        }, $"{_resourcesNamespace}.CounterTests_Collection.txt");
    }
}
