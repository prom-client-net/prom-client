using System;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class TupleHelperTests
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
            var type = TupleHelper.MakeValueTupleType(len);

            Assert.Equal(expected, type);
        }

        [Fact]
        public void GetSize0Test()
        {
            var size = TupleHelper.GetSize<ValueTuple>();

            Assert.Equal(0, size);
        }

        [Fact]
        public void GetSize1Test()
        {
            var size = TupleHelper.GetSize<ValueTuple<string>>();

            Assert.Equal(1, size);
        }

        [Fact]
        public void GetSize2Test()
        {
            var size = TupleHelper.GetSize<(string, string)>();

            Assert.Equal(2, size);
        }

        [Fact]
        public void GetSize8Test()
        {
            var size = TupleHelper.GetSize<(string, string, string, string, string, string, string, string)>();

            Assert.Equal(8, size);
        }

        [Fact]
        public void GetSize16Test()
        {
            var size = TupleHelper.GetSize<(string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string)>();

            Assert.Equal(16, size);
        }

        [Fact]
        public void FormatTuple0()
        {
            var tuple = ValueTuple.Create();
            var formatted = TupleHelper.GenerateFormatter<ValueTuple>()(tuple);

            Assert.Equal(new string[0], formatted);
        }

        [Fact]
        public void FormatTuple1()
        {
            var tuple = ValueTuple.Create("1");
            var formatted = TupleHelper.GenerateFormatter<ValueTuple<string>>()(tuple);

            Assert.Equal(new[] { "1" }, formatted);
        }

        [Fact]
        public void FormatTuple2()
        {
            var tuple = ("1", "2");
            var formatted = TupleHelper.GenerateFormatter<(string, string)>()(tuple);

            Assert.Equal(new[] { "1", "2" }, formatted);
        }

        [Fact]
        public void FormatTuple8()
        {
            var tuple = ("1", "2", "3", "4", "5", "6", "7", "8");
            var formatted = TupleHelper.GenerateFormatter<(string, string, string, string, string, string, string, string)>()(tuple);

            Assert.Equal(new[] { "1", "2", "3", "4", "5", "6", "7", "8" }, formatted);
        }

        [Fact]
        public void FormatTuple16()
        {
            var tuple = ("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16");
            var formatted = TupleHelper.GenerateFormatter<(string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string)>()(tuple);

            Assert.Equal(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" }, formatted);
        }

        [Fact]
        public void ParseTuple0()
        {
            var arr = new string[0];
            var parsed = TupleHelper.GenerateParser<ValueTuple>()(arr);

            Assert.Equal(ValueTuple.Create(), parsed);
        }

        [Fact]
        public void ParseTuple1()
        {
            var arr = new[] { "1" };
            var parsed = TupleHelper.GenerateParser<ValueTuple<string>>()(arr);

            Assert.Equal(ValueTuple.Create("1"), parsed);
        }

        [Fact]
        public void ParseTuple2()
        {
            var arr = new[] { "1", "2" };
            var parsed = TupleHelper.GenerateParser<(string, string)>()(arr);

            Assert.Equal(("1", "2"), parsed);
        }

        [Fact]
        public void ParseTuple8()
        {
            var arr = new[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            var parsed = TupleHelper.GenerateParser<(string, string, string, string, string, string, string, string)>()(arr);

            Assert.Equal(("1", "2", "3", "4", "5", "6", "7", "8"), parsed);
        }

        [Fact]
        public void ParseTuple16()
        {
            var arr = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };
            var parsed = TupleHelper.GenerateParser<(string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string)>()(arr);

            Assert.Equal(("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16"), parsed);
        }
    }
}
