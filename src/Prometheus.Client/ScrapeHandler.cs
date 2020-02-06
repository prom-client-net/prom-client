using System.IO;
using System.Threading.Tasks;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client
{
    public static class ScrapeHandler
    {
        public static async Task ProcessAsync(ICollectorRegistry registry, Stream outputStream)
        {
            using (var metricsWriter = new MetricsTextWriter(outputStream))
            {
                await registry.CollectToAsync(metricsWriter).ConfigureAwait(false);
                await metricsWriter.CloseWriterAsync().ConfigureAwait(false);
            }
        }

        public static async Task<MemoryStream> ProcessAsync(ICollectorRegistry registry)
        {
            // leave open
            var stream = new MemoryStream();
            await ProcessAsync(registry, stream);
            stream.Position = 0;
            return stream;
        }
    }
}
