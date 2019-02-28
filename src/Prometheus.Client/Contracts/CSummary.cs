namespace Prometheus.Client.Contracts
{
    public class CSummary
    {
        public ulong SampleCount { get; set; }

        public double SampleSum { get; set; }

        public CQuantile[] Quantiles { get; set; }
    }
}
