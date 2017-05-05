using Prometheus.Advanced.DataContracts;
using System.Collections.Generic;

namespace Prometheus.Client.Advanced
{
    public interface ICollectorRegistry
    {
        ICollector GetOrAdd(ICollector collector);

        bool Remove(ICollector collector);

        IEnumerable<MetricFamily> CollectAll();
    }
}