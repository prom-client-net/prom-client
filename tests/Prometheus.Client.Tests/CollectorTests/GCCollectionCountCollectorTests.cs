using System.IO;
using System.Linq;
using System.Text;
using Prometheus.Client.Collectors.DotNetStats;
using Prometheus.Client.MetricsWriter;
using Xunit;

namespace Prometheus.Client.Tests.CollectorTests
{
    public class GCCollectionCountCollectorTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("promitor_")]
        [InlineData("myprefix_")]
        public void Check_MetricNames(string prefixName)
        {
            var collector = new GCCollectionCountCollector(prefixName);

            Assert.Equal(prefixName + "dotnet_collection_count_total", collector.MetricNames.First());
        }

        [Fact]
        public void Check_Collect_NoPrefix()
        {
            using var stream = new MemoryStream();
            var metricWriter = new MetricsTextWriter(stream);
            var collector = new GCCollectionCountCollector();
            collector.Collect(metricWriter);
            metricWriter.FlushAsync();

            var response = Encoding.UTF8.GetString(stream.ToArray());

            Assert.Contains("# TYPE dotnet_collection_count_total counter", response);
        }

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("promitor_")]
        [InlineData("myprefix_")]
        public void Check_Collect(string prefixName)
        {
            using var stream = new MemoryStream();
            var metricWriter = new MetricsTextWriter(stream);
            var collector = new GCCollectionCountCollector(prefixName);
            collector.Collect(metricWriter);
            metricWriter.FlushAsync();

            var response = Encoding.UTF8.GetString(stream.ToArray());

            Assert.Contains($"# TYPE {prefixName}dotnet_collection_count_total counter", response);
        }
    }
}
