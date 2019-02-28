using System;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.DotNetStats
{
    public class GCTotalMemoryCollector : ICollector
    {
        private static readonly string _help = "Total known allocated memory";
        private static readonly string[] _labels = new string[0];

        public string Name => "dotnet_totalmemory";

        public string[] LabelNames => _labels;

        public void Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(Name, MetricType.Gauge, _help);
            writer.WriteSample(GC.GetTotalMemory(false));
        }
    }
}
