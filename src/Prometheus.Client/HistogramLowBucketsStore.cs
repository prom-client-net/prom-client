namespace Prometheus.Client
{
    internal sealed class HistogramLowBucketsStore : IHistogramBucketStore
    {
        private readonly double[] _bounds;

        public HistogramLowBucketsStore(double[] bucketBounds)
        {
            _bounds = bucketBounds;
            Buckets = new ThreadSafeLong[bucketBounds.Length + 1]; // Allocate one more element for +Inf value
        }

        public void Observe(double value)
        {
            var bucketIndex = Buckets.Length - 1;

            for (var i = 0; i < _bounds.Length; i++)
            {
                if (value <= _bounds[i])
                {
                    bucketIndex = i;
                    break;
                }
            }

            Buckets[bucketIndex].Add(1);
        }

        public ThreadSafeLong[] Buckets { get; }
    }
}
