using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prometheus.Client.Collectors;
using Prometheus.Client.SummaryImpl;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class SummaryTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1000000)]
        [InlineData(10000)]
        public void TestSummaryConcurrency(int n)
        {
            var random = new Random(42);
            int mutations = (n % 10000) + 10000;
            int concLevel = (n % 5) + 1;
            int total = mutations * concLevel;

            var sum = new Summary(new Summary.SummaryConfiguration("test_summary", "helpless", false, new string[0]));
            var allVars = new double[total];
            double sampleSum = 0;
            var tasks = new List<Task>();

            for (int i = 0; i < concLevel; i++)
            {
                var vals = new double[mutations];
                for (int j = 0; j < mutations; j++)
                {
                    double v = random.NormDouble();
                    vals[j] = v;
                    allVars[(i * mutations) + j] = v;
                    sampleSum += v;
                }

                tasks.Add(Task.Run(() =>
                {
                    foreach (double v in vals)
                        sum.Observe(v);
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Array.Sort(allVars);

            var m = sum.Unlabelled.ForkState(DateTime.Now);

            Assert.Equal(mutations * concLevel, (int) m.Count);

            double got = m.Sum;
            double want = sampleSum;

            Assert.True(Math.Abs(got - want) / want <= 0.001);

            var objectives = Summary.SummaryConfiguration.DefaultObjectives.Select(_ => _.Quantile).ToArray();
            Array.Sort(objectives);

            for (int i = 0; i < objectives.Length; i++)
            {
                var wantQ = Summary.SummaryConfiguration.DefaultObjectives.ElementAt(i);
                double epsilon = wantQ.Epsilon;
                double gotQ = m.Values[i].Key;
                double gotV = m.Values[i].Value;
                var minMax = GetBounds(allVars, wantQ.Quantile, epsilon);

                Assert.False(double.IsNaN(gotQ));
                Assert.False(double.IsNaN(gotV));
                Assert.False(double.IsNaN(minMax.Item1));
                Assert.False(double.IsNaN(minMax.Item2));

                Assert.Equal(wantQ.Quantile, gotQ);
                Assert.True(gotV >= minMax.Item1);
                Assert.True(gotV <= minMax.Item2);
            }
        }

        private static Tuple<double, double> GetBounds(double[] vars, double q, double epsilon)
        {
            // TODO: This currently tolerates an error of up to 2*ε. The error must
            // be at most ε, but for some reason, it's sometimes slightly
            // higher. That's a bug.
            double n = vars.Length;
            int lower = (int) ((q - (2 * epsilon)) * n);
            int upper = (int) Math.Ceiling((q + (2 * epsilon)) * n);

            double min = vars[0];
            if (lower > 1)
                min = vars[lower - 1];

            double max = vars[vars.Length - 1];
            if (upper < vars.Length)
                max = vars[upper - 1];

            return new Tuple<double, double>(min, max);
        }

        [Fact]
        public void TestSummary()
        {
            var summary = Metrics.CreateSummary("Summary", "helpless", "labelName").Labels("labelValue");

            // Default objectives are 0.5, 0.9, 0.99 quantile
            const int numIterations = 1000;
            const int numObservations = 100;

            int expectedSum = 0;
            for (int iteration = 0; iteration < numIterations; iteration++)
            {
                // 100 observations from 0 to 99
                for (int observation = 0; observation < numObservations; observation++)
                {
                    summary.Observe(observation);
                    expectedSum += observation;
                }
            }

            var state = summary.ForkState(DateTime.Now);

            Assert.Equal(numObservations * numIterations, (int) state.Count);
            Assert.Equal(expectedSum, state.Sum);

            Assert.True(state.Values.Single(_ => _.Key.Equals(0.5)).Value >= 50 - 2);
            Assert.True(state.Values.Single(_ => _.Key.Equals(0.5)).Value <= 50 + 2);

            Assert.True(state.Values.Single(_ => _.Key.Equals(0.9)).Value >= 90 - 2);
            Assert.True(state.Values.Single(_ => _.Key.Equals(0.9)).Value <= 90 + 2);

            Assert.True(state.Values.Single(_ => _.Key.Equals(0.99)).Value >= 99 - 2);
            Assert.True(state.Values.Single(_ => _.Key.Equals(0.99)).Value <= 99 + 2);
        }

        [Fact]
        public void TestSummaryDecay()
        {
            var baseTime = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var summaryConfig = new Summary.SummaryConfiguration("test_summary", "helpless", false, new string[0],
                new List<QuantileEpsilonPair> { new QuantileEpsilonPair(0.1d, 0.001d) }, TimeSpan.FromSeconds(100), 10);
            var child = new Summary.LabelledSummary();
            child.Init(LabelValues.Empty, summaryConfig, baseTime);

            for (int i = 0; i < 1000; i++)
            {
                var now = baseTime.AddSeconds(i);
                child.Observe(i, null, now);

                if (i % 10 == 0)
                {
                    var state = child.ForkState(now);
                    double got = state.Values[0].Value;
                    double want = Math.Max((double) i / 10, (double) i - 90);

                    Assert.True(Math.Abs(got - want) <= 1, $"{i}. got {got} want {want}");
                }
            }

            // Wait for MaxAge without observations and make sure quantiles are NaN.
            var newState = child.ForkState(baseTime.AddSeconds(1000).AddSeconds(100));
            Assert.True(double.IsNaN(newState.Values[0].Value));
        }
    }
}
