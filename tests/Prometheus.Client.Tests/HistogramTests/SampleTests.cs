using System;
using System.Collections.Generic;
using System.Linq;
using Prometheus.Client.Abstractions;
using Xunit;

namespace Prometheus.Client.Tests.HistogramTests
{
    public class SampleTests : MetricTestBase
    {
        [Theory]
        [MemberData(nameof(SumTestCases))]
        public void ShouldPopulateSumOnObservations(IReadOnlyList<double> items, double expectedSum)
        {
            var histogram = CreateHistogram();

            foreach (var item in items)
            {
                histogram.Observe(item);
            }

            Assert.Equal(expectedSum,histogram.Value.Sum);
        }

        [Theory]
        [MemberData(nameof(CountTestCases))]
        public void ShouldPopulateCountOnObservations(IReadOnlyList<double> items, int expectedCount)
        {
            var histogram = CreateHistogram();

            foreach (var item in items)
            {
                histogram.Observe(item);
            }

            Assert.Equal(expectedCount,histogram.Value.Count);
        }

        [Theory]
        [MemberData(nameof(BucketsTestCases))]
        public void ShouldPopulateBucketsOnObservations(IReadOnlyList<double> buckets, IReadOnlyList<double> items, IReadOnlyList<KeyValuePair<double, long>> expectedBuckets)
        {
            var histogram = CreateHistogram(buckets);

            foreach (var item in items)
            {
                histogram.Observe(item);
            }

            Assert.Equal(expectedBuckets, histogram.Value.Buckets);
        }

        public static IEnumerable<object[]> SumTestCases()
            => HistogramTestCases().Select(test => new object[] { test.Items, test.Sum });

        public static IEnumerable<object[]> CountTestCases()
            => HistogramTestCases().Select(test => new object[] { test.Items, test.Items.Count });

        public static IEnumerable<object[]> BucketsTestCases()
            => HistogramTestCases().Select(test => new object[] { test.Buckets, test.Items, MakeExpectedBuckets(test.Buckets, test.BucketsData) });

        private static IEnumerable<(IReadOnlyList<double> Items, double Sum, IReadOnlyList<double> Buckets, IReadOnlyList<long> BucketsData)> HistogramTestCases()
        {
            yield return (new double[0], 0, new double[] {-1, 0, 1}, new long[] {0, 0, 0, 0});
            yield return (new double[] {0}, 0, new double[] {-1, 0, 1}, new long[] {0, 1, 1, 1});
            yield return (new double[] { 1, -1 }, 0, new double[] { -5, 0, 5 }, new long[] { 0, 1, 2, 2});
            yield return (new double[] { 1, -1, 1.3 }, 1.3, new double[] { -5, 0, 5 }, new long[] { 0, 1, 3, 3});
            yield return (new double[] { 1, -1 , 100}, 100, new double[] { -5, 0, 5 }, new long[] { 0, 1, 2, 3});
            yield return (new double[] { 1, -1 , -100}, -100, new double[] { -5, 0, 5 }, new long[] { 1, 2, 3, 3});
        }

        private static IReadOnlyList<KeyValuePair<double, long>> MakeExpectedBuckets(IReadOnlyList<double> buckets, IReadOnlyList<long> data)
        {
            return buckets.Union(new[] {double.PositiveInfinity})
                .Zip(data, KeyValuePair.Create)
                .ToList();
        }

        private IHistogram CreateHistogram(IReadOnlyList<double> buckets = null, MetricFlags options = MetricFlags.Default)
        {
            var config = new HistogramConfiguration("test", string.Empty, Array.Empty<string>(), buckets, options);
            return new Histogram(config, Array.Empty<string>());
        }
    }
}
