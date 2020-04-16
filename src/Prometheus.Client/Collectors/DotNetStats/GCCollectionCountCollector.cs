using System;
using System.Collections.Generic;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client.Collectors.DotNetStats
{
    public class GCCollectionCountCollector : ICollector
    {
        private const string _help = "GC collection count";
        private const string _name = "dotnet_collection_count_total";
        private readonly string[] _genNames;

        public GCCollectionCountCollector()
        {
            Configuration = new CollectorConfiguration(nameof(GCCollectionCountCollector));
            MetricNames = new[] { _name };
            _genNames = new string[GC.MaxGeneration + 1];
            for (var gen = 0; gen <= GC.MaxGeneration; gen++)
            {
                _genNames[gen] = gen.ToString();
            }
        }

        public CollectorConfiguration Configuration { get; }

        public IReadOnlyList<string> MetricNames { get; }

        public void Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(_name, MetricType.Counter, _help);

            for (var gen = 0; gen <= GC.MaxGeneration; gen++)
            {
                writer.StartSample()
                    .StartLabels()
                    .WriteLabel("generation", _genNames[gen])
                    .EndLabels()
                    .WriteValue(GC.CollectionCount(gen))
                    .EndSample();
            }

            writer.EndMetric();
        }
    }
}
