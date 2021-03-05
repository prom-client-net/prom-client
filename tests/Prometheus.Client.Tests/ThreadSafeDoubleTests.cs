using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class ThreadSafeDoubleTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(10)]
        [InlineData(-3.14)]
        [InlineData(3.14)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.Epsilon)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NaN)]
        public void CanInitConstructor(double value)
        {
            var tsdouble = new ThreadSafeDouble(value);
            Assert.Equal(value, tsdouble.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(10)]
        [InlineData(-3.14)]
        [InlineData(3.14)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.Epsilon)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NaN)]
        public void CanSetValue(double value)
        {
            var tsdouble = new ThreadSafeDouble(0);
            tsdouble.Value = value;
            Assert.Equal(value, tsdouble.Value);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(-10, false)]
        [InlineData(10, false)]
        [InlineData(double.MinValue, false)]
        [InlineData(double.MaxValue, false)]
        [InlineData(double.Epsilon, false)]
        [InlineData(double.NegativeInfinity, false)]
        [InlineData(double.PositiveInfinity, false)]
        [InlineData(double.NaN, true)]
        public void IsNaN(double value, bool result)
        {
            var isNaN = ThreadSafeDouble.IsNaN(value);
            Assert.Equal(result, isNaN);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(-10, 10, 0)]
        [InlineData(3.10, 2, 5.10)]
        public void CanAddValue(double initial, double added, double expected)
        {
            var tsdouble = new ThreadSafeDouble(initial);
            tsdouble.Add(added);
            Assert.Equal(expected, tsdouble.Value);
        }

        [Theory]
        [InlineData(double.NaN, 0)]
        [InlineData(0, double.NaN)]
        [InlineData(3.14, double.NaN)]
        [InlineData(double.NaN, double.NaN)]
        public void AddThrowsOnNaN(double initial, double added)
        {
            var tsdouble = new ThreadSafeDouble(initial);
            Assert.Throws<InvalidOperationException>(() => tsdouble.Add(added));
        }

        [Theory]
        [InlineData(double.NaN, 0)]
        [InlineData(0, double.NaN)]
        [InlineData(3.14, double.NaN)]
        [InlineData(double.NaN, double.NaN)]
        public void IncToThrowsOnNaN(double initial, double value)
        {
            var tsdouble = new ThreadSafeDouble(initial);
            Assert.Throws<InvalidOperationException>(() => tsdouble.IncTo(value));
        }

        [Theory]
        [InlineData(double.NaN, 0)]
        [InlineData(0, double.NaN)]
        [InlineData(3.14, double.NaN)]
        [InlineData(double.NaN, double.NaN)]
        public void DecToThrowsOnNaN(double initial, double value)
        {
            var tsdouble = new ThreadSafeDouble(initial);
            Assert.Throws<InvalidOperationException>(() => tsdouble.DecTo(value));
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(2, 10, 10)]
        [InlineData(10, 2, 10)]
        [InlineData(-10, 10, 10)]
        [InlineData(-10, -2, -2)]
        [InlineData(-10, -20, -10)]
        public void CanIncToValue(double initial, double value, double expected)
        {
            var tslong = new ThreadSafeDouble(initial);
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
        public void CanDecToValue(double initial, double value, double expected)
        {
            var tslong = new ThreadSafeDouble(initial);
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
            var tslong = new ThreadSafeDouble(0);
            var testData = new IEnumerable<double>[threads];
            var sum = 0D;

            for (var i = 0; i < threads; i++)
            {
                var data = GenerateTestData(observations);
                sum += data.sum;
                testData[i] = data.items;
            }

            await Task.WhenAll(testData.Select(async d =>
            {
                foreach (double item in d)
                {
                    await Task.Yield();
                    tslong.Add(item);
                }
            }).ToArray());

            Assert.Equal(Math.Round(sum, 5), Math.Round(tslong.Value, 5));
        }

        [Theory]
        [InlineData(1, 1000)]
        [InlineData(2, 1000)]
        [InlineData(10, 1000)]
        [InlineData(100, 10000)]
        public async Task IncToMultiThreads(int threads, int observations)
        {
            var tslong = new ThreadSafeDouble(0);
            var testData = new IEnumerable<double>[threads];
            var max = double.MinValue;

            for (var i = 0; i < threads; i++)
            {
                var data = GenerateTestData(observations);
                max = Math.Max(data.max, max);
                testData[i] = data.items;
            }

            await Task.WhenAll(testData.Select(async d =>
            {
                foreach (double item in d)
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
            var tslong = new ThreadSafeDouble(0);
            var testData = new IEnumerable<double>[threads];
            var min = double.MaxValue;

            for (var i = 0; i < threads; i++)
            {
                var data = GenerateTestData(observations);
                min = Math.Min(data.min, min);
                testData[i] = data.items;
            }

            await Task.WhenAll(testData.Select(async d =>
            {
                foreach (double item in d)
                {
                    await Task.Yield();
                    tslong.DecTo(item);
                }
            }).ToArray());

            Assert.Equal(min, tslong.Value);
        }

        private (double sum, double min, double max, IEnumerable<double> items) GenerateTestData(int observations)
        {
            var rnd = new Random();
            var items = new double[observations];
            var sum = 0D;
            var min = double.MaxValue;
            var max = double.MinValue;

            for (var i = 0; i < observations; i++)
            {
                items[i] = 1000D - (2000D * Math.Round(rnd.NextDouble(), 6));
                sum += items[i];
                min = Math.Min(items[i], min);
                max = Math.Max(items[i], max);
            }

            return (sum, min, max, items);
        }
    }
}
