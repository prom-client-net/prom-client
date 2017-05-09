using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Prometheus.Client.Internal;
using Prometheus.Contracts;
using Xunit;

namespace Prometheus.Client.Tests
{
    public sealed class AsciiFormatterTests
    {
        [Theory]
        [InlineData("simple-label-value-1")]
        [InlineData("with\nlinebreaks")]
        [InlineData("with\nlinebreaks and \\slashes and quotes \"")]
        public void Family_Should_Be_Formatted_To_One_Line(string labelValue)
        {
            using (var ms = new MemoryStream())
            {
                var metricFamily = new MetricFamily
                {
                    name = "family1",
                    help = "help",
                    type = MetricType.COUNTER
                };

                var metricCounter = new Contracts.Counter { value = 100 };
                metricFamily.metric.Add(new Metric
                {
                    counter = metricCounter,
                    label = new List<LabelPair>
                    {
                        new LabelPair { name = "label1", value = labelValue }
                    }
                });

                AsciiFormatter.Format(ms, new[]
                {
                    metricFamily
                });

                using (var sr = new StringReader(Encoding.UTF8.GetString(ms.ToArray())))
                {
                    var linesCount = 0;
                    var line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        linesCount += 1;
                    }
                    Assert.Equal(3, linesCount);
                }
            }
        }
    }
}