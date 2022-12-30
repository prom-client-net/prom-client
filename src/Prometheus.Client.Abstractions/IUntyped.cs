namespace Prometheus.Client;

/// <summary>
///     Untyped metric type
/// </summary>
public interface IUntyped : IMetric<double>
{
    void Set(double val);

    void Set(double val, long? timestamp);
}
