using System.Collections.Generic;
using Prometheus.Advanced;
using Prometheus.Client.Advanced;

namespace Prometheus.Client.Owin
{
    public class PrometheusOptions
    {
        public string MapPath { get; set; } = "metrics";

        public ICollectorRegistry CollectorRegistry { get; set; } = PrometheusCollectorRegistry.Instance;

        public List<IOnDemandCollector> Collectors { get; set; } = new List<IOnDemandCollector>();

        public CollectorLocator CollectorLocator { get; set; } = new CollectorLocator();
    }
}