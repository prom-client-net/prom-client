namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Histogram metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#histogram
    ///     </remarks>
    /// </summary>
    public interface IHistogram
    {
        void Observe(double val);
    }
}