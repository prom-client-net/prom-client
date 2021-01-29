using System;
using System.Collections.Generic;
using System.Linq;
using Prometheus.Client.HistogramImpl;
using Xunit;

#pragma warning disable SA1313, xUnit1026

namespace Prometheus.Client.Tests.HistogramTests
{
    public class SampleTests
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
        [MemberData(nameof(CountTestCases))]
        public void ResetShouldClearObservations(IReadOnlyList<double> items, int _)
        {
            var histogram = CreateHistogram();

            foreach (var item in items)
            {
                histogram.Observe(item);
            }

            histogram.Reset();

            var state = histogram.Value;
            Assert.Equal(0,state.Count);
            Assert.Equal(0, state.Sum);
            Assert.True(state.Buckets.All(b => b.Value == 0));
        }

        [Theory]
        [MemberData(nameof(BucketsTestCases))]
        public void HistogramLowBucketsStore(double[] buckets, IReadOnlyList<double> items, long[] expectedBuckets)
        {
            var backetStore = new HistogramLowBucketsStore(buckets);

            foreach (var item in items)
            {
                backetStore.Observe(item);
            }

            Assert.Equal(expectedBuckets, backetStore.Buckets.Select(b => b.Value));
        }

        [Theory]
        [MemberData(nameof(BucketsTestCases))]
        public void HistogramHighBucketsStore(double[] buckets, IReadOnlyList<double> items, long[] expectedBuckets)
        {
            var backetStore = new HistogramHighBucketsStore(buckets);

            foreach (var item in items)
            {
                backetStore.Observe(item);
            }

            Assert.Equal(expectedBuckets, backetStore.Buckets.Select(b => b.Value));
        }

        [Theory]
        [MemberData(nameof(BucketsTestCases))]
        public void HistogramLowBucketsStoreReset(double[] buckets, IReadOnlyList<double> items, long[] _)
        {
            var expectedBuckets = new long[buckets.Length + 1];
            var backetStore = new HistogramLowBucketsStore(buckets);

            foreach (var item in items)
            {
                backetStore.Observe(item);
            }

            backetStore.Reset();

            Assert.Equal(expectedBuckets, backetStore.Buckets.Select(b => b.Value));
        }

        [Theory]
        [MemberData(nameof(BucketsTestCases))]
        public void HistogramHighBucketsStoreReset(double[] buckets, IReadOnlyList<double> items, long[] _)
        {
            var expectedBuckets = new long[buckets.Length + 1];
            var backetStore = new HistogramHighBucketsStore(buckets);

            foreach (var item in items)
            {
                backetStore.Observe(item);
            }

            backetStore.Reset();

            Assert.Equal(expectedBuckets, backetStore.Buckets.Select(b => b.Value));
        }

        public static IEnumerable<object[]> SumTestCases()
            => HistogramTestCases().Select(test => new object[] { test.Items, test.Sum });

        public static IEnumerable<object[]> CountTestCases()
            => HistogramTestCases().Select(test => new object[] { test.Items, test.Items.Count });

        public static IEnumerable<object[]> BucketsTestCases()
            => HistogramTestCases().Select(test => new object[] { test.Buckets, test.Items, test.BucketsData });

        private static IEnumerable<(IReadOnlyList<double> Items, double Sum, IReadOnlyList<double> Buckets, IReadOnlyList<long> BucketsData)> HistogramTestCases()
        {
            yield return (new double[0], 0, new double[] {-1, 0, 1}, new long[] {0, 0, 0, 0});
            yield return (new double[] {0}, 0, new double[] {-1, 0, 1}, new long[] {0, 1, 0, 0});
            yield return (new double[] { 1, -1 }, 0, new double[] { -5, 0, 5 }, new long[] { 0, 1, 1, 0});
            yield return (new double[] { 1, -1, 1.3 }, 1.3, new double[] { -5, 0, 5 }, new long[] { 0, 1, 2, 0});
            yield return (new double[] { 1, -1 , 100}, 100, new double[] { -5, 0, 5 }, new long[] { 0, 1, 1, 1});
            yield return (new double[] { 1, -1 , -100}, -100, new double[] { -5, 0, 5 }, new long[] { 1, 1, 1, 0});
        }

        private IHistogram CreateHistogram(double[] buckets = null)
        {
            var config = new HistogramConfiguration("test", string.Empty, Array.Empty<string>(), buckets, false);
            return new Histogram(config, Array.Empty<string>());
        }
    }
}

#pragma warning restore SA1313, xUnit1026
