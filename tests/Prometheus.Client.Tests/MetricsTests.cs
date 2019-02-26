using System;
using System.Linq;

using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class MetricsTests
    {
        [Fact]
        public void Custom_Registry()
        {
            var myRegistry = new CollectorRegistry();
            var counter1 = Metrics.WithCustomRegistry(myRegistry).CreateCounter("counter1", "help1"); //registered on a custom registry

            var counter2 = Metrics.CreateCounter("counter1", "help1"); //created on different registry - same name is hence permitted

            counter1.Inc(3);
            counter2.Inc(4);

            Assert.Single(myRegistry.Enumerate());
            Assert.Single(Metrics.DefaultCollectorRegistry.Enumerate());

            Assert.Equal(3, ((ICounter)myRegistry.Enumerate().Single()).Value); //counter1 == 3
            Assert.Equal(4, ((ICounter)Metrics.DefaultCollectorRegistry.Enumerate().Single()).Value); //counter2 == 4
        }

        [Fact]
        public void Label_Names()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.Throws<ArgumentException>(() => factory.CreateGauge("a", "help1", "my-metric"));
            Assert.Throws<ArgumentException>(() => factory.CreateGauge("a", "help1", "my!metric"));
            Assert.Throws<ArgumentException>(() => factory.CreateGauge("a", "help1", "my%metric"));
            Assert.Throws<ArgumentException>(() => factory.CreateHistogram("a", "help1", "le"));
            factory.CreateGauge("a", "help1", "my:metric");
            factory.CreateGauge("b", "help1", "good_name");

            Assert.Throws<ArgumentException>(() => factory.CreateGauge("c", "help1", "__reserved"));
        }
    }
}
