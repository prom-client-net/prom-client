namespace Prometheus.Client.MetricsWriter.Abstractions
{
    public interface ISampleWriter
    {
        ILabelWriter StartLabels();

        ISampleWriter WriteValue(double value);

        ISampleWriter WriteTimestamp(long timestamp);

        IMetricsWriter EndSample();
    }
}
