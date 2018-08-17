#if NET45

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Collectors
{
    /// <inheritdoc />
    /// <summary>
    ///     Collects metrics on standard Performance Counters
    /// </summary>
    public class PerfCounterCollector : IOnDemandCollector
    {
        private readonly MetricFactory _metricFactory;
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

        private readonly List<Tuple<Gauge, PerformanceCounter>> _collectors = new List<Tuple<Gauge, PerformanceCounter>>();
        private readonly string _instanceName;
        private Counter _perfErrors;


        /// <summary>
        ///     Constructor
        /// </summary>
        public PerfCounterCollector()
            : this(Metrics.DefaultFactory)
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public PerfCounterCollector(MetricFactory metricFactory)
        {
            _metricFactory = metricFactory;
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
            for (var i = 0; i < _standardPerfCounters.Length; i += 2)
            {
                var category = _standardPerfCounters[i];
                var name = _standardPerfCounters[i + 1];

                RegisterPerfCounter(category, name);
            }

            _perfErrors = _metricFactory.CreateCounter("performance_counter_errors_total",
                "Total number of errors that occured during performance counter collections");
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
            var gauge = _metricFactory.CreateGauge(GetName(category, name), GetHelp(name));
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