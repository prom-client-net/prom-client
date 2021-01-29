namespace Prometheus.Client.HistogramImpl
{
    internal interface IHistogramBucketStore
    {
        void Observe(double value);

        void Reset();

        ThreadSafeLong[] Buckets { get; }
    }
}
