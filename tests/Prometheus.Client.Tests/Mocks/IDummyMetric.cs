using Prometheus.Client.Abstractions;

namespace Prometheus.Client.Tests.Mocks
{
    public interface IDummyMetric : IMetric
    {
        void Observe(long? ts);
    }
}
