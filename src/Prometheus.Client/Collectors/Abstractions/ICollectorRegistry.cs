using System.Collections.Generic;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollectorRegistry
    {
        ICollector Add(ICollector collector);

        ICollector GetOrAdd(ICollector collector);

        bool Remove(ICollector collector);

        IEnumerable<ICollector> Enumerate();
    }
}
