using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Collectors
{
    public class CollectorConfiguration : ICollectorConfiguration
    {
        public CollectorConfiguration(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
