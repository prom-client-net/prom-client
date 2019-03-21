using System.IO;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client
{
    public static class ScrapeHandler
    {
        public static void Process(ICollectorRegistry registry, Stream outputStream)
        {
            using (var metricsWriter = new MetricsTextWriter(outputStream))
            {
                registry.CollectTo(metricsWriter);
            }
        }

        public static MemoryStream Process(ICollectorRegistry registry)
        {
            // leave open
            var stream = new MemoryStream();
            Process(registry, stream);
            stream.Position = 0;
            return stream;
        }
    }
}
