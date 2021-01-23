namespace Prometheus.Client.MetricsWriter
{
    public interface ISampleWriter
    {
        ILabelWriter StartLabels();

        ISampleWriter WriteValue(double value);

        ISampleWriter WriteTimestamp(long timestamp);

        IMetricsWriter EndSample();
    }
}
