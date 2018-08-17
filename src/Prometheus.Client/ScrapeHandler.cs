using System.IO;
using System.Linq;
using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client
{
    public class ScrapeHandler
    {
        public static void Process(ICollectorRegistry registry, Stream outputStream)
        {
            var collected = registry.CollectAll();
            TextFormatter.Format(outputStream, collected.ToArray());
        }

        public static MemoryStream Process(ICollectorRegistry registry)
        {
            // leave open
            var stream = new MemoryStream();
            var collected = registry.CollectAll();
            TextFormatter.Format(stream, collected.ToArray());
            stream.Position = 0;
            return stream;
        }
    }
}