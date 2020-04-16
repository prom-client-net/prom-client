using System;
using System.Threading.Tasks;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollectorRegistry
    {
        void Add(ICollector collector);

        bool TryGet(string name, out ICollector collector);

        TCollector GetOrAdd<TCollector, TConfig>(TConfig config, Func<TConfig, TCollector> collectorFactory)
            where TCollector : class, ICollector
            where TConfig: CollectorConfiguration;

        ICollector Remove(string name);

        bool Remove(ICollector collector);

        Task CollectToAsync(IMetricsWriter writer);
    }
}
