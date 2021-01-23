extern alias Their;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Summary
{
    public class SummarySampleResolvingBenchmarks : ComparisonBenchmarkBase
    {
        private IMetricFamily<ISummary> _summaryFamily;
        private IMetricFamily<ISummary, (string, string, string, string, string)> _summaryTuplesFamily;
        private Their.Prometheus.Summary _theirSummary;

        private string[][] _labels;

        [GlobalSetup]
        public void Setup()
        {
            _labels = GenerateLabelValues(10_000, 5, 0.1);

            _summaryTuplesFamily = OurMetricFactory.CreateSummary("_summaryFamilyTuples", string.Empty, ("label1", "label2", "label3", "label4", "label5" ));
            _summaryFamily = OurMetricFactory.CreateSummary("_summaryFamily", string.Empty, "label1", "label2", "label3", "label4", "label5");
            _theirSummary = TheirMetricFactory.CreateSummary("_summary", string.Empty, "label1", "label2", "label3", "label4", "label5");

            foreach (var lbls in _labels)
            {
                _theirSummary.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
                _summaryFamily.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
                _summaryTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]));
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Baseline()
        {
            foreach (var lbls in _labels)
                _theirSummary.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Array()
        {
            foreach (var lbls in _labels)
                _summaryFamily.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Tuple()
        {
            foreach (var lbls in _labels)
                _summaryTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]));
        }
    }
}
