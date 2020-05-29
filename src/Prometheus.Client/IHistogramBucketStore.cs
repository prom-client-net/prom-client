namespace Prometheus.Client
{
    internal interface IHistogramBucketStore
    {
        public abstract void Observe(double value);

        public ThreadSafeLong[] Buckets { get; }
    }
}
