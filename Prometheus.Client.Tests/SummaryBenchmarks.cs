using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Prometheus.Advanced.DataContracts;
using Prometheus.Client.Internal;
using Xunit;

namespace Prometheus.Client.Tests
{
    /* todo: move to benchmark tests
    public class SummaryBenchmarks
    {
        [Theory]
        [ClassData(typeof(ObserveTestCases))]
        public void BenchmarkSummaryObserve(int w)
        {
            var stopwatch = new Stopwatch();

            const int N = 100000;
            var summary = new Summary("test_summary", "helpless", new string[0]);
            var tasks = new Task[w];

            stopwatch.Start();
            for (var i = 0; i < w; i++)
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < N; j++)
                        summary.Observe(j);
                });

            Task.WaitAll(tasks);
            stopwatch.Stop();

            Console.WriteLine($"{w} tasks doing  {N} observations took {stopwatch.Elapsed.TotalMilliseconds} milliseconds");
        }

        [Theory]
        [ClassData(typeof(WriteTestCases))]
        public void BencharmSummaryWrite(int w)
        {
            var stopwatch = new Stopwatch();

            var summary = new Summary("test_summary", "helpless", new string[0]);
            var child = new Summary.Child();
            var now = new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            child.Init(summary, LabelValues.Empty, now);

            const int N = 1000;

            for (var obsNum = 0; obsNum < 1000000; obsNum++)
                child.Observe(obsNum, now);

            stopwatch.Start();
            var tasks = new Task[w];
            for (var taskNum = 0; taskNum < w; taskNum++)
            {
                var metric = new Metric();

                tasks[taskNum] = Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < N; i++)
                        child.Populate(metric, now);
                });
            }

            Task.WaitAll(tasks);
            stopwatch.Stop();

            Console.WriteLine($"{w} tasks doing {N} writes took {stopwatch.Elapsed.TotalMilliseconds} milliseconds");
        }

        public class ObserveTestCases : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {1},
                new object[] {2},
                new object[] {4},
                new object[] {8}
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _data.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class WriteTestCases : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {1},
                new object[] {2},
                new object[] {4},
                new object[] {8}
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _data.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
    */
}