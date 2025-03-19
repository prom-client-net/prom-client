using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.CounterTests;

public class CollectionTests
{
    private const string _resourcesNamespace = "Prometheus.Client.Tests.CounterTests.Resources";

    [Fact]
    public Task EmptyCollection()
    {
        return CollectionTestHelper.TestCollectionAsync(factory => {
            factory.CreateCounter("test", "with help text");
        }, $"{_resourcesNamespace}.CounterTests_Empty.txt");
    }

    [Fact]
    public Task SuppressEmptySamples()
    {
        return CollectionTestHelper.TestCollectionAsync(factory => {
            var counter = factory.CreateCounter("test", "with help text", "category");
            counter.WithLabels("some").Inc(5.5);
        }, $"{_resourcesNamespace}.CounterTests_SuppressEmpty.txt");
    }

    [Fact]
    public Task Collection()
    {
        return CollectionTestHelper.TestCollectionAsync(factory => {
            var counter = factory.CreateCounter("test", "with help text", "category");
            counter.Unlabelled.Inc();
            counter.WithLabels("some").Inc(2.1);

            var counter2 = factory.CreateCounter("nextcounter", "with help text", ("group", "type"));
            counter2.Unlabelled.Inc(10.1);
            counter2.WithLabels(("any", "2")).Inc(5.2);
        }, $"{_resourcesNamespace}.CounterTests_Collection.txt");
    }

    [Fact]
    public Task RemoveExpiredSerieDueToTtl()
    {
        return CollectionTestHelper.TestCollectionAsync(factory => {
                var counterWithoutTtl = factory.CreateCounter("test", "with help text", "category");
                counterWithoutTtl.Unlabelled.Inc();
                counterWithoutTtl.WithLabels("some").Inc(2.1);

                var counterWithTtl = factory.CreateCounter("nextcounter", "with help text", ("group", "type"), timeToLive: TimeSpan.FromSeconds(1));
                counterWithTtl.WithLabels(("old", "serie")).Inc(8.7);

                Thread.Sleep(1100);

                counterWithTtl.Unlabelled.Inc(10.1);
                counterWithTtl.WithLabels(("any", "2")).Inc(5.2);

            }, $"{_resourcesNamespace}.CounterTests_Collection.txt");
    }
}
