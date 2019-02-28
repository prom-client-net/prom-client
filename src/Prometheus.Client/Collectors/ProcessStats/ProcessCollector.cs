using System;
using System.Diagnostics;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.ProcessStats
{
    /// <inheritdoc />
    /// <summary>
    ///     Collects metrics on .net without performance counters
    /// </summary>
    public class ProcessCollector : ICollector
    {
        private static readonly string[] _labels = new string[0];

        private readonly Process _process;
        private readonly double _processStartTime;

        /// <inheritdoc />
        /// <summary>
        ///     Constructors
        /// </summary>
        public ProcessCollector(Process process)
        {
            _process = process;

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            _processStartTime = (_process.StartTime.ToUniversalTime() - epoch).TotalSeconds;
        }

        public string Name => "process_collector";

        public string[] LabelNames => _labels;

        public void Collect(IMetricsWriter writer)
        {
            _process.Refresh();

            writer.WriteMetricHeader("process_cpu_seconds_total", MetricType.Counter, "Total user and system CPU time spent in seconds");
            writer.WriteSample(_process.TotalProcessorTime.TotalSeconds);

            writer.WriteMetricHeader("process_virtual_bytes", MetricType.Gauge, "Process virtual memory size");
            writer.WriteSample(_process.VirtualMemorySize64);

            writer.WriteMetricHeader("process_working_set", MetricType.Gauge, "Process working set");
            writer.WriteSample(_process.WorkingSet64);

            writer.WriteMetricHeader("process_private_bytes", MetricType.Gauge, "Process private memory size");
            writer.WriteSample(_process.PrivateMemorySize64);

            writer.WriteMetricHeader("process_num_threads", MetricType.Gauge, "Total number of threads");
            writer.WriteSample(_process.Threads.Count);

            writer.WriteMetricHeader("process_processid", MetricType.Untyped, "Process ID");
            writer.WriteSample(_process.Id);

            writer.WriteMetricHeader("process_start_time_seconds", MetricType.Untyped, "Start time of the process since unix epoch in seconds");
            writer.WriteSample(_processStartTime);
        }
    }
}
