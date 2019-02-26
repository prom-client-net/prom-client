using System;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.DotNetStats
{
    public class GCTotalMemoryCollector : ICollector
    {
        private static string _help = "Total known allocated memory";

        public string Name => "dotnet_totalmemory";

        public string[] MetricNames => new[] { Name };

        public void Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(Name, MetricType.Gauge, _help);
            writer.WriteSample(GC.GetTotalMemory(false));
        }
    }
}
