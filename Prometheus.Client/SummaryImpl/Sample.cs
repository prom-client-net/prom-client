namespace Prometheus.Client.SummaryImpl
{
    // Sample holds an observed value and meta information for compression. 
    public struct Sample
    {
        public double Value;
        public double Width;
        public double Delta;
    }
}