using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.DotNetStats;
using Prometheus.Client.Collectors.ProcessStats;
using Xunit;

namespace Prometheus.Client.Tests.CollectorTests
{
    public class CollectorRegistryExtensionsTests
    {
        [Fact]
        public void UseDotNetStats_TryGet()
        {
            var collectorRegistry = new CollectorRegistry();
            collectorRegistry.UseDotNetStats();

            Assert.True(collectorRegistry.TryGet(nameof(GCCollectionCountCollector), out _));
            Assert.True(collectorRegistry.TryGet(nameof(GCTotalMemoryCollector), out _));
        }
        
        [Fact]
        public void UseProcessStats_TryGet()
        {
            var collectorRegistry = new CollectorRegistry();
            collectorRegistry.UseProcessStats();

            Assert.True(collectorRegistry.TryGet(nameof(ProcessCollector), out _));
        }
        
        [Fact]
        public void UseDefaultCollectors_TryGet()
        {
            var collectorRegistry = new CollectorRegistry();
            collectorRegistry.UseDefaultCollectors();

            Assert.True(collectorRegistry.TryGet(nameof(GCCollectionCountCollector), out _));
            Assert.True(collectorRegistry.TryGet(nameof(GCTotalMemoryCollector), out _));
            Assert.True(collectorRegistry.TryGet(nameof(ProcessCollector), out _));
        }
        
        //TODO: add more checks
    }
}
