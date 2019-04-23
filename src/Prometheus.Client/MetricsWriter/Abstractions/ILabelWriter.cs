namespace Prometheus.Client.MetricsWriter.Abstractions
{
    public interface ILabelWriter
    {
        ILabelWriter WriteLabel(string name, string value);

        ISampleWriter EndLabels();
    }
}
