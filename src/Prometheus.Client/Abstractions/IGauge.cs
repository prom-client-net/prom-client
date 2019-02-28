namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Gauge metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#gauge
    ///     </remarks>
    /// </summary>
    public interface IGauge
    {
        double Value { get; }

        void Inc();

        void Inc(double increment);

        void Set(double val);

        void Dec();

        void Dec(double decrement);
    }
}
