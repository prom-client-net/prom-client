extern alias Their;
using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    [MemoryDiagnoser]
    public class CounterCollectionBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 100;
        private const int _labelsCount = 5;
        private const int _variantsCount = 100;

        private const string _helpText = "some help text";

        public CounterCollectionBenchmarks()
        {
            var labelNames = GenerateLabelNames(_labelsCount).ToArray();
            var labelVariants = GenerateLabels(_variantsCount, _labelsCount);
            var rnd = new Random();

            foreach (var metric in GenerateMetrics(_metricsCount))
            {
                var ourMetric = OurMetricFactory.CreateCounter(metric, _helpText, labelNames);
                var theirMetric = TheirMetricFactory.CreateCounter(metric, _helpText, labelNames);

                foreach (var labels in labelVariants)
                {
                    var val = rnd.Next(10000);
                    ourMetric.WithLabels(labels).Inc(val);
                    theirMetric.WithLabels(labels).Inc(val);
                }
            }
        }

        [Benchmark(Baseline = true)]
        public void Collection_BaseLine()
        {
            using (var stream = Stream.Null)
            {
                TheirCollectorRegistry.CollectAndExportAsTextAsync(stream, default).GetAwaiter().GetResult();
            }
        }

        [Benchmark]
        public void Collection()
        {
            using (var stream = Stream.Null)
            {
                ScrapeHandler.ProcessAsync(OurCollectorRegistry , stream).GetAwaiter().GetResult();
            }
        }
    }
}
