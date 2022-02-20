using System;
using System.Collections.Generic;
using System.Diagnostics;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.ProcessStats
{
    /// <inheritdoc />
    /// <summary>
    ///     Collects metrics on .net without performance counters
    /// </summary>
    public class ProcessCollector : ICollector
    {
        private readonly string _cpuSecondsTotalName;
        private readonly string _virtualMemoryBytesName;
        private readonly string _workingSetBytesName;
        private readonly string _privateMemoryBytesName;
        private readonly string _numThreadsName;
        private readonly string _openHandlesName;
        private readonly string _processIdName;
        private readonly string _startTimeSecondsName;

        private readonly Process _process;
        private readonly double _processStartTime;

        public ProcessCollector(Process process)
            : this(process, string.Empty)
        {
        }

        public ProcessCollector(Process process, string prefixName)
        {
            _processIdName = prefixName + "process_processid";

            _cpuSecondsTotalName = prefixName + "process_cpu_seconds_total";
            _startTimeSecondsName = prefixName + "process_start_time_seconds";

            _virtualMemoryBytesName = prefixName + "process_virtual_memory_bytes";
            _workingSetBytesName = prefixName + "process_working_set_bytes";
            _privateMemoryBytesName = prefixName + "process_private_memory_bytes";

            _numThreadsName = prefixName + "process_num_threads";
            _openHandlesName = prefixName + "process_open_handles";

            _process = process;
            Configuration = new CollectorConfiguration(nameof(ProcessCollector));

            _processStartTime = ((DateTimeOffset)_process.StartTime.ToUniversalTime()).ToUnixTimeSeconds();
            MetricNames = new[]
            {
                _processIdName, _cpuSecondsTotalName, _startTimeSecondsName, _virtualMemoryBytesName, _workingSetBytesName, _privateMemoryBytesName, _numThreadsName,
                _openHandlesName
            };
        }

        public CollectorConfiguration Configuration { get; }

        public IReadOnlyList<string> MetricNames { get; }

        public void Collect(IMetricsWriter writer)
        {
            _process.Refresh();

            writer.WriteMetricHeader(_processIdName, MetricType.Gauge, "Process ID");
            writer.WriteSample(_process.Id);
            writer.EndMetric();

            writer.WriteMetricHeader(_cpuSecondsTotalName, MetricType.Counter, "Total user and system CPU time spent in seconds");
            writer.WriteSample(_process.TotalProcessorTime.TotalSeconds);
            writer.EndMetric();

            writer.WriteMetricHeader(_startTimeSecondsName, MetricType.Gauge, "Start time of the process since unix epoch in seconds");
            writer.WriteSample(_processStartTime);
            writer.EndMetric();

            writer.WriteMetricHeader(_virtualMemoryBytesName, MetricType.Gauge, "Process virtual memory size in bytes");
            writer.WriteSample(_process.VirtualMemorySize64);
            writer.EndMetric();

            writer.WriteMetricHeader(_workingSetBytesName, MetricType.Gauge, "Process working set in bytes");
            writer.WriteSample(_process.WorkingSet64);
            writer.EndMetric();

            writer.WriteMetricHeader(_privateMemoryBytesName, MetricType.Gauge, "Process private memory size in bytes");
            writer.WriteSample(_process.PrivateMemorySize64);
            writer.EndMetric();

            writer.WriteMetricHeader(_numThreadsName, MetricType.Gauge, "Total number of threads");
            writer.WriteSample(_process.Threads.Count);
            writer.EndMetric();

            writer.WriteMetricHeader(_openHandlesName, MetricType.Gauge, "Number of open handles");
            writer.WriteSample(_process.HandleCount);
            writer.EndMetric();
        }
    }
}
