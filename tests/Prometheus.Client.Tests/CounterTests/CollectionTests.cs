using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.CounterTests
{
    public class CollectionTests : MetricTestBase
    {
        private const string _resourcesNamespace = "Prometheus.Client.Tests.CounterTests.Resources";

        [Fact]
        public Task EmptyCollection()
        {
            return TestCollectionAsync(factory => {
                var counter = factory.CreateCounter("test", "with help text", MetricFlags.None);
            }, $"{_resourcesNamespace}.CounterTests_Empty.txt");
        }

        [Fact]
        public Task SuppressEmptySamples()
        {
            return TestCollectionAsync(factory => {
                var counter = factory.CreateCounter("test", "with help text", "category");
                counter.WithLabels("some").Inc(5);
            }, $"{_resourcesNamespace}.CounterTests_SuppressEmpty.txt");
        }

        [Fact]
        public Task Collection()
        {
            return TestCollectionAsync(factory => {
                var counter = factory.CreateCounter("test", "with help text", MetricFlags.None, "category");
                counter.Unlabelled.Inc();
                counter.WithLabels("some").Inc(2);

                var counter2 = factory.CreateCounter("nextcounter", "with help text", ("group", "type"), MetricFlags.None);
                counter2.Unlabelled.Inc(10);
                counter2.WithLabels(("any", "2")).Inc(5);
            }, $"{_resourcesNamespace}.CounterTests_Collection.txt");
        }
    }
}
