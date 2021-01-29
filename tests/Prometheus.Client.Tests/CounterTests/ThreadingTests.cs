using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.CounterTests
{
    public class ThreadingTests
    {
        [Theory]
        [InlineData(10000, 1)]
        [InlineData(10000, 10)]
        [InlineData(10000, 100)]
        public async Task ObserveInParallel(int observations, int threads)
        {
            var metric = CreateCounter();

            var tasks = Enumerable.Range(0, threads)
                .Select(n => Task.Run(() =>
                {
                    double vl = 0;
                    var rnd = new Random();
                    for (var i = 0; i < observations; i++)
                    {
                        metric.Inc(rnd.NextDouble());
                        if(i % 100 == 0)
                            vl = metric.Value;
                    }

                    metric.Reset();
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private Counter CreateCounter()
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), false);
            return new Counter(config, Array.Empty<string>());
        }
    }
}
