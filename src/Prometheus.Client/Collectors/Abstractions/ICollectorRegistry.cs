using System;
using System.Collections.Generic;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollectorRegistry
    {
        void Add(string name, ICollector collector);

        ICollector GetOrAdd(string name, Func<ICollector> collectorFactory);

        ICollector Remove(string name);

        IEnumerable<ICollector> Enumerate();
    }
}
