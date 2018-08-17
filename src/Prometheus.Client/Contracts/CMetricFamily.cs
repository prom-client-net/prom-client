namespace Prometheus.Client.Contracts
{
    public class CMetricFamily
    {
        public string Name { get; set; }

        public string Help { get; set; }

        public CMetricType Type { get; set; }

        public CMetric[] Metrics { get; set; }
    }
}