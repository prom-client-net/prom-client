using System;
using System.Collections.Generic;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Tests.Mocks
{
    internal class DummyMetric : MetricBase<MetricConfiguration>, IDummyMetric
    {
        public DummyMetric(MetricConfiguration config, IReadOnlyList<string> labelValues, Func<DateTimeOffset> currentTimeProvider)
            : base(config, labelValues, currentTimeProvider)
        {
        }

        public void Observe(long? ts)
        {
            TrackObservation(ts);
        }

        protected internal override void Collect(IMetricsWriter writer)
        {
        }
    }
}
