using System;
using System.Threading.Tasks;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors
{
    /// <summary>
    /// This helper class wraps provided base writer and ensure that each metric
    /// uses only names exposed through MetricNames property to avoid names conflict
    /// </summary>
    internal sealed class MetricWriterWrapper : IMetricsWriter
    {
        private readonly IMetricsWriter _baseWriter;
        private ICollector _currentCollector;

        public MetricWriterWrapper(IMetricsWriter baseWriter)
        {
            _baseWriter = baseWriter;
        }

        public void SetCurrentCollector(ICollector collector)
        {
            _currentCollector = collector;
        }

        public Task FlushAsync()
        {
            return _baseWriter.FlushAsync();
        }

        public Task CloseWriterAsync()
        {
            return _baseWriter.CloseWriterAsync();
        }

        public void Dispose()
        {
            _currentCollector = null;
            _baseWriter.Dispose();
        }

        public IMetricsWriter StartMetric(string metricName)
        {
            for (var i = 0; i < _currentCollector.MetricNames.Count; i++)
            {
                if (string.Equals(metricName, _currentCollector.MetricNames[i], StringComparison.Ordinal))
                {
                    return _baseWriter.StartMetric(metricName);
                }
            }

            throw new InvalidOperationException("Cannot use a metric name other than reserved by collector.");
        }

        public ISampleWriter StartSample(string suffix = "")
        {
            return _baseWriter.StartSample(suffix);
        }

        public IMetricsWriter WriteHelp(string help)
        {
            return _baseWriter.WriteHelp(help);
        }

        public IMetricsWriter WriteType(MetricType metricType)
        {
            return _baseWriter.WriteType(metricType);
        }

        public IMetricsWriter EndMetric()
        {
            return _baseWriter.EndMetric();
        }
    }
}
