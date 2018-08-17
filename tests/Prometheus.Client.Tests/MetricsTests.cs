using System;
using System.Linq;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class MetricsTests
    {
        public MetricsTests()
        {
            CollectorRegistry.Instance.Clear();
        }

        [Fact]
        public void Api_Usage()
        {
            var gauge = Metrics.CreateGauge("name1", "help1");
            gauge.Inc();
            Assert.Equal(1, gauge.Value);
            gauge.Inc(3.2);
            Assert.Equal(4.2, gauge.Value);
            gauge.Set(4);
            Assert.Equal(4, gauge.Value);
            gauge.Dec(0.2);
            Assert.Equal(3.8, gauge.Value);

            Assert.Throws<ArgumentException>(() => gauge.Labels("1"));

            var counter = Metrics.CreateCounter("name2", "help2", "label1");
            counter.Inc();
            counter.Inc(3.2);
            Assert.Equal(4.2, counter.Value);

            Assert.Equal(0, counter.Labels("a").Value);
            counter.Labels("a").Inc(3.3);
            Assert.Equal(3.3, counter.Labels("a").Value);
            counter.Labels("a").Inc(1.1);
            Assert.Equal(4.4, counter.Labels("a").Value);
        }

        [Fact]
        public void Null_Labels()
        {
            var counter = Metrics.CreateCounter("name2", "help2", "label1", "label2");
            Assert.Throws<ArgumentException>(() => counter.Labels().Inc());
            Assert.Throws<ArgumentNullException>(() => counter.Labels(null).Inc());
            Assert.Throws<ArgumentNullException>(() => counter.Labels("param1", null).Inc());
        }

        [Fact]
        public void Cannot_Create_Metrics_With_The_Same_Name_But_Different_Labels()
        {
            Metrics.CreateGauge("name1", "h");
            Assert.Throws<ArgumentException>(() => Metrics.CreateCounter("name1", "h", "label1"));
        }

        [Fact]
        public void Counter_Collection()
        {
            var counter = Metrics.CreateCounter("name1", "help1", "label1");

            counter.Inc();
            counter.Inc(3.2);
            counter.Labels("abc").Inc(3.2);

            var exported = CollectorRegistry.Instance.CollectAll().ToArray();

            Assert.Equal(1, exported.Length);
            var familiy1 = exported[0];
            Assert.Equal("name1", familiy1.Name);
            Assert.Equal("help1", familiy1.Help);
            var metrics = familiy1.Metrics;
            Assert.Equal(2, metrics.Length);

            foreach (var metric in metrics)
            {
                Assert.Null(metric.CGauge);
                Assert.Null(metric.CHistogram);
                Assert.Null(metric.CSummary);
                Assert.Null(metric.CUntyped);
                Assert.NotNull(metric.CCounter);
            }

            Assert.Equal(4.2, metrics[0].CCounter.Value);
            Assert.Equal(0, metrics[0].Labels.Length);

            Assert.Equal(3.2, metrics[1].CCounter.Value);
            var labelPairs = metrics[1].Labels;
            Assert.Equal(1, labelPairs.Length);
            Assert.Equal("label1", labelPairs[0].Name);
            Assert.Equal("abc", labelPairs[0].Value);
        }

        [Fact]
        public void Counter_Reset()
        {
            var counter = Metrics.CreateCounter("name1", "help1", "label1");

            counter.Inc();
            counter.Inc(3.2);
            counter.Labels("test").Inc(1);
            var counterValue = CollectorRegistry.Instance.CollectAll().ToArray()[0].Metrics[0].CCounter.Value;
            Assert.Equal(4.2, counterValue);

            counter.Reset();
            var metricFamily = CollectorRegistry.Instance.CollectAll().ToArray()[0];
            counterValue = metricFamily.Metrics[0].CCounter.Value;
            var counterValueLabeled = metricFamily.Metrics[1].CCounter.Value;
            Assert.Equal(0, counterValue);
            Assert.Equal(0, counterValueLabeled);

            counter.Inc();
            counterValue = CollectorRegistry.Instance.CollectAll().ToArray()[0].Metrics[0].CCounter.Value;
            Assert.Equal(1, counterValue);
        }

        [Fact]
        public void Custom_Registry()
        {
            var myRegistry = new CollectorRegistry();
            var counter1 = Metrics.WithCustomRegistry(myRegistry).CreateCounter("counter1", "help1"); //registered on a custom registry

            var counter2 = Metrics.CreateCounter("counter1", "help1"); //created on different registry - same name is hence permitted

            counter1.Inc(3);
            counter2.Inc(4);

            Assert.Equal(3, myRegistry.CollectAll().ToArray()[0].Metrics[0].CCounter.Value); //counter1 == 3
            Assert.Equal(4, CollectorRegistry.Instance.CollectAll().ToArray()[0].Metrics[0].CCounter.Value); //counter2 == 4
        }

        [Fact]
        public void Gauge_Collection()
        {
            var gauge = Metrics.CreateGauge("name1", "help1");

            gauge.Inc();
            gauge.Inc(3.2);
            gauge.Set(4);
            gauge.Dec(0.2);

            var exported = CollectorRegistry.Instance.CollectAll().ToArray();

            Assert.Equal(1, exported.Length);
            var familiy1 = exported[0];
            Assert.Equal("name1", familiy1.Name);
            var metrics = familiy1.Metrics;
            Assert.Equal(1, metrics.Length);
            foreach (var metric in metrics)
            {
                Assert.Null(metric.CCounter);
                Assert.Null(metric.CHistogram);
                Assert.Null(metric.CSummary);
                Assert.Null(metric.CUntyped);
                Assert.NotNull(metric.CGauge);
            }

            Assert.Equal(3.8, metrics[0].CGauge.Value);
        }

        [Fact]
        public void Histogram_Tests()
        {
            var histogram = Metrics.CreateHistogram("hist1", "help", new[] { 1.0, 2.0, 3.0 });
            histogram.Observe(1.5);
            histogram.Observe(2.5);
            histogram.Observe(1);
            histogram.Observe(2.4);
            histogram.Observe(2.1);
            histogram.Observe(0.4);
            histogram.Observe(1.4);
            histogram.Observe(1.5);
            histogram.Observe(3.9);

            var metric = histogram.Collect().Metrics[0];
            Assert.NotNull(metric.CHistogram);
            Assert.Equal(9ul, metric.CHistogram.SampleCount);
            Assert.Equal(16.7, metric.CHistogram.SampleSum);
            Assert.Equal(4, metric.CHistogram.Buckets.Length);
            Assert.Equal(2ul, metric.CHistogram.Buckets[0].CumulativeCount);
            Assert.Equal(5ul, metric.CHistogram.Buckets[1].CumulativeCount);
            Assert.Equal(8ul, metric.CHistogram.Buckets[2].CumulativeCount);
            Assert.Equal(9ul, metric.CHistogram.Buckets[3].CumulativeCount);
        }

        [Fact]
        public void Label_Names()
        {
            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("a", "help1", "my-metric"));
            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("a", "help1", "my!metric"));
            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("a", "help1", "my%metric"));
            Assert.Throws<ArgumentException>(() => Metrics.CreateHistogram("a", "help1", "le"));
            Metrics.CreateGauge("a", "help1", "my:metric");
            Metrics.CreateGauge("b", "help1", "good_name");

            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("c", "help1", "__reserved"));
        }

        [Fact]
        public void Metric_Names()
        {
            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("my-metric", "help"));
            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("my!metric", "help"));
            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("%", "help"));
            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("5a", "help"));

            Metrics.CreateGauge("abc", "help");
            Metrics.CreateGauge("myMetric2", "help");
            Metrics.CreateGauge("a:3", "help");
        }

        [Fact]
        public void Same_Labels_Return_Same_Instance()
        {
            var gauge = Metrics.CreateGauge("name1", "help1", "label1");

            var labelled1 = gauge.Labels("1");

            var labelled2 = gauge.Labels("1");

            Assert.Same(labelled1, labelled2);
        }

        [Fact]
        public void Summary_Tests()
        {
            var summary = Metrics.CreateSummary("summ1", "help");

            summary.Observe(1);
            summary.Observe(2);
            summary.Observe(3);

            var metric = summary.Collect().Metrics[0];
            Assert.NotNull(metric.CSummary);
            Assert.Equal(3ul, metric.CSummary.SampleCount);
            Assert.Equal(6, metric.CSummary.SampleSum);
        }
    }
}