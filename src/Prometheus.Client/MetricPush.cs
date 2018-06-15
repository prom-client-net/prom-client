using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Prometheus.Client.Collectors;

namespace Prometheus.Client
{
    public class MetricPush
    {
        public const string ContentType = "text/plain; version=0.0.4";
        public async Task PushAsync(string endpoint, string job, string instance, string contentType = ContentType)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException(nameof(endpoint));
            }
            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            var metrics = CollectorRegistry.Instance.CollectAll();
            var memoryStream = new MemoryStream();
            ScrapeHandler.ProcessScrapeRequest(metrics, contentType, memoryStream);
            memoryStream.Position = 0;
            var streamContent = new StreamContent(memoryStream);

            var httpClient = new HttpClient();
            var url = $"{endpoint.TrimEnd('/')}/job/{job}";
            if (!string.IsNullOrEmpty(instance))
            {
                url = $"{url}/instance/{instance}";
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var targetUrl))
            {
                throw new ArgumentException("Endpoint must be a valid url", nameof(endpoint));
            }

            var response = await httpClient.PostAsync(targetUrl, streamContent);
            response.EnsureSuccessStatusCode();
        }
    }
}