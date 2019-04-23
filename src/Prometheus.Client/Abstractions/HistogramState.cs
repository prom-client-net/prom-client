using System.Collections.Generic;

namespace Prometheus.Client.Abstractions
{
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
