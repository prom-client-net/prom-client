namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Counter metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#counter
    ///     </remarks>
    /// </summary>
    public interface ICounter<T>
    {
        T Value { get; }

        void Inc();

        void Inc(T increment);
    }

    public interface ICounter : ICounter<double>
    {
    }
}
