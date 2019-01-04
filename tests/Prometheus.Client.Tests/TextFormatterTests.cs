using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Prometheus.Client.Contracts;
using Xunit;
using Xunit.Abstractions;

namespace Prometheus.Client.Tests
{
    public sealed class TextFormatterTests
    {
        private readonly ITestOutputHelper _output;

        public TextFormatterTests(ITestOutputHelper output)
        {
            _output = output;
        }

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

                var metricCounter = new CCounter { Value = 100 };
                metricFamily.Metrics = new[]
                {
                    new CMetric
                    {
                        CCounter = metricCounter,
                        Labels = new[]
                        {
                            new CLabelPair { Name = "label1", Value = labelValue }
                        }
                    }
                };

                TextFormatter.Format(ms, new[]
                {
                    metricFamily
                });

                using (var sr = new StringReader(Encoding.UTF8.GetString(ms.ToArray())))
                {
                    var linesCount = 0;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        _output.WriteLine(line);
                        linesCount += 1;
                    }

                    Assert.Equal(3, linesCount);
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-2)]
        [InlineData(-6)]
        [InlineData(20)]
        [InlineData(0)]
        public void Timestamp_Should_Be_Parse_To_Long(int diffMinutes)
        {
            using (var ms = new MemoryStream())
            {
                var metricFamily = new CMetricFamily
                {
                    Name = "family1",
                    Help = "help",
                    Type = CMetricType.Counter
                };

                var metricCounter = new CCounter
                {
                    Value = 65
                };

                var timestamp = (long) (DateTime.UtcNow.AddMinutes(diffMinutes) - new DateTime(1970, 1, 1)).TotalSeconds;
                
                metricFamily.Metrics = new[]
                {
                    new CMetric
                    {
                        CCounter = metricCounter,
                        Timestamp = timestamp
                    }
                };

                TextFormatter.Format(ms, new[]
                {
                    metricFamily
                });

                using (var sr = new StringReader(Encoding.UTF8.GetString(ms.ToArray())))
                {
                    var linesCount = 0;
                    string line;
                    string lineMetric = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        _output.WriteLine(line);
                        linesCount += 1;
                        if (linesCount == 3)
                            lineMetric = line;
                    }

                    Assert.NotNull(lineMetric);
                    var arrMetric = lineMetric.Split(' ');
                    Assert.Equal(timestamp, long.Parse(arrMetric[2]));
                }
            }
        }
    }
}
