using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests.SummaryTests;

public class ThreadingTests
{
    [Theory]
    [InlineData(10000, 1)]
    [InlineData(10000, 10)]
    [InlineData(10000, 100)]
    public async Task ObserveInParallel(int observations, int threads)
    {
        var metric = CreateSummary();

        var tasks = Enumerable.Range(0, threads)
            .Select(n => Task.Run(() =>
            {
                SummaryState vl;
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

    private Summary CreateSummary()
    {
        var config = new SummaryConfiguration("test", string.Empty, Array.Empty<string>(), false, TimeSpan.Zero);
        return new Summary(config, Array.Empty<string>());
    }
}
