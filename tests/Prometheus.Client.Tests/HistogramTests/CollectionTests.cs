using System.Threading.Tasks;
using Xunit;
using Prometheus.Client;

namespace Prometheus.Client.Tests.HistogramTests
{
    public class CollectionTests : MetricTestBase
    {
        private const string _resourcesNamespace = "Prometheus.Client.Tests.HistogramTests.Resources";

        [Fact]
        public Task EmptyCollection()
        {
            return TestCollectionAsync(factory => {
                factory.CreateHistogram("hist1", "help", false, false, new[] { 1.0, 2.0, 3.0 });
            }, $"{_resourcesNamespace}.HistogramTests_Empty.txt");
        }

        [Fact]
        public Task SuppressEmptySamples()
        {
            return TestCollectionAsync(factory => {
                factory.CreateHistogram("hist1", "help", new[] { -5.0, 0, 5.0, 10 }, "type");
            }, $"{_resourcesNamespace}.HistogramTests_SuppressEmpty.txt");
        }

        [Fact]
        public Task Collection()
        {
            return TestCollectionAsync(factory => {
                var histogram = factory.CreateHistogram("hist1", "help", new[] { 1.0, 2.0, 3.0 });
                histogram.Observe(1.5);
                histogram.Observe(2.5);
                histogram.Observe(1);
                histogram.Observe(2.4);
                histogram.Observe(2.1);
                histogram.Observe(0.4);
                histogram.Observe(1.4);
                histogram.Observe(1.5);
                histogram.Observe(3.9);

                var histogram2 = factory.CreateHistogram("hist2", "help2", new[] { -5.0, 0, 5.0, 10 });
                histogram2.Observe(-20);
                histogram2.Observe(-1);
                histogram2.Observe(0);
                histogram2.Observe(2.5);
                histogram2.Observe(5);
                histogram2.Observe(9);
                histogram2.Observe(11);
            }, $"{_resourcesNamespace}.HistogramTests_Collection.txt");
        }
    }
}
