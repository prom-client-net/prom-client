using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Prometheus.Client.Contracts;
using Prometheus.Client.Internal;
using Xunit;

namespace Prometheus.Client.Tests
{
    public sealed class TextFormatterTests
    {
        [Theory]
        [InlineData("simple-label-value-1")]
        [InlineData("with\nlinebreaks")]
        [InlineData("with\nlinebreaks and \\slashes and quotes \"")]
        public void Family_Should_Be_Formatted_To_One_Line(string labelValue)
        {
            using (var ms = new MemoryStream())
            {
                var metricFamily = new CMetricFamily
                {
                    Name = "family1",
                    Help = "help",
                    Type = CMetricType.Counter
                };

                var metricCounter = new Contracts.CCounter { Value = 100 };
                metricFamily.Metrics.Add(new CMetric
                {
                    CCounter = metricCounter,
                    Labels = new List<CLabelPair>
                    {
                        new CLabelPair { Name = "label1", Value = labelValue }
                    }
                });

                TextFormatter.Format(ms, new[]
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