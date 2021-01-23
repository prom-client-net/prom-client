namespace Prometheus.Client
{
    /// <summary>
    ///     Histogram metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#histogram
    ///     </remarks>
    /// </summary>
    public interface IHistogram : IMetric<HistogramState>, IValueObserver
    {
    }
}
