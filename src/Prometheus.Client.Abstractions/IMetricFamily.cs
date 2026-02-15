using System;
using System.Collections.Generic;
#if HAS_ITUPLE
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client;

public interface IMetricFamily<TMetric>
    where TMetric : IMetric
{
    string Name { get; }
    TMetric Unlabelled { get; }
    IEnumerable<KeyValuePair<IReadOnlyList<string>, TMetric>> Labelled { get; }
    TMetric WithLabels(params string[] labels);
    TMetric RemoveLabelled(params string[] labels);
    IReadOnlyList<string> LabelNames { get; }
}

public interface IMetricFamily<TMetric, TLabels>
    where TMetric : IMetric
#if HAS_ITUPLE
    where TLabels : struct, ITuple, IEquatable<TLabels>
#else
    where TLabels : struct, IEquatable<TLabels>
#endif
{
    string Name { get; }
    TMetric Unlabelled { get; }
    IEnumerable<KeyValuePair<TLabels, TMetric>> Labelled { get; }
    TMetric WithLabels(TLabels labels);
    TMetric RemoveLabelled(TLabels labels);
    TLabels LabelNames { get; }
}
