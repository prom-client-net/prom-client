using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Prometheus.Client;

public class SummaryConfiguration : MetricConfiguration
{
    // Label that defines the quantile in a summary.
    private const string _quantileLabel = "quantile";

    // Default number of buckets used to calculate the age of observations
    private const int _defaultAgeBuckets = 5;

    // Standard buffer size for collecting Summary observations
    private const int _defaultBufCap = 500;

    // Default Summary quantile values.
    public static readonly IReadOnlyList<QuantileEpsilonPair> DefaultObjectives = new List<QuantileEpsilonPair>
    {
        new QuantileEpsilonPair(0.5, 0.05),
        new QuantileEpsilonPair(0.9, 0.01),
        new QuantileEpsilonPair(0.99, 0.001)
    };

    private static readonly double[] DefaultSortedObjectives = { 0.5, 0.9, 0.99 };

    // Default duration for which observations stay relevant
    private static readonly TimeSpan _defaultMaxAge = TimeSpan.FromMinutes(10);

    private readonly Lazy<string[]> _formattedObjectives;

    public SummaryConfiguration(
        string name,
        string help,
        string[] labelNames,
        bool includeTimestamp,
        IReadOnlyList<QuantileEpsilonPair> objectives = null,
        TimeSpan? maxAge = null,
        int? ageBuckets = null,
        int? bufCap = null)
        : base(name, help, labelNames, includeTimestamp)
    {
        Objectives = objectives;
        if (Objectives == null || Objectives.Count == 0)
        {
            Objectives = DefaultObjectives;
            SortedObjectives = DefaultSortedObjectives;
        }
        else
        {
            var sorted = new double[Objectives.Count];
            for (int i = 0; i < Objectives.Count; i++)
                sorted[i] = Objectives[i].Quantile;

            Array.Sort(sorted);
            SortedObjectives = sorted;
        }

        _formattedObjectives = new Lazy<string[]>(() => GetFormattedObjectives(SortedObjectives));

        MaxAge = maxAge ?? _defaultMaxAge;
        if (MaxAge < TimeSpan.Zero)
            throw new ArgumentException($"Illegal max age {MaxAge}");

        AgeBuckets = ageBuckets ?? _defaultAgeBuckets;
        if (AgeBuckets == 0)
            AgeBuckets = _defaultAgeBuckets;

        BufCap = bufCap ?? _defaultBufCap;
        if (BufCap == 0)
            BufCap = _defaultBufCap;

        if (LabelNames.Any(_ => _ == _quantileLabel))
            throw new ArgumentException($"{_quantileLabel} is a reserved label name");
    }

    // Objectives defines the quantile rank estimates with their respective
    // absolute error. If Objectives[q] = e, then the value reported
    // for q will be the φ-quantile value for some φ between q-e and q+e.
    // The default value is DefObjectives.
    public IReadOnlyList<QuantileEpsilonPair> Objectives { get; }

    // MaxAge defines the duration for which an observation stays relevant
    // for the summary. Must be positive. The default value is DefMaxAge.
    public TimeSpan MaxAge { get; }

    // AgeBuckets is the number of buckets used to exclude observations that
    // are older than MaxAge from the summary. A higher number has a
    // resource penalty, so only increase it if the higher resolution is
    // really required. For very high observation rates, you might want to
    // reduce the number of age buckets. With only one age bucket, you will
    // effectively see a complete reset of the summary each time MaxAge has
    // passed. The default value is DefAgeBuckets.
    public int AgeBuckets { get; }

    // BufCap defines the default sample stream buffer size.  The default
    // value of DefBufCap should suffice for most uses. If there is a need
    // to increase the value, a multiple of 500 is recommended (because that
    // is the internal buffer size of the underlying package
    // "github.com/bmizerany/perks/quantile").
    public int BufCap { get; }

    internal double[] SortedObjectives { get; }

    internal string[] FormattedObjectives => _formattedObjectives.Value;

    private static string[] GetFormattedObjectives(IReadOnlyList<double> objectives)
    {
        if (objectives == null || objectives.Count == 0)
            return Array.Empty<string>();

        var result = new string[objectives.Count];
        for (var i = 0; i < objectives.Count; i++)
            result[i] = objectives[i].ToString(CultureInfo.InvariantCulture);

        return result;
    }
}
