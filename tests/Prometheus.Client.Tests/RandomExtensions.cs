using System;

namespace Prometheus.Client.Tests
{
    public static class RandomExtensions
    {
        public static double NormDouble(this Random r)
        {
            double u1 = r.NextDouble();
            double u2 = r.NextDouble();

            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }
    }
}
