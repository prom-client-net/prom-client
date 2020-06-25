extern alias Their;
using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Histogram
{
    [MemoryDiagnoser]
    public class HistogramCollectionBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 100;
        private const int _labelsCount = 5;
        private const int _variantsCount = 100;
        private const int _observationsCount = 100;

        private const string _helpText = "some help text";

        public HistogramCollectionBenchmarks()
        {
            var labelNames = GenerateLabelNames(_labelsCount).ToArray();
            var labelVariants = GenerateLabels(_variantsCount, _labelsCount);
            var rnd = new Random();

            foreach (var metric in GenerateMetrics(_metricsCount))
            {
                var ourMetric = OurMetricFactory.CreateHistogram(metric, _helpText, labelNames);
                var theirMetric = TheirMetricFactory.CreateHistogram(metric, _helpText, labelNames);

                foreach (var labels in labelVariants)
                {
                    for (var i = 0; i < _observationsCount; i++)
                    {
                        var val = rnd.Next(10);
                        ourMetric.WithLabels(labels).Observe(val);
                        theirMetric.WithLabels(labels).Observe(val);
                    }
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
