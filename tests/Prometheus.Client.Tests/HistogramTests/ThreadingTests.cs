using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.HistogramTests;

public class ThreadingTests
{
    [Theory]
    [InlineData(10000, 1)]
    [InlineData(10000, 10)]
    [InlineData(10000, 100)]
    public async Task ObserveInParallel(int observations, int threads)
    {
        var metric = CreateHistogram();

        var tasks = Enumerable.Range(0, threads)
            .Select(n => Task.Run(() =>
            {
                HistogramState vl;
                var rnd = new Random();
                for (var i = 0; i < observations; i++)
                {
                    metric.Observe(rnd.NextDouble());
                    if (i % 100 == 0)
                        vl = metric.Value;
                }

                metric.Reset();
            }))
            .ToArray();

        await Task.WhenAll(tasks);
    }

    private IHistogram CreateHistogram(double[] buckets = null)
    {
        var config = new HistogramConfiguration("test", string.Empty, Array.Empty<string>(), buckets, false);
        return new Histogram(config, Array.Empty<string>());
    }
}
