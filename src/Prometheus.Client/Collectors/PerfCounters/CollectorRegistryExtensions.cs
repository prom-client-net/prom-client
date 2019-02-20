#if NET45

using System;
using System.Diagnostics;

using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Collectors.PerfCounters
{
    public static class CollectorRegistryExtensions
    {
        private const string _memCat = ".NET CLR Memory";
        private const string _procCat = "Process";

        private static readonly string[] _standardPerfCounters =
        {
            _memCat, "Gen 0 heap size",
            _memCat, "Gen 1 heap size",
            _memCat, "Gen 2 heap size",
            _memCat, "Large Object Heap size",
            _memCat, "% Time in GC",
            _procCat, "% Processor Time",
            _procCat, "Private Bytes",
            _procCat, "Working Set",
            _procCat, "Virtual Bytes"
        };

        public static ICollectorRegistry UsePerfCounters(this ICollectorRegistry registry)
        {
            var currentProcess = Process.GetCurrentProcess();
            var instanceName = IsLinux() ? currentProcess.Id.ToString() : currentProcess.ProcessName;

            for (var i = 0; i < _standardPerfCounters.Length; i += 2)
            {
                var category = _standardPerfCounters[i];
                var name = _standardPerfCounters[i + 1];

                var perfCounter = new PerformanceCounter(category, name, instanceName);
                registry.Add(new PerfCounterCollector(perfCounter));
            }

            return registry;
        }

        private static bool IsLinux()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix;
        }
    }
}

#endif
