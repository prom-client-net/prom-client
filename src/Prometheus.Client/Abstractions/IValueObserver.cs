namespace Prometheus.Client.Abstractions
{
    public interface IValueObserver
    {
        void Observe(double val);

        void Observe(double val, long? timestamp);
    }
}
