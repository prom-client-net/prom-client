using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Xunit;

namespace Prometheus.Client.Tests;

internal static class CollectionTestHelper
{
    public static Task TestCollectionAsync(Action<IMetricFactory> metricsSetup, string resourceName)
    {
        return TestCollectionAsync(registry =>
        {
            var factory = new MetricFactory(registry);
            metricsSetup(factory);
        }, resourceName);
    }

    public static async Task TestCollectionAsync(Action<ICollectorRegistry> setup, string resourceName)
    {
        var registry = new CollectorRegistry();

        setup(registry);

        string formattedText;

        using (var stream = new MemoryStream())
        {
            using (var writer = new MetricsTextWriter(stream))
            {
                await registry.CollectToAsync(writer);

                await writer.CloseWriterAsync();
            }

            stream.Seek(0, SeekOrigin.Begin);

            using (var streamReader = new StreamReader(stream))
            {
                formattedText = await streamReader.ReadToEndAsync();
            }
        }

        Assert.Equal(GetFileContent(resourceName), formattedText);
    }

    private static string GetFileContent(string resourcePath)
    {
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd().ToUnixLineEndings();
    }
}
