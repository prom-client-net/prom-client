using System;
using System.Linq;

using NSubstitute;
using NSubstitute.Extensions;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
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

        [Theory]
        [InlineData(3.2)]
        [InlineData(3.1)]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(8.9)]
        public void Counter_Collection(double value)
        {
            var writer = Substitute.For<IMetricsWriter>();
            var counter = Metrics.CreateCounter("name1", "help1", "label1");

            counter.Inc();
            counter.Inc(value);
            counter.WithLabels("abc").Inc(value);

            counter.Collect(writer);

            Received.InOrder(() => {
                writer.StartMetric("name1");
                writer.WriteHelp("help1");
                writer.WriteType(Contracts.CMetricType.Counter);

                var sample1 = writer.StartSample();
                sample1.WriteValue(value + 1);

                var sample2 = writer.StartSample();
                var lbl = sample2.StartLabels();
                lbl.WriteLabel("label1", "abc");
                lbl.EndLabels();
                sample2.WriteValue(value);
            });
        }

        [Fact]
        public void Counter_Reset()
        {
            var writer = Substitute.For<IMetricsWriter>();
            var counter = Metrics.CreateCounter("name1", "help1", "label1");

            counter.Inc();
            counter.Inc(3.2);
            counter.Labels("test").Inc(1);
            counter.Reset();

            counter.Collect(writer);

            Received.InOrder(() => {
                writer.StartMetric("name1");
                writer.WriteHelp("help1");
                writer.WriteType(Contracts.CMetricType.Counter);

                var sample1 = writer.StartSample();
                sample1.WriteValue(0);

                var sample2 = writer.StartSample();
                var lbl = sample2.StartLabels();
                lbl.WriteLabel("label1", "test");
                lbl.EndLabels();
                sample2.WriteValue(0);
            });
        }

        [Fact]
        public void Custom_Registry()
        {
            var myRegistry = new CollectorRegistry();
            var counter1 = Metrics.WithCustomRegistry(myRegistry).CreateCounter("counter1", "help1"); //registered on a custom registry

            var counter2 = Metrics.CreateCounter("counter1", "help1"); //created on different registry - same name is hence permitted

            counter1.Inc(3);
            counter2.Inc(4);

            Assert.Single(myRegistry.Enumerate());
            Assert.Single(CollectorRegistry.Instance.Enumerate());

            Assert.Equal(3, ((ICounter)myRegistry.Enumerate().Single()).Value); //counter1 == 3
            Assert.Equal(4, ((ICounter)CollectorRegistry.Instance.Enumerate().Single()).Value); //counter2 == 4
        }

        [Fact]
        public void Gauge_Collection()
        {
            var writer = Substitute.For<IMetricsWriter>();
            var gauge = Metrics.CreateGauge("name1", "help1");

            gauge.Inc();
            gauge.Inc(3.2);
            gauge.Set(4);
            gauge.Dec(0.2);

            gauge.Collect(writer);

            Received.InOrder(() => {
                writer.StartMetric("name1");
                writer.WriteHelp("help1");
                writer.WriteType(Contracts.CMetricType.Gauge);

                var sample1 = writer.StartSample();
                sample1.WriteValue(3.8);
            });
        }

        [Fact]
        public void Histogram_Tests()
        {
            var writer = Substitute.For<IMetricsWriter>();
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

            histogram.Collect(writer);

            Received.InOrder(() => {
                writer.StartMetric("hist1");
                writer.WriteHelp("help");
                writer.WriteType(Contracts.CMetricType.Histogram);

                var sample = writer.StartSample("_bucket");
                var lbl = sample.StartLabels();
                lbl.WriteLabel("le", "1");
                lbl.EndLabels();
                sample.WriteValue(2);

                sample = writer.StartSample("_bucket");
                lbl = sample.StartLabels();
                lbl.WriteLabel("le", "2");
                lbl.EndLabels();
                sample.WriteValue(5);

                sample = writer.StartSample("_bucket");
                lbl = sample.StartLabels();
                lbl.WriteLabel("le", "3");
                lbl.EndLabels();
                sample.WriteValue(8);

                sample = writer.StartSample("_bucket");
                lbl = sample.StartLabels();
                lbl.WriteLabel("le", "+Inf");
                lbl.EndLabels();
                sample.WriteValue(9);

                sample = writer.StartSample("_sum");
                sample.WriteValue(16.7);

                sample = writer.StartSample("_count");
                sample.WriteValue(9);
            });
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

    }
}
