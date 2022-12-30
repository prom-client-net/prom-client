using System.Collections.Generic;

namespace Prometheus.Client.SummaryImpl;

internal delegate double Invariant(SampleStream stream, double r);

internal static class Invariants
{
    // LowBiased returns an invariant for low-biased quantiles
    // (e.g. 0.01, 0.1, 0.5) where the needed quantiles are not known a priori, but
    // error guarantees can still be given even for the lower ranks of the data
    // distribution.
    //
    // The provided epsilon is a relative error, i.e. the true quantile of a value
    // returned by a query is guaranteed to be within (1±Epsilon)*Quantile.
    //
    // See http://www.cs.rutgers.edu/~muthu/bquant.pdf for time, space, and error
    // properties.
    public static Invariant LowBiased(double epsilon) => (stream, r) => 2 * epsilon * r;

    // HighBiased returns an invariant for high-biased quantiles
    // (e.g. 0.01, 0.1, 0.5) where the needed quantiles are not known a priori, but
    // error guarantees can still be given even for the higher ranks of the data
    // distribution.
    //
    // The provided epsilon is a relative error, i.e. the true quantile of a value
    // returned by a query is guaranteed to be within 1-(1±Epsilon)*(1-Quantile).
    //
    // See http://www.cs.rutgers.edu/~muthu/bquant.pdf for time, space, and error
    // properties.
    public static Invariant HighBiased(double epsilon) => (stream, r) => 2 * epsilon * (stream.Count - r);

    // Targeted returns an invariant concerned with a particular set of
    // quantile values that are supplied a priori. Knowing these a priori reduces
    // space and computation time. The targets map maps the desired quantiles to
    // their absolute errors, i.e. the true quantile of a value returned by a query
    // is guaranteed to be within (Quantile±Epsilon).
    //
    // See http://www.cs.rutgers.edu/~muthu/bquant.pdf for time, space, and error properties.
    public static Invariant Targeted(IReadOnlyList<QuantileEpsilonPair> targets)
    {
        return (stream, r) =>
        {
            double m = double.MaxValue;

            for (var i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                double f;
                if (target.Quantile * stream.Count <= r)
                    f = (2 * target.Epsilon * r) / target.Quantile;
                else
                    f = (2 * target.Epsilon * (stream.Count - r)) / (1 - target.Quantile);

                if (f < m)
                    m = f;
            }

            return m;
        };
    }
}
