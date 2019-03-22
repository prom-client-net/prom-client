using System;
using System.Collections.Generic;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.DotNetStats
{
    public class GCTotalMemoryCollector : ICollector
    {
        private const string _help = "Total known allocated memory";
        private const string _name = "dotnet_totalmemory";

        public GCTotalMemoryCollector()
        {
            MetricNames = new[] { _name };
        }

        public IReadOnlyList<string> MetricNames { get; }

        public void Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(_name, MetricType.Gauge, _help);
            writer.WriteSample(GC.GetTotalMemory(false));
            writer.EndMetric();
        }
    }
}
