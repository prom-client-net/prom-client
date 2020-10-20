using System;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class LabelsHelperTests
    {
        [Theory]
        [InlineData(0, typeof(ValueTuple))]
        [InlineData(1, typeof(ValueTuple<string>))]
        [InlineData(2, typeof((string, string)))]
        [InlineData(7, typeof((string, string, string, string, string, string, string)))]
        [InlineData(8, typeof((string, string, string, string, string, string, string, string)))]
        [InlineData(16, typeof((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string)))]
        public void MakeValueTupleTypeTests(int len, Type expected)
        {
            var type = LabelsHelper.MakeValueTupleType(len);

            Assert.Equal(expected, type);
        }

        [Fact]
        public void GetSize0Test()
        {
            var size = LabelsHelper.GetSize<ValueTuple>();

            Assert.Equal(0, size);
        }

        [Fact]
        public void GetSize1Test()
        {
            var size = LabelsHelper.GetSize<ValueTuple<string>>();

            Assert.Equal(1, size);
        }

        [Fact]
        public void GetSize2Test()
        {
            var size = LabelsHelper.GetSize<(string, string)>();

            Assert.Equal(2, size);
        }

        [Fact]
        public void GetSize8Test()
        {
            var size = LabelsHelper.GetSize<(string, string, string, string, string, string, string, string)>();

            Assert.Equal(8, size);
        }

        [Fact]
        public void GetSize16Test()
        {
            var size = LabelsHelper.GetSize<(string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string)>();

            Assert.Equal(16, size);
        }

        [Fact]
        public void FormatTuple0()
        {
            var tuple = ValueTuple.Create();
            var formatted = LabelsHelper.ToArray(tuple);

            Assert.Equal(new string[0], formatted);
        }

        [Fact]
        public void FormatTuple1()
        {
            var tuple = ValueTuple.Create("1");
            var formatted = LabelsHelper.ToArray(tuple);

            Assert.Equal(new[] { "1" }, formatted);
        }

        [Fact]
        public void FormatTuple2()
        {
            var tuple = ("1", "2");
            var formatted = LabelsHelper.ToArray(tuple);

            Assert.Equal(new[] { "1", "2" }, formatted);
        }

        [Fact]
        public void FormatTuple7()
        {
            var tuple = ("1", "2", "3", "4", "5", "6", "7");
            var formatted = LabelsHelper.ToArray(tuple);

            Assert.Equal(new[] { "1", "2", "3", "4", "5", "6", "7" }, formatted);
        }

        [Fact]
        public void FormatTuple8()
        {
            var tuple = ("1", "2", "3", "4", "5", "6", "7", "8");
            var formatted = LabelsHelper.ToArray(tuple);

            Assert.Equal(new[] { "1", "2", "3", "4", "5", "6", "7", "8" }, formatted);
        }

        [Fact]
        public void FormatTuple16()
        {
            var tuple = ("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16");
            var formatted = LabelsHelper.ToArray(tuple);

            Assert.Equal(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" }, formatted);
        }

        [Fact]
        public void ParseTuple0()
        {
            var arr = new string[0];
            var parsed = LabelsHelper.FromArray<ValueTuple>(arr);

            Assert.Equal(ValueTuple.Create(), parsed);
        }

        [Fact]
        public void ParseTuple1()
        {
            var arr = new[] { "1" };
            var parsed = LabelsHelper.FromArray<ValueTuple<string>>(arr);

            Assert.Equal(ValueTuple.Create("1"), parsed);
        }

        [Fact]
        public void ParseTuple2()
        {
            var arr = new[] { "1", "2" };
            var parsed = LabelsHelper.FromArray<(string, string)>(arr);

            Assert.Equal(("1", "2"), parsed);
        }

        [Fact]
        public void ParseTuple7()
        {
            var arr = new[] { "1", "2", "3", "4", "5", "6", "7" };
            var parsed = LabelsHelper.FromArray<(string, string, string, string, string, string, string)>(arr);

            Assert.Equal(("1", "2", "3", "4", "5", "6", "7"), parsed);
        }

        [Fact]
        public void ParseTuple8()
        {
            var arr = new[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            var parsed = LabelsHelper.FromArray<(string, string, string, string, string, string, string, string)>(arr);

            Assert.Equal(("1", "2", "3", "4", "5", "6", "7", "8"), parsed);
        }

        [Fact]
        public void ParseTuple16()
        {
            var arr = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };
            var parsed = LabelsHelper.FromArray<(string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string)>(arr);

            Assert.Equal(("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16"), parsed);
        }

        [Fact]
        public void ThrowOnIntLabelName1()
        {
            var labels = ValueTuple.Create(1);

            Assert.Throws<NotSupportedException>(() => LabelsHelper.ToArray(labels));
        }

        [Fact]
        public void ThrowOnIntLabelName8()
        {
            var labels = ("1", "2", "3", "4", "5", "6", "7", 8);

            Assert.Throws<NotSupportedException>(() => LabelsHelper.ToArray(labels));
        }

        [Fact]
        public void ThrowOnIntLabelName16()
        {
            var labels = ("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", 16);

            Assert.Throws<NotSupportedException>(() => LabelsHelper.ToArray(labels));
        }

        [Fact]
        public void ThrowOnEnumLabelName1()
        {
            var labels = ValueTuple.Create(MetricType.Untyped);

            Assert.Throws<NotSupportedException>(() => LabelsHelper.ToArray(labels));
        }

        [Fact]
        public void ThrowOnEnumLabelName8()
        {
            var labels = ("1", "2", "3", "4", "5", "6", "7", MetricType.Untyped);

            Assert.Throws<NotSupportedException>(() => LabelsHelper.ToArray(labels));
        }

        [Fact]
        public void ThrowOnEnumLabelName16()
        {
            var labels = ("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", MetricType.Untyped);

            Assert.Throws<NotSupportedException>(() => LabelsHelper.ToArray(labels));
        }

        [Fact]
        public void GetHashCode0()
        {
            var tupleCode = LabelsHelper.GetHashCode(ValueTuple.Create());
            var arrayCode = LabelsHelper.GetHashCode(new string[0]);

            Assert.Equal(tupleCode, arrayCode);
        }

        [Fact]
        public void GetHashCode1()
        {
            var tupleCode = LabelsHelper.GetHashCode(ValueTuple.Create("1"));
            var arrayCode = LabelsHelper.GetHashCode(new [] { "1" });

            Assert.Equal(tupleCode, arrayCode);
        }

        [Fact]
        public void GetHashCode2()
        {
            var tupleCode = LabelsHelper.GetHashCode(ValueTuple.Create("1", "2"));
            var arrayCode = LabelsHelper.GetHashCode(new [] {"1", "2"});

            Assert.Equal(tupleCode, arrayCode);
        }

        [Fact]
        public void GetHashCode8()
        {
            var tupleCode = LabelsHelper.GetHashCode(ValueTuple.Create("1", "2", "3", "4", "5", "6", "7", "8"));
            var arrayCode = LabelsHelper.GetHashCode(new [] {"1", "2", "3", "4", "5", "6", "7", "8"});

            Assert.Equal(tupleCode, arrayCode);
        }
    }
}
