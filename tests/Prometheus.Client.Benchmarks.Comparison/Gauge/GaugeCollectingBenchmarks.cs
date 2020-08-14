extern alias Their;
using System;
using System.IO;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Gauge
{
    public class GaugeCollectingBenchmarks : ComparisonBenchmarkBase
    {
        private const int _metricsCount = 100;
        private const int _labelsCount = 5;
        private const int _variantsCount = 100;

        private const string _helpText = "some help text";

        public GaugeCollectingBenchmarks()
        {
            var labelNames = GenerateLabelNames(_labelsCount);
            var labelVariants = GenerateLabelValues(_variantsCount, _labelsCount);
            var rnd = new Random();

            foreach (var metric in GenerateMetricNames(_metricsCount))
            {
                var ourMetric = OurMetricFactory.CreateGauge(metric, _helpText, labelNames);
                var theirMetric = TheirMetricFactory.CreateGauge(metric, _helpText, labelNames);

                foreach (var labels in labelVariants)
                {
                    var val = rnd.Next(10000);
                    ourMetric.WithLabels(labels).Inc(val);
                    theirMetric.WithLabels(labels).Inc(val);
                }
            }
        }

        [Benchmark(Baseline = true)]
        public void Collecting_Baseline()
        {
            using var stream = Stream.Null;
            TheirCollectorRegistry.CollectAndExportAsTextAsync(stream).GetAwaiter().GetResult();
        }

        [Benchmark]
        public void Collecting()
        {
            using var stream = Stream.Null;
            ScrapeHandler.ProcessAsync(OurCollectorRegistry , stream).GetAwaiter().GetResult();
        }
    }
}
