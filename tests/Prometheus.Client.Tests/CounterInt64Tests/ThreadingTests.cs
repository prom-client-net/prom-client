using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.CounterInt64Tests
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
                    long vl = 0;
                    var rnd = new Random();
                    for (var i = 0; i < observations; i++)
                    {
                        metric.Inc(rnd.Next());
                        if(i % 100 == 0)
                            vl = metric.Value;
                    }

                    metric.Reset();
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private CounterInt64 CreateCounter()
        {
            var config = new MetricConfiguration("test", string.Empty, Array.Empty<string>(), false);
            return new CounterInt64(config, Array.Empty<string>());
        }
    }
}
