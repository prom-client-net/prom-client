namespace Prometheus.Client;

/// <summary>
///     Counter metric type
///     <remarks>
///         https://prometheus.io/docs/concepts/metric_types/#counter
///     </remarks>
/// </summary>
public interface ICounter<T> : IMetric<T>
    where T : struct
{
    void Inc();

    void Inc(T increment);

    void Inc(T increment, long? timestamp);

    void IncTo(T value);

    void IncTo(T value, long? timestamp);
}

public interface ICounter : ICounter<double>
{
}
