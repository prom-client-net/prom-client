using System.Collections.Generic;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client.Tests.Mocks
{
    public class DummyCollector : ICollector
    {
        public DummyCollector(string collectorName, params string[] metricNames)
        {
            Configuration = new CollectorConfiguration(collectorName);
            MetricNames = metricNames;
        }

        public CollectorConfiguration Configuration { get; }
        public IReadOnlyList<string> MetricNames { get; }
        public void Collect(IMetricsWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
