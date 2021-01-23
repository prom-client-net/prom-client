extern alias Their;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Counter
{
    public class CounterSampleResolvingBenchmarks : ComparisonBenchmarkBase
    {
        private IMetricFamily<ICounter> _counterFamily;
        private IMetricFamily<ICounter, (string, string, string, string, string)> _counterTuplesFamily;
        private IMetricFamily<ICounter<long>> _counterInt64Family;
        private IMetricFamily<ICounter<long>, (string, string, string, string, string)> _counterInt64TuplesFamily;
        private Their.Prometheus.Counter _theirCounter;

        private string[][] _labels;

        [GlobalSetup]
        public void Setup()
        {
            _labels = GenerateLabelValues(10_000, 5, 0.1);

            _counterTuplesFamily = OurMetricFactory.CreateCounter("_counterFamilyTuples", string.Empty, ("label1", "label2", "label3", "label4", "label5" ));
            _counterFamily = OurMetricFactory.CreateCounter("_counterFamily", string.Empty, "label1", "label2", "label3", "label4", "label5");
            _counterInt64TuplesFamily = OurMetricFactory.CreateCounterInt64("_counterInt64FamilyTuples", string.Empty, ("label1", "label2", "label3", "label4", "label5" ));
            _counterInt64Family = OurMetricFactory.CreateCounterInt64("_counterInt64Family", string.Empty, "label1", "label2", "label3", "label4", "label5");
            _theirCounter = TheirMetricFactory.CreateCounter("_counter", string.Empty, "label1", "label2", "label3", "label4", "label5");
            
            foreach (var lbls in _labels)
            {
                _theirCounter.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
                _counterFamily.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
                _counterTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]));
                _counterInt64Family.WithLabels(lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]);
                _counterInt64TuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2],lbls[3],lbls[4]));
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Baseline()
        {
            foreach (var lbls in _labels)
                _theirCounter.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Array()
        {
            foreach (var lbls in _labels)
                _counterFamily.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Tuple()
        {
            foreach (var lbls in _labels)
                _counterTuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]));
        }

        [Benchmark]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Int64Array()
        {
            foreach (var lbls in _labels)
                _counterInt64Family.WithLabels(lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]);
        }

        [Benchmark]
        [BenchmarkCategory("ResolveLabeled")]
        public void ResolveLabeled_Int64Tuple()
        {
            foreach (var lbls in _labels)
                _counterInt64TuplesFamily.WithLabels((lbls[0], lbls[1], lbls[2], lbls[3], lbls[4]));
        }
    }
}
