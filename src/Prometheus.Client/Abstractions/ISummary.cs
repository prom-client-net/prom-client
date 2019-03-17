using System.Collections.Generic;

namespace Prometheus.Client.Abstractions
{
    /// <summary>
    ///     Summary metric type
    ///     <remarks>
    ///         https://prometheus.io/docs/concepts/metric_types/#summary
    ///     </remarks>
    /// </summary>
    public interface ISummary : IMetric<SummaryState>, IValueObserver
    {
    }

    public readonly struct SummaryState
    {
        public SummaryState(long count, double sum, IReadOnlyList<KeyValuePair<double, double>> quantiles)
        {
            Count = count;
            Sum = sum;
            Quantiles = quantiles;
        }

        public long Count { get; }

        public double Sum { get; }

        public IReadOnlyList<KeyValuePair<double, double>> Quantiles { get; }
    }
}
