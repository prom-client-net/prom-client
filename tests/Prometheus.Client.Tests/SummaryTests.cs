using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prometheus.Client.Internal;
using Prometheus.Client.SummaryImpl;
using Prometheus.Contracts;
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
            var mutations = n % 10000 + 10000;
            var concLevel = n % 5 + 1;
            var total = mutations * concLevel;

            var sum = new Summary("test_summary", "helpless", new string[0]);
            var allVars = new double[total];
            double sampleSum = 0;
            var tasks = new List<Task>();

            for (var i = 0; i < concLevel; i++)
            {
                var vals = new double[mutations];
                for (var j = 0; j < mutations; j++)
                {
                    var v = random.NormDouble();
                    vals[j] = v;
                    allVars[i * mutations + j] = v;
                    sampleSum += v;
                }

                tasks.Add(Task.Run(() =>
                {
                    foreach (var v in vals)
                        sum.Observe(v);
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Array.Sort(allVars);

            var m = sum.Collect().metric.Single().summary;

            Assert.Equal(mutations * concLevel, (int) m.sample_count);

            var got = m.sample_sum;
            var want = sampleSum;

            Assert.True(Math.Abs(got - want) / want <= 0.001);

            var objectives = Summary.DefObjectives.Select(_ => _.Quantile).ToArray();
            Array.Sort(objectives);

            for (var i = 0; i < objectives.Length; i++)
            {
                var wantQ = Summary.DefObjectives.ElementAt(i);
                var epsilon = wantQ.Epsilon;
                var gotQ = m.quantile[i].quantile;
                var gotV = m.quantile[i].value;
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

        [Fact]
        public void TestSummaryDecay()
        {
            var baseTime = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var sum = new Summary("test_summary", "helpless", new string[0], objectives: new List<QuantileEpsilonPair> {new QuantileEpsilonPair(0.1d, 0.001d)}, maxAge: TimeSpan.FromSeconds(100), ageBuckets: 10);
            var child = new Summary.ThisChild();
            child.Init(sum, LabelValues.Empty, baseTime);

            Contracts.Summary m;
            var metric = new Metric();

            for (var i = 0; i < 1000; i++)
            {
                var now = baseTime.AddSeconds(i);
                child.Observe(i, now);

                if (i % 10 == 0)
                {
                    child.Populate(metric, now);
                    m = metric.summary;
                    var got = m.quantile[0].value;
                    var want = Math.Max((double) i / 10, (double) i - 90);

                    Assert.True(Math.Abs(got - want) <= 1, $"{i}. got {got} want {want}");
                }
            }

            // Wait for MaxAge without observations and make sure quantiles are NaN.
            child.Populate(metric, baseTime.AddSeconds(1000).AddSeconds(100));
            m = metric.summary;

            Assert.True(double.IsNaN(m.quantile[0].value));
        }

        [Fact]
        public void TestSummary()
        {
            var summary = Metrics.CreateSummary("Summary", "helpless", "labelName").Labels("labelValue");

            // Default objectives are 0.5, 0.9, 0.99 quantile
            const int numIterations = 1000;
            const int numObservations = 100;

            var expectedSum = 0;
            for (var iteration = 0; iteration < numIterations; iteration++)
            {
                // 100 observations from 0 to 99
                for (var observation = 0; observation < numObservations; observation++)
                {
                    summary.Observe(observation);
                    expectedSum += observation;
                }
            }
            var metric = new Metric();
            summary.Populate(metric, DateTime.UtcNow);
            var m = metric.summary;

            Assert.Equal(numObservations * numIterations, (int) m.sample_count);
            Assert.Equal(expectedSum, m.sample_sum);

            Assert.True(m.quantile.Single(_ => _.quantile.Equals(0.5)).value >= 50 - 2);
            Assert.True(m.quantile.Single(_ => _.quantile.Equals(0.5)).value <= 50 + 2);

            Assert.True(m.quantile.Single(_ => _.quantile.Equals(0.9)).value >= 90 - 2);
            Assert.True(m.quantile.Single(_ => _.quantile.Equals(0.9)).value <= 90 + 2);

            Assert.True(m.quantile.Single(_ => _.quantile.Equals(0.99)).value >= 99 - 2);
            Assert.True(m.quantile.Single(_ => _.quantile.Equals(0.99)).value <= 99 + 2);
        }

        static Tuple<double, double> GetBounds(double[] vars, double q, double epsilon)
        {
            // TODO: This currently tolerates an error of up to 2*ε. The error must
            // be at most ε, but for some reason, it's sometimes slightly
            // higher. That's a bug.
            var n = (double) vars.Length;
            var lower = (int) ((q - 2 * epsilon) * n);
            var upper = (int) Math.Ceiling((q + 2 * epsilon) * n);

            var min = vars[0];
            if (lower > 1)
            {
                min = vars[lower - 1];
            }

            var max = vars[vars.Length - 1];
            if (upper < vars.Length)
            {
                max = vars[upper - 1];
            }

            return new Tuple<double, double>(min, max);
        }
    }
}