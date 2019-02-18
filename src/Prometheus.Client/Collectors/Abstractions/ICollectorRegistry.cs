using System.Collections.Generic;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollectorRegistry
    {
        ICollector GetOrAdd(ICollector collector);

        bool Remove(ICollector collector);

        IEnumerable<ICollector> Enumerate();

        void RegisterOnDemandCollectors(List<IOnDemandCollector> onDemandCollectors);
    }
}
