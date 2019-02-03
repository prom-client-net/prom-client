namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Untyped metric type
    /// </summary>
    public interface IUntyped
    {
        double Value { get; }
    }
}
