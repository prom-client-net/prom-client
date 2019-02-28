namespace Prometheus.Client.SummaryImpl
{
    // Sample holds an observed value and meta information for compression. 
    public struct Sample
    {
        public double Value { get; set; }
        public double Width { get; set; }
        public double Delta { get; set; }
    }
}
