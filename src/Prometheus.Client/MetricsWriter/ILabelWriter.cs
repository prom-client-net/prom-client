namespace Prometheus.Client.MetricsWriter
{
    public interface ILabelWriter
    {
        ILabelWriter WriteLabel(string name, string value);

        ISampleWriter EndLabels();
    }
}
