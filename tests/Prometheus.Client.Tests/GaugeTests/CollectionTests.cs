using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.GaugeTests
{
    public class CollectionTests
    {
        private const string _resourcesNamespace = "Prometheus.Client.Tests.GaugeTests.Resources";

        [Fact]
        public Task EmptyCollection()
        {
            return CollectionTestHelper.TestCollectionAsync(factory => {
                factory.CreateGauge("test", "with help text", false, false, "category");
            }, $"{_resourcesNamespace}.GaugeTests_Empty.txt");
        }

        [Fact]
        public Task SuppressEmptySamples()
        {
            return CollectionTestHelper.TestCollectionAsync(factory => {
                var gauge = factory.CreateGauge("test", "with help text", "category");
                gauge.WithLabels("some").Inc(5.5);
            }, $"{_resourcesNamespace}.GaugeTests_SuppressEmpty.txt");
        }

        [Fact]
        public Task Collection()
        {
            return CollectionTestHelper.TestCollectionAsync(factory => {
                var gauge = factory.CreateGauge("test", "with help text", "category");
                gauge.Inc();
                gauge.WithLabels("some").Inc(5.5);

                var gauge2 = factory.CreateGauge("nextgauge", "with help text", "group", "type");
                gauge2.Inc();
                gauge2.WithLabels("any", "2").Dec(5.2);

                var nanGauge = factory.CreateGauge("nangauge", "example of NaN");
                nanGauge.Set(double.NaN);
            }, $"{_resourcesNamespace}.GaugeTests_Collection.txt");
        }
    }
}
