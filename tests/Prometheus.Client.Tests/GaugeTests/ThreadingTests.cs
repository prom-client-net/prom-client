using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.GaugeTests
{
    public class ThreadingTests
    {
        [Theory]
        [InlineData(10000, 1)]
        [InlineData(10000, 10)]
        [InlineData(10000, 100)]
        public async Task ObserveInParallel(int observations, int threads)
        {
            var metric = CreateGauge();

            var tasks = Enumerable.Range(0, threads)
                .Select(n => Task.Run(() =>
                {
                    double vl;
                    var rnd = new Random();
                    for (var i = 0; i < observations; i++)
                    {
                        metric.Inc(rnd.NextDouble());
                        if (i % 100 == 0)
                            vl = metric.Value;
                    }

                    metric.Reset();
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private IGauge CreateGauge()
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), false);
            return new Gauge(config, Array.Empty<string>());
        }
    }
}
