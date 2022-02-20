using System.Diagnostics;
using System.IO;
using System.Text;
using Prometheus.Client.Collectors.ProcessStats;
using Prometheus.Client.MetricsWriter;
using Xunit;

namespace Prometheus.Client.Tests.CollectorTests
{
    public class ProcessCollectorTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("promitor_")]
        [InlineData("myprefix_")]
        public void Check_MetricNames(string prefixName)
        {
            var collector = new ProcessCollector(Process.GetCurrentProcess(), prefixName);

            Assert.Contains(prefixName + "process_cpu_seconds_total", collector.MetricNames);
            Assert.Contains(prefixName + "process_virtual_memory_bytes", collector.MetricNames);
            Assert.Contains(prefixName + "process_working_set_bytes", collector.MetricNames);
            Assert.Contains(prefixName + "process_private_memory_bytes", collector.MetricNames);
            Assert.Contains(prefixName + "process_num_threads", collector.MetricNames);
            Assert.Contains(prefixName + "process_processid", collector.MetricNames);
            Assert.Contains(prefixName + "process_start_time_seconds", collector.MetricNames);
        }

        [Fact]
        public void Check_Collect_NoPrefix()
        {
            using var stream = new MemoryStream();
            var metricWriter = new MetricsTextWriter(stream);
            var collector = new ProcessCollector(Process.GetCurrentProcess());
            collector.Collect(metricWriter);
            metricWriter.FlushAsync();

            var response = Encoding.UTF8.GetString(stream.ToArray());

            Assert.Contains("# TYPE process_cpu_seconds_total counter", response);
            Assert.Contains("# TYPE process_virtual_memory_bytes gauge", response);
            Assert.Contains("# TYPE process_working_set_bytes gauge", response);
            Assert.Contains("# TYPE process_private_memory_bytes gauge", response);
            Assert.Contains("# TYPE process_num_threads gauge", response);
            Assert.Contains("# TYPE process_processid gauge", response);
            Assert.Contains("# TYPE process_start_time_seconds gauge", response);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("promitor_")]
        [InlineData("myprefix_")]
        public void Check_Collect(string prefixName)
        {
            using var stream = new MemoryStream();
            var metricWriter = new MetricsTextWriter(stream);
            var collector = new ProcessCollector(Process.GetCurrentProcess(), prefixName);
            collector.Collect(metricWriter);
            metricWriter.FlushAsync();

            var response = Encoding.UTF8.GetString(stream.ToArray());

            Assert.Contains($"# TYPE {prefixName}process_cpu_seconds_total counter", response);
            Assert.Contains($"# TYPE {prefixName}process_virtual_memory_bytes gauge", response);
            Assert.Contains($"# TYPE {prefixName}process_working_set_bytes gauge", response);
            Assert.Contains($"# TYPE {prefixName}process_private_memory_bytes gauge", response);
            Assert.Contains($"# TYPE {prefixName}process_num_threads gauge", response);
            Assert.Contains($"# TYPE {prefixName}process_processid gauge", response);
            Assert.Contains($"# TYPE {prefixName}process_start_time_seconds gauge", response);
        }
    }
}
