namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Gauge metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#gauge
    ///     </remarks>
    /// </summary>
    public interface IGauge : IMetric<double>
    {
        void Inc();

        void Inc(double increment);

        void Inc(double increment, long? timestamp);

        void Set(double val);

        void Set(double val, long? timestamp);

        void Dec();

        void Dec(double decrement);

        void Dec(double decrement, long? timestamp);
    }
}
