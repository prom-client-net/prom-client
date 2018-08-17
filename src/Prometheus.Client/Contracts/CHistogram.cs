namespace Prometheus.Client.Contracts
{
    public class CHistogram 
    {
        public ulong SampleCount { get; set; }
       
        public double SampleSum { get; set; }
       
        public CBucket[] Buckets { get; set; } 
    }
}