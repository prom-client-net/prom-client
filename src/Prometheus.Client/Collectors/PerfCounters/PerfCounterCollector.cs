#if NET45

using System;
using System.Diagnostics;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tools;

namespace Prometheus.Client.Collectors.PerfCounters
{
    /// <inheritdoc />
    /// <summary>
    ///     Collects metrics on standard Performance Counters
    /// </summary>
    public class PerfCounterCollector : ICollector
    {
        private static string[] NoLabels = new string[0];

        private readonly string _help;
        private readonly PerformanceCounter _perfCounter;
        private ThreadSafeDouble _lastValue;

        public PerfCounterCollector(PerformanceCounter perfCounter)
        {
            _perfCounter = perfCounter;
            Name = GetName(perfCounter.CategoryName, perfCounter.CounterName);
            _help = perfCounter.CounterHelp;
        }

        public string Name { get; }

        public string[] LabelNames => NoLabels;

        public void Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(Name, Contracts.CMetricType.Gauge, _help);
            double value = _lastValue.Value;
            try
            {
                value = _perfCounter.NextValue();
                _lastValue.Value = value;
            }
            catch (Exception)
            {
            }
            writer.WriteSample(value);
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
