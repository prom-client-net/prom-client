namespace Prometheus.Client.Advanced
{
    public interface IOnDemandCollector
    {
        void RegisterMetrics();

        void UpdateMetrics();
    }
}