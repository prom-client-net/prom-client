using System.Collections.Generic;
using Prometheus.Advanced.DataContracts;

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
