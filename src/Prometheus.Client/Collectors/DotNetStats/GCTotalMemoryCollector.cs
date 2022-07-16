using System;
using System.Collections.Generic;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.DotNetStats
{
    public class GCTotalMemoryCollector : ICollector
    {
        private const string _help = "Total known allocated memory in bytes";
        private readonly string _name;
        private readonly string _legacyName;
        private readonly bool _addLegacyMetricNames;

        public GCTotalMemoryCollector(bool addLegacyMetricNames = false)
            : this(string.Empty, addLegacyMetricNames)
        {
        }

        public GCTotalMemoryCollector(string prefixName, bool addLegacyMetricNames = false)
        {
            _legacyName = prefixName + "dotnet_totalmemory";
            _name = prefixName + "dotnet_total_memory_bytes";

            _addLegacyMetricNames = addLegacyMetricNames;

            Configuration = new CollectorConfiguration(nameof(GCTotalMemoryCollector));
            MetricNames = _addLegacyMetricNames ? new[] { _legacyName, _name } : new[] { _name };
        }

        public CollectorConfiguration Configuration { get; }
        public IReadOnlyList<string> MetricNames { get; }

        public void Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(_name, MetricType.Gauge, _help);
            writer.WriteSample(GC.GetTotalMemory(false));
            writer.EndMetric();

            if (_addLegacyMetricNames)
            {
                writer.WriteMetricHeader(_legacyName, MetricType.Gauge, _help);
                writer.WriteSample(GC.GetTotalMemory(false));
                writer.EndMetric();
            }
        }
    }
}
