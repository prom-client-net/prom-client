namespace Prometheus.Client.Collectors
{
    public class CollectorConfiguration
    {
        public CollectorConfiguration(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
