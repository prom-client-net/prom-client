using System.Collections.Generic;
using System.Diagnostics;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client.Collectors.ProcessStats
{
    /// <inheritdoc />
    /// <summary>
    ///     Collects metrics on .net without performance counters
    /// </summary>
    public class ProcessCollector : ICollector
    {
        private const string _cpuSecondsTotalName = "process_cpu_seconds_total";
        private const string _virtualBytesName = "process_virtual_bytes";
        private const string _workingSetName = "process_working_set";
        private const string _privateBytesName = "process_private_bytes";
        private const string _numThreadsName = "process_num_threads";
        private const string _processIdName = "process_processid";
        private const string _startTimeSecondsName = "process_start_time_seconds";

        private readonly Process _process;
        private readonly double _processStartTime;

        /// <summary>
        ///     Constructors
        /// </summary>
        public ProcessCollector(Process process)
        {
            _process = process;
            Configuration = new CollectorConfiguration(nameof(ProcessCollector));

            _processStartTime = _process.StartTime.ToUniversalTime().ToUnixTimeSeconds();
            MetricNames = new[]
            {
                _cpuSecondsTotalName,
                _virtualBytesName,
                _workingSetName,
                _privateBytesName,
                _numThreadsName,
                _processIdName,
                _startTimeSecondsName,
            };
        }

        public ICollectorConfiguration Configuration { get; }

        public IReadOnlyList<string> MetricNames { get; }

        public void Collect(IMetricsWriter writer)
        {
            _process.Refresh();

            writer.WriteMetricHeader(_cpuSecondsTotalName, MetricType.Counter, "Total user and system CPU time spent in seconds");
            writer.WriteSample(_process.TotalProcessorTime.TotalSeconds);
            writer.EndMetric();

            writer.WriteMetricHeader(_virtualBytesName, MetricType.Gauge, "Process virtual memory size");
            writer.WriteSample(_process.VirtualMemorySize64);
            writer.EndMetric();

            writer.WriteMetricHeader(_workingSetName, MetricType.Gauge, "Process working set");
            writer.WriteSample(_process.WorkingSet64);
            writer.EndMetric();

            writer.WriteMetricHeader(_privateBytesName, MetricType.Gauge, "Process private memory size");
            writer.WriteSample(_process.PrivateMemorySize64);
            writer.EndMetric();

            writer.WriteMetricHeader(_numThreadsName, MetricType.Gauge, "Total number of threads");
            writer.WriteSample(_process.Threads.Count);
            writer.EndMetric();

            writer.WriteMetricHeader(_processIdName, MetricType.Gauge, "Process ID");
            writer.WriteSample(_process.Id);
            writer.EndMetric();

            writer.WriteMetricHeader(_startTimeSecondsName, MetricType.Gauge, "Start time of the process since unix epoch in seconds");
            writer.WriteSample(_processStartTime);
            writer.EndMetric();
        }
    }
}
