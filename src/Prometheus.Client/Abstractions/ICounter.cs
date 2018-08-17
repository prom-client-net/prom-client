namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Counter metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#counter
    ///     </remarks>
    /// </summary>
    public interface ICounter
    {
        void Inc();
        
        void Inc(double increment);
        
        double Value { get; }
    }
}