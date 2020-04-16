using System;
using System.Collections.Generic;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client.Collectors.DotNetStats
{
    public class GCTotalMemoryCollector : ICollector
    {
        private const string _help = "Total known allocated memory";
        private const string _name = "dotnet_totalmemory";

        public GCTotalMemoryCollector()
        {
            Configuration = new CollectorConfiguration(nameof(GCTotalMemoryCollector));
            MetricNames = new[] { _name };
        }

        public CollectorConfiguration Configuration { get; }

        public IReadOnlyList<string> MetricNames { get; }

        public void Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(_name, MetricType.Gauge, _help);
            writer.WriteSample(GC.GetTotalMemory(false));
            writer.EndMetric();
        }
    }
}
