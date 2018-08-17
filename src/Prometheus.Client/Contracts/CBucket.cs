namespace Prometheus.Client.Contracts
{
    public class CBucket
    {
        public ulong CumulativeCount { get; set; }

        public double UpperBound { get; set; }
    }
}