using System;
using System.Collections.Generic;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollectorRegistry
    {
        void Add(string name, ICollector collector);

        bool TryGet(string name, out ICollector collector);

        TCollector GetOrAdd<TCollector, TConfig>(TConfig config, Func<TConfig, TCollector> collectorFactory)
            where TCollector : class, ICollector
            where TConfig: CollectorConfiguration;

        ICollector Remove(string name);

        IEnumerable<ICollector> Enumerate();
    }
}
