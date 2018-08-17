namespace Prometheus.Client.Contracts
{
    public enum CMetricType
    {
        Counter = 0,
        
        Gauge = 1,
        
        Summary = 2,
       
        Untyped = 3,
       
        Histogram = 4
    }
}