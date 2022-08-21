using System;
using System.Collections.Generic;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.DotNetStats
{
    public class GCCollectionCountCollector : ICollector
    {
        private const string _help = "GC collection count";
        private readonly string _name;
        private readonly string[] _genNames;

        public CollectorConfiguration Configuration { get; }

        public IReadOnlyList<string> MetricNames { get; }

        public GCCollectionCountCollector()
            : this(string.Empty)
        {
        }

        public GCCollectionCountCollector(string prefixName)
        {
            _name = prefixName + "dotnet_collection_count_total";
            Configuration = new CollectorConfiguration(nameof(GCCollectionCountCollector));
            MetricNames = new[] { _name };
            _genNames = new string[GC.MaxGeneration + 1];
            for (var gen = 0; gen <= GC.MaxGeneration; gen++)
                _genNames[gen] = gen.ToString();
        }

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
