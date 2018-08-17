namespace Prometheus.Client.Contracts
{
    public class CMetric
    {
        public CLabelPair[] Labels { get; set; }

        public CGauge CGauge { get; set; }

        public CCounter CCounter { get; set; }

        public CSummary CSummary { get; set; }

        public CUntyped CUntyped { get; set; }

        public CHistogram CHistogram { get; set; }

        public long TimestampMs { get; set; }
    }
}