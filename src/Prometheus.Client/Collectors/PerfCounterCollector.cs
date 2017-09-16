#if !NETSTANDART13

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prometheus.Client.Collectors
{
    /// <summary>
    ///     Collects metrics on standard Performance Counters
    /// </summary>
    public class PerfCounterCollector : IOnDemandCollector
    {
        private const string MemCat = ".NET CLR Memory";
        private const string ProcCat = "Process";

        private static readonly string[] StandardPerfCounters =
        {
            MemCat, "Gen 0 heap size",
            MemCat, "Gen 1 heap size",
            MemCat, "Gen 2 heap size",
            MemCat, "Large Object Heap size",
            MemCat, "% Time in GC",
            ProcCat, "% Processor Time",
            ProcCat, "Private Bytes",
            ProcCat, "Working Set",
            ProcCat, "Virtual Bytes"
        };

        private readonly List<Tuple<Gauge, PerformanceCounter>> _collectors = new List<Tuple<Gauge, PerformanceCounter>>();
        private readonly string _instanceName;
        private Counter _perfErrors;

        /// <summary>
        ///     Constructor
        /// </summary>
        public PerfCounterCollector()
        {
            var currentProcess = Process.GetCurrentProcess();
            _instanceName = currentProcess.ProcessName;
            if (IsLinux())
                _instanceName = currentProcess.Id.ToString();
        }

        /// <summary>
        ///     Register metrics
        /// </summary>
        public void RegisterMetrics()
        {
            for (var i = 0; i < StandardPerfCounters.Length; i += 2)
            {
                var category = StandardPerfCounters[i];
                var name = StandardPerfCounters[i + 1];

                RegisterPerfCounter(category, name);
            }

            _perfErrors = Metrics.CreateCounter("performance_counter_errors_total", "Total number of errors that occured during performance counter collections");
        }

        /// <summary>
        ///     Update metrics
        /// </summary>
        public void UpdateMetrics()
        {
            foreach (var collector in _collectors)
                try
                {
                    collector.Item1.Set(collector.Item2.NextValue());
                }
                catch (Exception)
                {
                    _perfErrors.Inc();
                }
        }

        private static bool IsLinux()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix;
        }

        private void RegisterPerfCounter(string category, string name)
        {
            var gauge = Metrics.CreateGauge(GetName(category, name), GetHelp(name));
            _collectors.Add(Tuple.Create(gauge, new PerformanceCounter(category, name, _instanceName)));
        }

        private static string GetHelp(string name)
        {
            return name + " Perf Counter";
        }

        private static string GetName(string category, string name)
        {
            return ToPromName(category) + "_" + ToPromName(name);
        }

        private static string ToPromName(string name)
        {
            return name.Replace("%", "pct").Replace(" ", "_").Replace(".", "dot").ToLowerInvariant();
        }
    }
}

#endif