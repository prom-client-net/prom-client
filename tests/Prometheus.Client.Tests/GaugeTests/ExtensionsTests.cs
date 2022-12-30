using System;
using NSubstitute;
using Xunit;

namespace Prometheus.Client.Tests.GaugeTests;

public class ExtensionsTests
{
    [Fact]
    public void IncWithTs()
    {
        var metric = Substitute.For<IGauge>();
        var ts = DateTimeOffset.UtcNow;
        var inc = 2;

        metric.Inc(inc, ts);

        metric.Received().Inc(inc, ts.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void IncToWithTs()
    {
        var metric = Substitute.For<IGauge>();
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

    [Fact]
    public void DecWithTs()
    {
        var metric = Substitute.For<IGauge>();
        var ts = DateTimeOffset.UtcNow;
        var dec = 2;

        metric.Dec(dec, ts);

        metric.Received().Dec(dec, ts.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void DecToWithTs()
    {
        var metric = Substitute.For<IGauge>();
        var ts = DateTimeOffset.UtcNow;
        var value = 2;

        metric.DecTo(value, ts);

        metric.Received().DecTo(value, ts.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void UnlabelledDec()
    {
        var family = MockFamily();
        var dec = 2;

        family.Dec(dec);

        family.Unlabelled.Received().Dec(dec);
    }

    [Fact]
    public void UnlabelledDecWithTs()
    {
        var family = MockFamily();
        var ts = 123;
        var dec = 2;

        family.Dec(dec, ts);

        family.Unlabelled.Received().Dec(dec, ts);
    }

    [Fact]
    public void UnlabelledDecWithTsDate()
    {
        var family = MockFamily();
        var ts = DateTimeOffset.UtcNow;
        var dec = 2;

        family.Dec(dec, ts);

        family.Unlabelled.Received().Dec(dec, ts.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void UnlabelledDecTo()
    {
        var family = MockFamily();
        var val = 2;

        family.DecTo(val);

        family.Unlabelled.Received().DecTo(val);
    }

    [Fact]
    public void UnlabelledDecToWithTs()
    {
        var family = MockFamily();
        var ts = 123;
        var val = 2;

        family.DecTo(val, ts);

        family.Unlabelled.Received().DecTo(val, ts);
    }

    [Fact]
    public void UnlabelledDecToWithTsDate()
    {
        var family = MockFamily();
        var ts = DateTimeOffset.UtcNow;
        var val = 2;

        family.DecTo(val, ts);

        family.Unlabelled.Received().DecTo(val, ts.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void UnlabelledTupleDec()
    {
        var family = MockFamilyTuple();
        var dec = 2;

        family.Dec(dec);

        family.Unlabelled.Received().Dec(dec);
    }

    [Fact]
    public void UnlabelledTupleDecWithTs()
    {
        var family = MockFamilyTuple();
        var ts = 123;
        var dec = 2;

        family.Dec(dec, ts);

        family.Unlabelled.Received().Dec(dec, ts);
    }

    [Fact]
    public void UnlabelledTupleDecWithTsDate()
    {
        var family = MockFamilyTuple();
        var ts = DateTimeOffset.UtcNow;
        var dec = 2;

        family.Dec(dec, ts);

        family.Unlabelled.Received().Dec(dec, ts.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void UnlabelledTupleDecTo()
    {
        var family = MockFamilyTuple();
        var val = 2;

        family.DecTo(val);

        family.Unlabelled.Received().DecTo(val);
    }

    [Fact]
    public void UnlabelledTupleDecToWithTs()
    {
        var family = MockFamilyTuple();
        var ts = 123;
        var val = 2;

        family.DecTo(val, ts);

        family.Unlabelled.Received().DecTo(val, ts);
    }

    [Fact]
    public void UnlabelledTupleDecToWithTsDate()
    {
        var family = MockFamilyTuple();
        var ts = DateTimeOffset.UtcNow;
        var val = 2;

        family.DecTo(val, ts);

        family.Unlabelled.Received().DecTo(val, ts.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void SetWithTs()
    {
        var metric = Substitute.For<IGauge>();
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

    private IMetricFamily<IGauge> MockFamily()
    {
        var metric = Substitute.For<IGauge>();
        var family = Substitute.For<IMetricFamily<IGauge>>();
        family.Unlabelled.Returns(metric);

        return family;
    }

    private IMetricFamily<IGauge, (string, string)> MockFamilyTuple()
    {
        var metric = Substitute.For<IGauge>();
        var family = Substitute.For<IMetricFamily<IGauge, (string, string)>>();
        family.Unlabelled.Returns(metric);

        return family;
    }
}
