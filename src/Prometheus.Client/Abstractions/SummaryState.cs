using System.Collections.Generic;

namespace Prometheus.Client.Abstractions
{
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
