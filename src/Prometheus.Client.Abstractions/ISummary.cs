namespace Prometheus.Client;

/// <summary>
///     Summary metric type
///     <remarks>
///         https://prometheus.io/docs/concepts/metric_types/#summary
///     </remarks>
/// </summary>
public interface ISummary : IMetric<SummaryState>, IValueObserver
{
}
