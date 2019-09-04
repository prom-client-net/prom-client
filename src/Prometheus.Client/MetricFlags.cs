using System;

namespace Prometheus.Client
{
    [Flags]
    public enum MetricFlags
    {
        None = 0,

        SupressEmptySamples = 1,

        IncludeTimestamp = 2,

        Default = SupressEmptySamples,
    }
}
