using System.Collections.Generic;
using Prometheus.Contracts;

namespace Prometheus.Client.SummaryImpl
{
    internal class QuantileComparer : IComparer<Quantile>
    {
        public int Compare(Quantile x, Quantile y)
        {
            return x.quantile.CompareTo(y.quantile);
        }
    }
}
