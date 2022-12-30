using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests;

public class ThreadSafeLongTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(10)]
    [InlineData(long.MinValue)]
    [InlineData(long.MaxValue)]
    public void CanInitConstructor(long value)
    {
        var tslong = new ThreadSafeLong(value);
        Assert.Equal(value, tslong.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(10)]
    [InlineData(long.MinValue)]
    [InlineData(long.MaxValue)]
    public void CanSetValue(long value)
    {
        var tslong = new ThreadSafeLong(0);
        tslong.Add(value);
        Assert.Equal(value, tslong.Value);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(-10, 10, 0)]
    [InlineData(10, 2, 12)]
    public void CanAddValue(long initial, long added, long expected)
    {
        var tslong = new ThreadSafeLong(initial);
        tslong.Add(added);
        Assert.Equal(expected, tslong.Value);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(2, 10, 10)]
    [InlineData(10, 2, 10)]
    [InlineData(-10, 10, 10)]
    [InlineData(-10, -2, -2)]
    [InlineData(-10, -20, -10)]
    public void CanIncToValue(long initial, long value, long expected)
    {
        var tslong = new ThreadSafeLong(initial);
        tslong.IncTo(value);
        Assert.Equal(expected, tslong.Value);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(2, 10, 2)]
    [InlineData(10, 2, 2)]
    [InlineData(-10, 10, -10)]
    [InlineData(-10, -2, -10)]
    [InlineData(-10, -20, -20)]
    public void CanDecToValue(long initial, long value, long expected)
    {
        var tslong = new ThreadSafeLong(initial);
        tslong.DecTo(value);
        Assert.Equal(expected, tslong.Value);
    }

    [Theory]
    [InlineData(1, 1000)]
    [InlineData(2, 1000)]
    [InlineData(10, 1000)]
    [InlineData(100, 10000)]
    public async Task AddMultiThreads(int threads, int observations)
    {
        var tslong = new ThreadSafeLong(0);
        var testData = new IEnumerable<long>[threads];
        var sum = 0L;

        for (var i = 0; i < threads; i++)
        {
            var data = GenerateTestData(observations);
            sum += data.sum;
            testData[i] = data.items;
        }

        await Task.WhenAll(testData.Select(async d =>
        {
            foreach (long item in d)
            {
                await Task.Yield();
                tslong.Add(item);
            }
        }).ToArray());

        Assert.Equal(sum, tslong.Value);
    }

    [Theory]
    [InlineData(1, 1000)]
    [InlineData(2, 1000)]
    [InlineData(10, 1000)]
    [InlineData(100, 10000)]
    public async Task IncToMultiThreads(int threads, int observations)
    {
        var tslong = new ThreadSafeLong(0);
        var testData = new IEnumerable<long>[threads];
        var max = long.MinValue;

        for (var i = 0; i < threads; i++)
        {
            var data = GenerateTestData(observations);
            max = Math.Max(data.max, max);
            testData[i] = data.items;
        }

        await Task.WhenAll(testData.Select(async d =>
        {
            foreach (long item in d)
            {
                await Task.Yield();
                tslong.IncTo(item);
            }
        }).ToArray());

        Assert.Equal(max, tslong.Value);
    }

    [Theory]
    [InlineData(1, 1000)]
    [InlineData(2, 1000)]
    [InlineData(10, 1000)]
    [InlineData(100, 10000)]
    public async Task DecToMultiThreads(int threads, int observations)
    {
        var tslong = new ThreadSafeLong(0);
        var testData = new IEnumerable<long>[threads];
        var min = long.MaxValue;

        for (var i = 0; i < threads; i++)
        {
            var data = GenerateTestData(observations);
            min = Math.Min(data.min, min);
            testData[i] = data.items;
        }

        await Task.WhenAll(testData.Select(async d =>
        {
            foreach (long item in d)
            {
                await Task.Yield();
                tslong.DecTo(item);
            }
        }).ToArray());

        Assert.Equal(min, tslong.Value);
    }

    private (long sum, long min, long max, IEnumerable<long> items) GenerateTestData(int observations)
    {
        var rnd = new Random();
        var items = new long[observations];
        var sum = 0L;
        var min = long.MaxValue;
        var max = long.MinValue;

        for (var i = 0; i < observations; i++)
        {
            items[i] = rnd.Next(-10000, 10000);
            sum += items[i];
            min = Math.Min(items[i], min);
            max = Math.Max(items[i], max);
        }

        return (sum, min, max, items);
    }
}
