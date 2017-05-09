using System;
using System.Diagnostics;

namespace Prometheus.Client.Collectors
{
    /// <summary>
    ///     Windows specific metrics .net
    /// </summary>
    public class WindowsDotNetStatsCollector : IOnDemandCollector
    {
        private readonly Process _process;
        private Gauge _numThreads;

        private Counter _perfErrors;
        private Gauge _pid;
        private Gauge _privateMemorySize;
        private Gauge _virtualMemorySize;
        private Gauge _workingSet;

        /// <summary>
        ///     Constructors
        /// </summary>
        public WindowsDotNetStatsCollector()
        {
            _process = Process.GetCurrentProcess();
        }

        /// <inheritdoc />
        public void RegisterMetrics()
        {
            _perfErrors = Metrics.CreateCounter("dotnet_collection_errors_total", "Total number of errors that occured during collections");

            // Windows specific metrics
            _virtualMemorySize = Metrics.CreateGauge("process_windows_virtual_bytes", "Process virtual memory size");
            _workingSet = Metrics.CreateGauge("process_windows_working_set", "Process working set");
            _privateMemorySize = Metrics.CreateGauge("process_windows_private_bytes", "Process private memory size");
            _numThreads = Metrics.CreateGauge("process_windows_num_threads", "Total number of threads");
            _pid = Metrics.CreateGauge("process_windows_processid", "Process ID");
            _pid.Set(_process.Id);
        }

        /// <inheritdoc />
        public void UpdateMetrics()
        {
            try
            {
                _process.Refresh();
                _virtualMemorySize.Set(_process.VirtualMemorySize64);
                _workingSet.Set(_process.WorkingSet64);
                _privateMemorySize.Set(_process.PrivateMemorySize64);
                _numThreads.Set(_process.Threads.Count);
            }
            catch (Exception)
            {
                _perfErrors.Inc();
            }
        }
    }
}