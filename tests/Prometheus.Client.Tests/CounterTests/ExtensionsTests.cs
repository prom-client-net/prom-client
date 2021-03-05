using System;
using NSubstitute;
using Xunit;

namespace Prometheus.Client.Tests.CounterTests
{
    public class ExtensionsTests
    {
        [Fact]
        public void IncWithTs()
        {
            var metric = Substitute.For<ICounter>();
            var ts = DateTimeOffset.UtcNow;
            var inc = 2;

            metric.Inc(inc, ts);

            metric.Received().Inc(inc, ts.ToUnixTimeMilliseconds());
        }

        [Fact]
        public void IncToWithTs()
        {
            var metric = Substitute.For<ICounter>();
            var ts = DateTimeOffset.UtcNow;
            var value = 2;

            metric.IncTo(value, ts);

            metric.Received().IncTo(value, ts.ToUnixTimeMilliseconds());
        }

        [Fact]
        public void UnlabelledInc()
        {
            var family = MockFamily();
            var inc = 2;

            family.Inc(inc);

            family.Unlabelled.Received().Inc(inc);
        }

        [Fact]
        public void UnlabelledIncWithTs()
        {
            var family = MockFamily();
            var ts = 123;
            var inc = 2;

            family.Inc(inc, ts);

            family.Unlabelled.Received().Inc(inc, ts);
        }

        [Fact]
        public void UnlabelledIncWithTsDate()
        {
            var family = MockFamily();
            var ts = DateTimeOffset.UtcNow;
            var inc = 2;

            family.Inc(inc, ts);

            family.Unlabelled.Received().Inc(inc, ts.ToUnixTimeMilliseconds());
        }

        [Fact]
        public void UnlabelledIncTo()
        {
            var family = MockFamily();
            var val = 2;

            family.IncTo(val);

            family.Unlabelled.Received().IncTo(val);
        }

        [Fact]
        public void UnlabelledIncToWithTs()
        {
            var family = MockFamily();
            var ts = 123;
            var val = 2;

            family.IncTo(val, ts);

            family.Unlabelled.Received().IncTo(val, ts);
        }

        [Fact]
        public void UnlabelledIncToWithTsDate()
        {
            var family = MockFamily();
            var ts = DateTimeOffset.UtcNow;
            var val = 2;

            family.IncTo(val, ts);

            family.Unlabelled.Received().IncTo(val, ts.ToUnixTimeMilliseconds());
        }
        
        [Fact]
        public void UnlabelledTupleInc()
        {
            var family = MockFamilyTuple();
            var inc = 2;

            family.Inc(inc);

            family.Unlabelled.Received().Inc(inc);
        }

        [Fact]
        public void UnlabelledTupleIncWithTs()
        {
            var family = MockFamilyTuple();
            var ts = 123;
            var inc = 2;

            family.Inc(inc, ts);

            family.Unlabelled.Received().Inc(inc, ts);
        }

        [Fact]
        public void UnlabelledTupleIncWithTsDate()
        {
            var family = MockFamilyTuple();
            var ts = DateTimeOffset.UtcNow;
            var inc = 2;

            family.Inc(inc, ts);

            family.Unlabelled.Received().Inc(inc, ts.ToUnixTimeMilliseconds());
        }

        [Fact]
        public void UnlabelledTupleIncTo()
        {
            var family = MockFamilyTuple();
            var val = 2;

            family.IncTo(val);

            family.Unlabelled.Received().IncTo(val);
        }

        [Fact]
        public void UnlabelledTupleIncToWithTs()
        {
            var family = MockFamilyTuple();
            var ts = 123;
            var val = 2;

            family.IncTo(val, ts);

            family.Unlabelled.Received().IncTo(val, ts);
        }

        [Fact]
        public void UnlabelledTupleIncToWithTsDate()
        {
            var family = MockFamilyTuple();
            var ts = DateTimeOffset.UtcNow;
            var val = 2;

            family.IncTo(val, ts);

            family.Unlabelled.Received().IncTo(val, ts.ToUnixTimeMilliseconds());
        }

        private IMetricFamily<ICounter> MockFamily()
        {
            var metric = Substitute.For<ICounter>();
            var family = Substitute.For<IMetricFamily<ICounter>>();
            family.Unlabelled.Returns(metric);

            return family;
        }

        private IMetricFamily<ICounter, (string, string)> MockFamilyTuple()
        {
            var metric = Substitute.For<ICounter>();
            var family = Substitute.For<IMetricFamily<ICounter, (string, string)>>();
            family.Unlabelled.Returns(metric);

            return family;
        }
    }
}
