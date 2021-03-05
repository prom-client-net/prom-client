using System;
using NSubstitute;
using Xunit;

namespace Prometheus.Client.Tests.UntypedTests
{
    public class ExtensionsTests
    {
        [Fact]
        public void SetWithTs()
        {
            var metric = Substitute.For<IUntyped>();
            var ts = DateTimeOffset.UtcNow;
            var val = 2;

            metric.Set(val, ts);

            metric.Received().Set(val, ts.ToUnixTimeMilliseconds());
        }

        [Fact]
        public void UnlabelledSet()
        {
            var family = MockFamily();
            var val = 2;

            family.Set(val);

            family.Unlabelled.Received().Set(val);
        }

        [Fact]
        public void UnlabelledSetWithTs()
        {
            var family = MockFamily();
            var ts = 123;
            var val = 2;

            family.Set(val, ts);

            family.Unlabelled.Received().Set(val, ts);
        }

        [Fact]
        public void UnlabelledSetWithTsDate()
        {
            var family = MockFamily();
            var ts = DateTimeOffset.UtcNow;
            var val = 2;

            family.Set(val, ts);

            family.Unlabelled.Received().Set(val, ts.ToUnixTimeMilliseconds());
        }

        [Fact]
        public void UnlabelledTupleSet()
        {
            var family = MockFamilyTuple();
            var val = 2;

            family.Set(val);

            family.Unlabelled.Received().Set(val);
        }

        [Fact]
        public void UnlabelledTupleSetWithTs()
        {
            var family = MockFamilyTuple();
            var ts = 123;
            var val = 2;

            family.Set(val, ts);

            family.Unlabelled.Received().Set(val, ts);
        }

        [Fact]
        public void UnlabelledTupleSetWithTsDate()
        {
            var family = MockFamilyTuple();
            var ts = DateTimeOffset.UtcNow;
            var val = 2;

            family.Set(val, ts);

            family.Unlabelled.Received().Set(val, ts.ToUnixTimeMilliseconds());
        }

        private IMetricFamily<IUntyped> MockFamily()
        {
            var metric = Substitute.For<IUntyped>();
            var family = Substitute.For<IMetricFamily<IUntyped>>();
            family.Unlabelled.Returns(metric);

            return family;
        }

        private IMetricFamily<IUntyped, (string, string)> MockFamilyTuple()
        {
            var metric = Substitute.For<IUntyped>();
            var family = Substitute.For<IMetricFamily<IUntyped, (string, string)>>();
            family.Unlabelled.Returns(metric);

            return family;
        }
    }
}
