using System;

namespace Prometheus.Client
{
    [Flags]
    public enum MetricFlags
    {
        None = 0,

        SuppressEmptySamples = 1,

        IncludeTimestamp = 2,

        Default = SuppressEmptySamples
    }
}
