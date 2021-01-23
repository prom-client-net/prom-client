using System;

namespace Prometheus.Client.HistogramImpl
{
    internal sealed class HistogramHighBucketsStore: IHistogramBucketStore
    {
        private readonly double[] _bounds;

        public HistogramHighBucketsStore(double[] bucketBounds)
        {
            _bounds = bucketBounds;
            Buckets = new ThreadSafeLong[bucketBounds.Length + 1]; // Allocate one more element for +Inf value
        }

        public void Observe(double value)
        {
            var bucketIndex = Array.BinarySearch(_bounds, value);
            if (bucketIndex < 0)
                bucketIndex = ~bucketIndex;

            Buckets[bucketIndex].Add(1);
        }

        public void Reset()
        {
            for (var i = 0; i < Buckets.Length; i++)
            {
                Buckets[i].Value = default;
            }
        }

        public ThreadSafeLong[] Buckets { get; }
    }
}
