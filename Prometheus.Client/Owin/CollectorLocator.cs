using System.Collections.Generic;
using Prometheus.Advanced;
using Prometheus.Client.Advanced;

namespace Prometheus.Client.Owin
{
    public class CollectorLocator
    {
        public IEnumerable<IOnDemandCollector> Get()
        {
            yield return new DotNetStatsCollector();
        }
    }
}