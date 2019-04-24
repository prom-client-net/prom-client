using System.IO;
using BenchmarkDotNet.Attributes;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks
{
    [MemoryDiagnoser]
    public class SerializationBenchmarks
    {
        // Metric -> Variant -> Label values
        private static readonly string[][][] _labelValueRows;

        private const int _metricCount = 100;
        private const int _variantCount = 100;
        private const int _labelCount = 5;

        private const string _help = "arbitrary help message for metric, not relevant for benchmarking";

        static SerializationBenchmarks()
        {
            _labelValueRows = new string[_metricCount][][];

            for (var metricIndex = 0; metricIndex < _metricCount; metricIndex++)
            {
                var variants = new string[_variantCount][];
                _labelValueRows[metricIndex] = variants;

                for (var variantIndex = 0; variantIndex < _variantCount; variantIndex++)
                {
                    var values = new string[_labelCount];
                    _labelValueRows[metricIndex][variantIndex] = values;

                    for (var labelIndex = 0; labelIndex < _labelCount; labelIndex++)
                        values[labelIndex] = $"metric{metricIndex:D2}_label{labelIndex:D2}_variant{variantIndex:D2}";
                }
            }
        }

        private readonly CollectorRegistry _registry = new CollectorRegistry();
        private readonly Prometheus.Client.Counter[] _counters;
        private readonly Prometheus.Client.Gauge[] _gauges;
        private readonly Prometheus.Client.Summary[] _summaries;
        private readonly Prometheus.Client.Histogram[] _histograms;

        public SerializationBenchmarks()
        {
            _counters = new Prometheus.Client.Counter[_metricCount];
            _gauges = new Prometheus.Client.Gauge[_metricCount];
            _summaries = new Prometheus.Client.Summary[_metricCount];
            _histograms = new Prometheus.Client.Histogram[_metricCount];

            var factory = new MetricFactory(_registry);

            // Just use 1st variant for the keys (all we care about are that there is some name-like value in there).
            for (var metricIndex = 0; metricIndex < _metricCount; metricIndex++)
            {
                _counters[metricIndex] = factory.CreateCounter($"counter{metricIndex:D2}", _help, _labelValueRows[metricIndex][0]);
                _gauges[metricIndex] = factory.CreateGauge($"gauge{metricIndex:D2}", _help, _labelValueRows[metricIndex][0]);
                _summaries[metricIndex] = factory.CreateSummary($"summary{metricIndex:D2}", _help, _labelValueRows[metricIndex][0]);
                _histograms[metricIndex] = factory.CreateHistogram($"histogram{metricIndex:D2}", _help, _labelValueRows[metricIndex][0]);
            }
        }

        [GlobalSetup]
        public void GenerateData()
        {
            for (var metricIndex = 0; metricIndex < _metricCount; metricIndex++)
            {
                for (var variantIndex = 0; variantIndex < _variantCount; variantIndex++)
                {
                    _counters[metricIndex].Labels(_labelValueRows[metricIndex][variantIndex]).Inc();
                    _gauges[metricIndex].Labels(_labelValueRows[metricIndex][variantIndex]).Inc();
                    _summaries[metricIndex].Labels(_labelValueRows[metricIndex][variantIndex]).Observe(variantIndex);
                    _histograms[metricIndex].Labels(_labelValueRows[metricIndex][variantIndex]).Observe(variantIndex);
                }
            }
        }

        [Benchmark]
        public void CollectAndSerialize()
        {
            using (var stream = Stream.Null)
            {
                ScrapeHandler.ProcessAsync(_registry, stream).GetAwaiter().GetResult();
            }
        }
    }
}
