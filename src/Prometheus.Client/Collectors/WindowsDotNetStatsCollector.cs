using System;
using System.Diagnostics;

namespace Prometheus.Client.Collectors
{
    /// <inheritdoc />
    /// <summary>
    ///     Windows specific metrics .net
    /// </summary>
    public class WindowsDotNetStatsCollector : IOnDemandCollector
    {
        private readonly MetricFactory _metricFactory;
        private readonly Process _process;
        private Gauge _numThreads;

        private Counter _perfErrors;
        private Gauge _privateMemorySize;
        private Gauge _virtualMemorySize;
        private Gauge _workingSet;


        /// <summary>
        ///     Constructors
        /// </summary>
        public WindowsDotNetStatsCollector()
            : this(Metrics.DefaultFactory)
        {
        }

        /// <summary>
        ///     Constructors
        /// </summary>
        public WindowsDotNetStatsCollector(MetricFactory metricFactory)
        {
            _metricFactory = metricFactory;
            _process = Process.GetCurrentProcess();
        }

        /// <inheritdoc />
        public void RegisterMetrics()
        {
            _perfErrors = _metricFactory.CreateCounter("dotnet_collection_errors_total", "Total number of errors that occured during collections");

            // Windows specific metrics
            _virtualMemorySize = _metricFactory.CreateGauge("process_windows_virtual_bytes", "Process virtual memory size");
            _workingSet = _metricFactory.CreateGauge("process_windows_working_set", "Process working set");
            _privateMemorySize = _metricFactory.CreateGauge("process_windows_private_bytes", "Process private memory size");
            _numThreads = _metricFactory.CreateGauge("process_windows_num_threads", "Total number of threads");
            _metricFactory.CreateGauge("process_windows_processid", "Process ID").Set(_process.Id);
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