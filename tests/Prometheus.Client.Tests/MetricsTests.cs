using System;
using System.Linq;
using Prometheus.Client.Abstractions;
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
        public void Cannot_Create_Metrics_With_The_Same_Name_But_Different_Labels()
        {
            Metrics.CreateGauge("name1", "h");
            Assert.Throws<ArgumentException>(() => Metrics.CreateCounter("name1", "h", "label1"));
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

            Assert.Equal(3, ((ICounter) myRegistry.Enumerate().Single()).Value); //counter1 == 3
            Assert.Equal(4, ((ICounter) CollectorRegistry.Instance.Enumerate().Single()).Value); //counter2 == 4
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
