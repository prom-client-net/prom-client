using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.GaugeInt64Tests
{
    public class CollectionTests
    {
        private const string _resourcesNamespace = "Prometheus.Client.Tests.GaugeInt64Tests.Resources";

        [Fact]
        public Task EmptyCollection()
        {
            return CollectionTestHelper.TestCollectionAsync(factory => {
                factory.CreateGaugeInt64("test", "with help text", false, false, "category");
            }, $"{_resourcesNamespace}.GaugeTests_Empty.txt");
        }

        [Fact]
        public Task SuppressEmptySamples()
        {
            return CollectionTestHelper.TestCollectionAsync(factory => {
                var gauge = factory.CreateGaugeInt64("test", "with help text", "category");
                gauge.WithLabels("some").Inc(5);
            }, $"{_resourcesNamespace}.GaugeTests_SuppressEmpty.txt");
        }

        [Fact]
        public Task Collection()
        {
            return CollectionTestHelper.TestCollectionAsync(factory => {
                var gauge = factory.CreateGaugeInt64("test", "with help text", "category");
                gauge.Inc();
                gauge.WithLabels("some").Inc(5);

                var gauge2 = factory.CreateGaugeInt64("nextgauge", "with help text", "group", "type");
                gauge2.Inc();
                gauge2.WithLabels("any", "2").Dec(5);
            }, $"{_resourcesNamespace}.GaugeTests_Collection.txt");
        }
    }
}
