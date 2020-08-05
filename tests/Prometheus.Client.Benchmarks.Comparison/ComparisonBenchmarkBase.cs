extern alias Their;
using System;
using System.Linq;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Benchmarks.Comparison
{
    public abstract class ComparisonBenchmarkBase
    {
        private IMetricFactory _factory;
        private ICollectorRegistry _registry;
        private Their.Prometheus.CollectorRegistry _theirRegistry;
        private Their.Prometheus.MetricFactory _theirMetricFactory;

        protected const string HelpText = "arbitrary help message for metric, not relevant for benchmarking";

        protected ComparisonBenchmarkBase()
        {
            ResetFactories();
        }

        protected void ResetFactories()
        {
            _registry = new CollectorRegistry();
            _factory = new MetricFactory(_registry);

            _theirRegistry = Their.Prometheus.Metrics.NewCustomRegistry();
            _theirMetricFactory = Their.Prometheus.Metrics.WithCustomRegistry(_theirRegistry);

            GC.Collect();
        }

        protected string[] GenerateMetricNames(int count, double duplicates = 0)
        {
            return GenerateData(count, duplicates, n => $"metric_{n}");
        }

        protected string[] GenerateLabelNames(int count)
        {
            var result = new string[count];
            for (var i = 0; i < count; i++)
                result[i] = $"label_{i}";

            return result;
        }

        protected string[][] GenerateLabelValues(int count, int labels, double duplicates = 0)
        {
            var labelsRange = Enumerable.Range(1, labels).ToArray();

            return GenerateData(count, duplicates, n => labelsRange.Select(i => $"label{n}_variant{i}").ToArray());
        }

        protected IMetricFactory OurMetricFactory => _factory;

        protected ICollectorRegistry OurCollectorRegistry => _registry;

        public Their::Prometheus.MetricFactory TheirMetricFactory => _theirMetricFactory;

        protected Their::Prometheus.CollectorRegistry TheirCollectorRegistry => _theirRegistry;

        private T[] GenerateData<T>(int count, double duplicates, Func<int, T> valueGenerator)
        {
            var groupWidth = 1;
            if (duplicates > 0)
                groupWidth = (int)Math.Ceiling(count * duplicates);

            var result = new T[count];
            var groupNum = 1;
            var i = 0;

            while (i < result.Length)
            {
                var currentWidth = Math.Min(groupWidth, result.Length - i);
                Array.Fill(result, valueGenerator(groupNum), i, currentWidth);

                i += currentWidth;
                groupNum++;
            }

            Shuffle(result);

            return result;
        }

        private static void Shuffle<T>(T[] array)
        {
            var r = new Random();

            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = r.Next(0, i + 1);

                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
    }
}
