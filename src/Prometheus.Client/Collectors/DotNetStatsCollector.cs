using System;
using System.Collections.Generic;
using System.Diagnostics;
using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Collectors
{
    /// <inheritdoc />
    /// <summary>
    ///     Collects metrics on .net without performance counters
    /// </summary>
    public class DotNetStatsCollector : IOnDemandCollector
    {
        private readonly MetricFactory _metricFactory;
        private readonly List<Counter.ThisChild> _collectionCounts = new List<Counter.ThisChild>();
        private Gauge _totalMemory;
        private Counter _errorCounter;

        /// <inheritdoc />
        /// <summary>
        ///     Constructors
        /// </summary>
        public DotNetStatsCollector()
            : this(Metrics.DefaultFactory)
        {
        }

        /// <summary>
        ///     Constructors
        /// </summary>
        public DotNetStatsCollector(MetricFactory metricFactory)
        {
            _metricFactory = metricFactory;
        }

        /// <inheritdoc />
        public void RegisterMetrics()
        {
            _errorCounter = _metricFactory.CreateCounter("dotnet_collection_errors_total", "Total number of errors that occured during collections");

            var collectionCountsParent = _metricFactory.CreateCounter("dotnet_collection_count_total", "GC collection count", "generation");

            // .net specific metrics
            _totalMemory = _metricFactory.CreateGauge("dotnet_totalmemory", "Total known allocated memory");


            for (var gen = 0; gen <= GC.MaxGeneration; gen++)
                _collectionCounts.Add(collectionCountsParent.Labels(gen.ToString()));
        }

        public void UpdateMetrics()
        {
            try
            {
                for (var gen = 0; gen <= GC.MaxGeneration; gen++)
                {
                    var collectionCount = _collectionCounts[gen];
                    collectionCount.Inc(GC.CollectionCount(gen) - collectionCount.Value);
                }

                _totalMemory.Set(GC.GetTotalMemory(false));
            }
            catch (Exception)
            {
                _errorCounter.Inc();
            }
        }
    }
}