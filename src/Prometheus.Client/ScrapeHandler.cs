using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client;

public static class ScrapeHandler
{
    public static async Task ProcessAsync(ICollectorRegistry registry, Stream outputStream, CancellationToken ct = default)
    {
        using var metricsWriter = new MetricsTextWriter(outputStream);

        await registry.CollectToAsync(metricsWriter, ct).ConfigureAwait(false);
        await metricsWriter.CloseWriterAsync(ct).ConfigureAwait(false);
    }

    public static async Task<MemoryStream> ProcessAsync(ICollectorRegistry registry, CancellationToken ct = default)
    {
        // leave open
        var stream = new MemoryStream();
        await ProcessAsync(registry, stream, ct);
        stream.Position = 0;
        return stream;
    }
}
