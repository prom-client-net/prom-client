using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.UntypedTests
{
    public class CollectionTests : MetricTestBase
    {
        private const string _resourcesNamespace = "Prometheus.Client.Tests.UntypedTests.Resources";

        [Fact]
        public Task EmptyCollection()
        {
            return TestCollectionAsync(factory => {
                factory.CreateUntyped("test", "with help text", false, false, "category");
            }, $"{_resourcesNamespace}.UntypedTests_Empty.txt");
        }

        [Fact]
        public Task Collection()
        {
            return TestCollectionAsync(factory => {
                var untyped = factory.CreateUntyped("test", "with help text", "category");
                untyped.Set(1);
                untyped.WithLabels("some").Set(double.NaN);

                var untyped2 = factory.CreateUntyped("nextuntyped", "with help text", "group", "type");
                untyped2.Set(10);
                untyped2.WithLabels("any", "2").Set(5);
            }, $"{_resourcesNamespace}.UntypedTests_Collection.txt");
        }
    }
}
