using System;
using System.Linq;
using Xunit;

namespace Prometheus.Client.Tests.SummaryTests
{
    public class SampleTests
    {
        [Fact]
        public void ResetShouldClearObservations()
        {
            var summary = CreateSummary();
            summary.Observe(123);

            summary.Reset();

            var state = summary.Value;
            Assert.Equal(0,state.Count);
            Assert.Equal(0, state.Sum);
            Assert.True(state.Quantiles.All(b => double.IsNaN(b.Value)));
        }

        private ISummary CreateSummary()
        {
            var config = new SummaryConfiguration("test", string.Empty, Array.Empty<string>(), false);
            return new Summary(config, Array.Empty<string>());
        }
    }
}
