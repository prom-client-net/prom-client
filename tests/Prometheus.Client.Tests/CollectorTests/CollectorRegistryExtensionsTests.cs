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
        public void UseDotNetStatsWithPrefix_TryGet()
        {
            var collectorRegistry = new CollectorRegistry();
            collectorRegistry.UseDotNetStats("prefix");

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
        public void UseProcessStatsWithPrefix_TryGet()
        {
            var collectorRegistry = new CollectorRegistry();
            collectorRegistry.UseProcessStats("prefix");

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

        [Fact]
        public void UseDefaultCollectorsWithPrefix_TryGet()
        {
            var collectorRegistry = new CollectorRegistry();
            collectorRegistry.UseDefaultCollectors("prefix");

            Assert.True(collectorRegistry.TryGet(nameof(GCCollectionCountCollector), out _));
            Assert.True(collectorRegistry.TryGet(nameof(GCTotalMemoryCollector), out _));
            Assert.True(collectorRegistry.TryGet(nameof(ProcessCollector), out _));
        }
    }
}
