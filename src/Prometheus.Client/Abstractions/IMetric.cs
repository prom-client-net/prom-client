namespace Prometheus.Client.Abstractions
{
    public interface IMetric<out TState> : IMetric
        where TState: struct
    {
        TState Value { get; }

        void Reset();
    }

    public interface IMetric
    {}
}
