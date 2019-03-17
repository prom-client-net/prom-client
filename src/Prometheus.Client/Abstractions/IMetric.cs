namespace Prometheus.Client.Abstractions
{
    public interface IMetric<out TState>
        where TState: struct
    {
        TState Value { get; }
    }
}
