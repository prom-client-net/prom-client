using System;
using System.Linq;
using Prometheus.Client.Advanced;
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

            Assert.Throws<InvalidOperationException>(() => gauge.Labels("1"));

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
        public void Cannot_Create_Metrics_With_The_Same_Name_But_Different_Labels()
        {
            Metrics.CreateGauge("name1", "h");
            Assert.Throws<InvalidOperationException>(() => Metrics.CreateCounter("name1", "h", "label1"));
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
            Assert.Equal("name1", familiy1.name);
            Assert.Equal("help1", familiy1.help);
            var metrics = familiy1.metric;
            Assert.Equal(2, metrics.Count);

            foreach (var metric in metrics)
            {
                Assert.Null(metric.gauge);
                Assert.Null(metric.histogram);
                Assert.Null(metric.summary);
                Assert.Null(metric.untyped);
                Assert.NotNull(metric.counter);
            }

            Assert.Equal(4.2, metrics[0].counter.value);
            Assert.Equal(0, metrics[0].label.Count);

            Assert.Equal(3.2, metrics[1].counter.value);
            var labelPairs = metrics[1].label;
            Assert.Equal(1, labelPairs.Count);
            Assert.Equal("label1", labelPairs[0].name);
            Assert.Equal("abc", labelPairs[0].value);
        }

        [Fact]
        public void Custom_Registry()
        {
            var myRegistry = new CollectorRegistry();
            var counter1 = Metrics.WithCustomRegistry(myRegistry).CreateCounter("counter1", "help1"); //registered on a custom registry

            var counter2 = Metrics.CreateCounter("counter1", "help1"); //created on different registry - same name is hence permitted

            counter1.Inc(3);
            counter2.Inc(4);

            Assert.Equal(3, myRegistry.CollectAll().ToArray()[0].metric[0].counter.value); //counter1 == 3
            Assert.Equal(4, CollectorRegistry.Instance.CollectAll().ToArray()[0].metric[0].counter.value); //counter2 == 4
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
            Assert.Equal("name1", familiy1.name);
            var metrics = familiy1.metric;
            Assert.Equal(1, metrics.Count);
            foreach (var metric in metrics)
            {
                Assert.Null(metric.counter);
                Assert.Null(metric.histogram);
                Assert.Null(metric.summary);
                Assert.Null(metric.untyped);
                Assert.NotNull(metric.gauge);
            }

            Assert.Equal(3.8, metrics[0].gauge.value);
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

            var metric = histogram.Collect().metric[0];
            Assert.NotNull(metric.histogram);
            Assert.Equal(9ul, metric.histogram.sample_count);
            Assert.Equal(16.7, metric.histogram.sample_sum);
            Assert.Equal(4, metric.histogram.bucket.Count);
            Assert.Equal(2ul, metric.histogram.bucket[0].cumulative_count);
            Assert.Equal(5ul, metric.histogram.bucket[1].cumulative_count);
            Assert.Equal(8ul, metric.histogram.bucket[2].cumulative_count);
            Assert.Equal(9ul, metric.histogram.bucket[3].cumulative_count);
        }

        [Fact]
        public void Label_Names()
        {
            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("a", "help1", "my-metric"));
            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("a", "help1", "my!metric"));
            Assert.Throws<ArgumentException>(() => Metrics.CreateGauge("a", "help1", "my%metric"));
            Assert.Throws<ArgumentException>(() => Metrics.CreateHistogram("a", "help1", null, "le"));
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

            var metric = summary.Collect().metric[0];
            Assert.NotNull(metric.summary);
            Assert.Equal(3ul, metric.summary.sample_count);
            Assert.Equal(6, metric.summary.sample_sum);
        }
    }
}