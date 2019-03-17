using System.Collections.Generic;

namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Histogram metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#histogram
    ///     </remarks>
    /// </summary>
    public interface IHistogram : IMetric<HistogramState>, IValueObserver
    {
    }

    public readonly struct HistogramState
    {
        public HistogramState(long count, double sum, IReadOnlyList<KeyValuePair<double, long>> buckets)
        {
            Count = count;
            Sum = sum;
            Buckets = buckets;
        }

        public long Count { get; }

        public double Sum { get; }

        public IReadOnlyList<KeyValuePair<double, long>> Buckets { get; }
    }
}
