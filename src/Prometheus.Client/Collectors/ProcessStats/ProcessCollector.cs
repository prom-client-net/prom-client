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
        private const string CpuSecondsTotalName = "process_cpu_seconds_total";
        private const string VirtualBytesName = "process_virtual_bytes";
        private const string WorkingSetName = "process_working_set";
        private const string PrivateBytesName = "process_private_bytes";
        private const string NumThreadsName = "process_num_threads";
        private const string ProcessIdName = "process_processid";
        private const string StartTimeSecondsName = "process_start_time_seconds";

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

        public string[] MetricNames => new[]
        {
            CpuSecondsTotalName,
            VirtualBytesName,
            WorkingSetName,
            PrivateBytesName,
            NumThreadsName,
            ProcessIdName,
            StartTimeSecondsName,
        };

        public void Collect(IMetricsWriter writer)
        {
            _process.Refresh();

            writer.WriteMetricHeader(CpuSecondsTotalName, MetricType.Counter, "Total user and system CPU time spent in seconds");
            writer.WriteSample(_process.TotalProcessorTime.TotalSeconds);

            writer.WriteMetricHeader(VirtualBytesName, MetricType.Gauge, "Process virtual memory size");
            writer.WriteSample(_process.VirtualMemorySize64);

            writer.WriteMetricHeader(WorkingSetName, MetricType.Gauge, "Process working set");
            writer.WriteSample(_process.WorkingSet64);

            writer.WriteMetricHeader(PrivateBytesName, MetricType.Gauge, "Process private memory size");
            writer.WriteSample(_process.PrivateMemorySize64);

            writer.WriteMetricHeader(NumThreadsName, MetricType.Gauge, "Total number of threads");
            writer.WriteSample(_process.Threads.Count);

            writer.WriteMetricHeader(ProcessIdName, MetricType.Untyped, "Process ID");
            writer.WriteSample(_process.Id);

            writer.WriteMetricHeader(StartTimeSecondsName, MetricType.Untyped, "Start time of the process since unix epoch in seconds");
            writer.WriteSample(_processStartTime);
        }
    }
}
