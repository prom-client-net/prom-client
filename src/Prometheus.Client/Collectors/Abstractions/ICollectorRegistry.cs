using System.Collections.Generic;
using Prometheus.Client.Contracts;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollectorRegistry
    {
        ICollector GetOrAdd(ICollector collector);

        bool Remove(ICollector collector);

        IEnumerable<CMetricFamily> CollectAll();

        void RegisterOnDemandCollectors(List<IOnDemandCollector> onDemandCollectors);
    }
}