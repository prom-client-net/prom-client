using System;
using System.Diagnostics;
using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Collectors
{
    /// <inheritdoc />
    /// <summary>
    ///     Collects metrics on .net without performance counters
    /// </summary>
    public class ProcessCollector : IOnDemandCollector
    {
        private readonly MetricFactory _metricFactory;
        private readonly Process _process;
        private Counter _cpuTotal;
        private Counter _errorCounter;
        
        private Gauge _numThreads;
        private Gauge _privateMemorySize;
        private Gauge _virtualMemorySize;
        private Gauge _workingSet;


        /// <inheritdoc />
        /// <summary>
        ///     Constructors
        /// </summary>
        public ProcessCollector()
            : this(Metrics.DefaultFactory)
        {
        }

        /// <summary>
        ///     Constructors
        /// </summary>
        public ProcessCollector(MetricFactory metricFactory)
        {
            _metricFactory = metricFactory;
            _process = Process.GetCurrentProcess();
        }

        /// <inheritdoc />
        public void RegisterMetrics()
        {
            _errorCounter = _metricFactory.CreateCounter("process_errors_total", "Total number of errors that occured during collections.");

            _cpuTotal = _metricFactory.CreateCounter("process_cpu_seconds_total", "Total user and system CPU time spent in seconds.");

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            _metricFactory.CreateGauge("process_start_time_seconds", "Start time of the process since unix epoch in seconds.")
                .Set((_process.StartTime.ToUniversalTime() - epoch).TotalSeconds);
            
                        _virtualMemorySize = _metricFactory.CreateGauge("process_virtual_bytes", "Process virtual memory size.");
            _workingSet = _metricFactory.CreateGauge("process_working_set", "Process working set.");
            _privateMemorySize = _metricFactory.CreateGauge("process_private_bytes", "Process private memory size.");
            _numThreads = _metricFactory.CreateGauge("process_num_threads", "Total number of threads.");
            
            _metricFactory.CreateGauge("process_processid", "Process ID").Set(_process.Id); // todo: Untyped           
        }

        public void UpdateMetrics()
        {
            try
            {
                _process.Refresh();

                _cpuTotal.Inc(_process.TotalProcessorTime.TotalSeconds - _cpuTotal.Value);
                
                _virtualMemorySize.Set(_process.VirtualMemorySize64);
                _workingSet.Set(_process.WorkingSet64);
                _privateMemorySize.Set(_process.PrivateMemorySize64);
                _numThreads.Set(_process.Threads.Count);
            }
            catch (Exception)
            {
                _errorCounter.Inc();
            }
        }
    }
}
