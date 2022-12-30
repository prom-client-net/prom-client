using System;

namespace Prometheus.Client.Collectors;

public class CollectorConfiguration
{
    public CollectorConfiguration(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
    }

    public string Name { get; }
}
