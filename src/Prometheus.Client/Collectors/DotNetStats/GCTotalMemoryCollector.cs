using System;
using System.Collections.Generic;
using Prometheus.Client.MetricsWriter;

#pragma warning disable CS0618

namespace Prometheus.Client.Collectors.DotNetStats;

public class GCTotalMemoryCollector : ICollector
{
    private const string _help = "Total known allocated memory in bytes";
    private readonly string _name;
    private readonly string _legacyName;
    private readonly bool _addLegacyMetrics;

    public CollectorConfiguration Configuration { get; }
    public IReadOnlyList<string> MetricNames { get; }

    public GCTotalMemoryCollector()
        : this(string.Empty)
    {
    }

    public GCTotalMemoryCollector(string prefixName)
        : this(prefixName, false)
    {
    }

    [Obsolete("'addLegacyMetrics' will be removed in future versions")]
    public GCTotalMemoryCollector(bool addLegacyMetrics)
        : this(string.Empty, addLegacyMetrics)
    {
    }

    [Obsolete("'addLegacyMetrics' will be removed in future versions")]
    public GCTotalMemoryCollector(string prefixName, bool addLegacyMetrics)
    {
        _legacyName = prefixName + "dotnet_totalmemory";
        _name = prefixName + "dotnet_total_memory_bytes";

        _addLegacyMetrics = addLegacyMetrics;

        Configuration = new CollectorConfiguration(nameof(GCTotalMemoryCollector));
        MetricNames = _addLegacyMetrics ? new[] { _legacyName, _name } : new[] { _name };
    }

    public void Collect(IMetricsWriter writer)
    {
        writer.WriteMetricHeader(_name, MetricType.Gauge, _help);
        writer.WriteSample(GC.GetTotalMemory(false));
        writer.EndMetric();

        if (_addLegacyMetrics)
        {
            writer.WriteMetricHeader(_legacyName, MetricType.Gauge, _help);
            writer.WriteSample(GC.GetTotalMemory(false));
            writer.EndMetric();
        }
    }
}
