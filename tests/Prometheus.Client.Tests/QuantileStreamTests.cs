using System;
using System.Collections.Generic;
using System.Linq;
using Prometheus.Client.SummaryImpl;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class QuantileStreamTests
    {
        private readonly IList<QuantileEpsilonPair> _targets = new List<QuantileEpsilonPair>
        {
            new QuantileEpsilonPair(0.01, 0.001),
            new QuantileEpsilonPair(0.10, 0.01),
            new QuantileEpsilonPair(0.50, 0.05),
            new QuantileEpsilonPair(0.90, 0.01),
            new QuantileEpsilonPair(0.99, 0.001)
        };

        private readonly double[] _lowQuantiles = { 0.01, 0.1, 0.5 };
        private readonly double[] _highQuantiles = { 0.99, 0.9, 0.5 };

        private const double _relativeEpsilon = 0.01;

        private static double[] PopulateStream(QuantileStream stream, Random random)
        {
            var a = new double[100100];
            for (var i = 0; i < a.Length; i++)
            {
                var v = random.NormDouble();

                // Add 5% asymmetric outliers.
                if (i % 20 == 0)
                    v = (v * v) + 1;

                stream.Insert(v);
                a[i] = v;
            }

            return a;
        }

        private void VerifyPercsWithAbsoluteEpsilon(double[] a, QuantileStream s)
        {
            Array.Sort(a);

            foreach (var target in _targets)
            {
                var n = (double) a.Length;
                var k = (int) (target.Quantile * n);
                var lower = (int) ((target.Quantile - target.Epsilon) * n);
                if (lower < 1)
                    lower = 1;
                var upper = (int) Math.Ceiling((target.Quantile + target.Epsilon) * n);
                if (upper > a.Length)
                    upper = a.Length;

                var w = a[k - 1];
                var min = a[lower - 1];
                var max = a[upper - 1];

                var g = s.Query(target.Quantile);

                Assert.True(g >= min, $"q={target.Quantile}: want {w} [{min}, {max}], got {g}");
                Assert.True(g <= max, $"q={target.Quantile}: want {w} [{min}, {max}], got {g}");
            }
        }

        private void VerifyLowPercsWithRelativeEpsilon(double[] a, QuantileStream s)
        {
            Array.Sort(a);

            foreach (var qu in _lowQuantiles)
            {
                var n = (double) a.Length;
                var k = (int) (qu * n);

                var lowerRank = (int) ((1 - _relativeEpsilon) * qu * n);
                var upperRank = (int) Math.Ceiling((1 + _relativeEpsilon) * qu * n);

                var w = a[k - 1];
                var min = a[lowerRank - 1];
                var max = a[upperRank - 1];

                var g = s.Query(qu);

                Assert.True(g >= min, $"q={qu}: want {w} [{min}, {max}], got {g}");

                Assert.True(g <= max, $"q={qu}: want {w} [{min}, {max}], got {g}");
            }
        }

        private void VerifyHighPercsWithRelativeEpsilon(double[] a, QuantileStream s)
        {
            Array.Sort(a);

            foreach (var qu in _highQuantiles)
            {
                var n = (double) a.Length;
                var k = (int) (qu * n);

                var lowerRank = (int) ((1 - ((1 + _relativeEpsilon) * (1 - qu))) * n);
                var upperRank = (int) Math.Ceiling((1 - ((1 - _relativeEpsilon) * (1 - qu))) * n);
                var w = a[k - 1];
                var min = a[lowerRank - 1];
                var max = a[upperRank - 1];

                var g = s.Query(qu);

                Assert.True(g >= min, $"q={qu}: want {w} [{min}, {max}], got {g}");
                Assert.True(g <= max, $"q={qu}: want {w} [{min}, {max}], got {g}");
            }
        }

        [Fact]
        public void TestDefaults()
        {
            var q = QuantileStream.NewTargeted(new List<QuantileEpsilonPair> { new QuantileEpsilonPair(0.99d, 0.001d) });
            var g = q.Query(0.99);
            Assert.Equal(0, g);
        }

        [Fact]
        public void TestHighBiasedQuery()
        {
            var random = new Random(42);
            var s = QuantileStream.NewHighBiased(_relativeEpsilon);
            var a = PopulateStream(s, random);
            VerifyHighPercsWithRelativeEpsilon(a, s);
        }

        [Fact]
        public void TestLowBiasedQuery()
        {
            var random = new Random(42);
            var s = QuantileStream.NewLowBiased(_relativeEpsilon);
            var a = PopulateStream(s, random);
            VerifyLowPercsWithRelativeEpsilon(a, s);
        }

        [Fact]
        public void TestTargetedQuery()
        {
            var random = new Random(42);
            var s = QuantileStream.NewTargeted(_targets);
            var a = PopulateStream(s, random);
            VerifyPercsWithAbsoluteEpsilon(a, s);
        }

        [Fact]
        public void TestUncompressed()
        {
            var q = QuantileStream.NewTargeted(_targets);

            for (var i = 100; i > 0; i--)
                q.Insert(i);

            Assert.Equal(100, q.Count);

            // Before compression, Query should have 100% accuracy
            foreach (var quantile in _targets.Select(_ => _.Quantile))
            {
                var w = quantile * 100;
                var g = q.Query(quantile);
                Assert.Equal(g, w);
            }
        }

        [Fact]
        public void TestUncompressedOne()
        {
            var q = QuantileStream.NewTargeted(new List<QuantileEpsilonPair> { new QuantileEpsilonPair(0.99d, 0.001d) });
            q.Insert(3.14);
            var g = q.Query(0.90);
            Assert.Equal(3.14, g);
        }

        [Fact]
        public void TestUncompressedSamples()
        {
            var q = QuantileStream.NewTargeted(new List<QuantileEpsilonPair> { new QuantileEpsilonPair(0.99d, 0.001d) });

            for (var i = 1; i <= 100; i++)
                q.Insert(i);

            Assert.Equal(100, q.SamplesCount);
        }
    }
}