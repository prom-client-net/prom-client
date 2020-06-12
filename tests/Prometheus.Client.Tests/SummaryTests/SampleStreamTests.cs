using System;
using System.Collections.Generic;
using Prometheus.Client.SummaryImpl;
using Xunit;

namespace Prometheus.Client.Tests.SummaryTests
{
    public class QuantileStreamTests
    {
        private readonly IReadOnlyList<QuantileEpsilonPair> _targets = new List<QuantileEpsilonPair>
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

        private static double[] PopulateStream(SampleStream stream, Random random)
        {
            var a = new double[100100];
            for (int i = 0; i < a.Length; i++)
            {
                double v = random.NormDouble();

                // Add 5% asymmetric outliers.
                if (i % 20 == 0)
                    v = (v * v) + 1;

                a[i] = v;
            }

            Array.Sort(a);

            stream.InsertRange(a);

            return a;
        }

        private void VerifyPercsWithAbsoluteEpsilon(double[] a, SampleStream s, IReadOnlyList<QuantileEpsilonPair> targets)
        {
            foreach (var target in targets)
            {
                double n = a.Length;
                int k = (int) (target.Quantile * n);
                if (k < 1)
                    k = 1;

                int lower = (int) ((target.Quantile - target.Epsilon) * n);
                if (lower < 1)
                    lower = 1;

                int upper = (int) Math.Ceiling((target.Quantile + target.Epsilon) * n);
                if (upper > a.Length)
                    upper = a.Length;

                double w = a[k - 1];
                double min = a[lower - 1];
                double max = a[upper - 1];

                double g = s.Query(target.Quantile);

                Assert.True(g >= min, $"q={target.Quantile}: want {w} [{min}, {max}], got {g}");
                Assert.True(g <= max, $"q={target.Quantile}: want {w} [{min}, {max}], got {g}");
            }
        }

        private void VerifyLowPercsWithRelativeEpsilon(double[] a, SampleStream s)
        {
            Array.Sort(a);

            foreach (double qu in _lowQuantiles)
            {
                double n = a.Length;
                int k = (int) (qu * n);

                int lowerRank = (int) ((1 - _relativeEpsilon) * qu * n);
                int upperRank = (int) Math.Ceiling((1 + _relativeEpsilon) * qu * n);

                double w = a[k - 1];
                double min = a[lowerRank - 1];
                double max = a[upperRank - 1];

                double g = s.Query(qu);

                Assert.True(g >= min, $"q={qu}: want {w} [{min}, {max}], got {g}");

                Assert.True(g <= max, $"q={qu}: want {w} [{min}, {max}], got {g}");
            }
        }

        private void VerifyHighPercsWithRelativeEpsilon(double[] a, SampleStream s)
        {
            Array.Sort(a);

            foreach (double qu in _highQuantiles)
            {
                double n = a.Length;
                int k = (int) (qu * n);

                int lowerRank = (int) ((1 - ((1 + _relativeEpsilon) * (1 - qu))) * n);
                int upperRank = (int) Math.Ceiling((1 - ((1 - _relativeEpsilon) * (1 - qu))) * n);
                double w = a[k - 1];
                double min = a[lowerRank - 1];
                double max = a[upperRank - 1];

                double g = s.Query(qu);

                Assert.True(g >= min, $"q={qu}: want {w} [{min}, {max}], got {g}");
                Assert.True(g <= max, $"q={qu}: want {w} [{min}, {max}], got {g}");
            }
        }

        [Fact]
        public void TestDefaults()
        {
            var q = new SampleStream(Invariants.Targeted(new[] { new QuantileEpsilonPair(0.99d, 0.001d) }) );
            double g = q.Query(0.99);
            Assert.Equal(double.NaN, g);
        }

        [Fact]
        public void TestHighBiasedQuery()
        {
            var random = new Random(42);
            var s = new SampleStream(Invariants.HighBiased(_relativeEpsilon) );
            var a = PopulateStream(s, random);
            VerifyHighPercsWithRelativeEpsilon(a, s);
        }

        [Fact]
        public void TestLowBiasedQuery()
        {
            var random = new Random(42);
            var s = new SampleStream(Invariants.LowBiased(_relativeEpsilon) );
            var a = PopulateStream(s, random);
            VerifyLowPercsWithRelativeEpsilon(a, s);
        }

        [Theory]
        //[InlineData(0.01, 0.001, new double[] {1, 2, 5, 5, 6, 7, 9, 10, 11, 11, 12, 13, 13, 13, 15, 16, 17, 18, 19, 19} )]
        [InlineData(0.10, 0.01, new double[] {1, 2, 5, 5, 6, 7, 9, 10, 11, 11, 12, 13, 13, 13, 15, 16, 17, 18, 19, 19} )]
        [InlineData(0.50, 0.05, new double[] {1, 2, 5, 5, 6, 7, 9, 10, 11, 11, 12, 13, 13, 13, 15, 16, 17, 18, 19, 19} )]
        [InlineData(0.90, 0.01, new double[] {1, 2, 5, 5, 6, 7, 9, 10, 11, 11, 12, 13, 13, 13, 15, 16, 17, 18, 19, 19} )]
        [InlineData(0.99, 0.001, new double[] {1, 2, 5, 5, 6, 7, 9, 10, 11, 11, 12, 13, 13, 13, 15, 16, 17, 18, 19, 19} )]
        public void TestTargetedQuery(double quantile, double epsilon, double[] data)
        {
            Array.Sort(data);
            var targets = new[] { new QuantileEpsilonPair(quantile, epsilon) };

            var s = new SampleStream(Invariants.Targeted(targets) );
            s.InsertRange(data);

            VerifyPercsWithAbsoluteEpsilon(data, s, targets);
        }

        [Fact]
        public void TestUncompressedOne()
        {
            var q = new SampleStream(Invariants.Targeted(new[] { new QuantileEpsilonPair(0.99d, 0.001d) }));
            q.InsertRange(new[] { 3.14 });
            double g = q.Query(0.90);
            Assert.Equal(3.14, g);
        }
    }
}
