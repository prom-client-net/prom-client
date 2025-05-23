using System.IO;
using System.Linq;
using System.Text;
using Prometheus.Client.Collectors.DotNetStats;
using Prometheus.Client.MetricsWriter;
using Xunit;

namespace Prometheus.Client.Tests.CollectorTests;

public class GCTotalMemoryCollectorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("promitor_")]
    [InlineData("myprefix_")]
    public void Check_MetricNames(string prefixName)
    {
        var collector = new GCTotalMemoryCollector(prefixName);

        Assert.Equal(prefixName + "dotnet_total_memory_bytes", collector.MetricNames.First());
    }

    [Fact]
    public void Check_Collect_NoPrefix()
    {
        using var stream = new MemoryStream();
        var metricWriter = new MetricsTextWriter(stream);
        var collector = new GCTotalMemoryCollector();
        collector.Collect(metricWriter);
        metricWriter.FlushAsync();

        var response = Encoding.UTF8.GetString(stream.ToArray());

        Assert.Contains("# TYPE dotnet_total_memory_bytes gauge", response);
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
        var collector = new GCTotalMemoryCollector(prefixName);
        collector.Collect(metricWriter);
        metricWriter.FlushAsync();

        var response = Encoding.UTF8.GetString(stream.ToArray());

        Assert.Contains($"# TYPE {prefixName}dotnet_total_memory_bytes gauge", response);
    }
}
