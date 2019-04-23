namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Untyped metric type
    /// </summary>
    public interface IUntyped
    {
        double Value { get; }

        void Set(double val);

        void Set(double val, long? timestamp);
    }
}
