using System.Collections.Generic;
using Prometheus.Contracts;

namespace Prometheus.Client.Collectors
{
    public interface ICollectorRegistry
    {
        ICollector GetOrAdd(ICollector collector);

        bool Remove(ICollector collector);

        IEnumerable<MetricFamily> CollectAll();

        void RegisterOnDemandCollectors(IEnumerable<IOnDemandCollector> onDemandCollectors);
    }
}