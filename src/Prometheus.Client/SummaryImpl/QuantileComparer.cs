using System.Collections.Generic;
using Prometheus.Client.Contracts;

namespace Prometheus.Client.SummaryImpl
{
    internal class QuantileComparer : IComparer<CQuantile>
    {
        public int Compare(CQuantile x, CQuantile y)
        {
            return x.Quantile.CompareTo(y.Quantile);
        }
    }
}
