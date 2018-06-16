using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Prometheus.Client.Collectors;

namespace Prometheus.Client
{
    public class MetricPush
    {
        public const string ContentType = "text/plain; version=0.0.4";

        /// <summary>
        /// Push metrics to single pushgateway endpoint
        /// </summary>
        /// <param name="endpoint">PushGateway endpoint</param>
        /// <param name="job">job name</param>
        /// <param name="instance">instance</param>
        /// <param name="contentType">Content-Type</param>
        /// <returns></returns>
        public async Task PushAsync(string endpoint, string job, string instance, string contentType = ContentType)
        {
            await PushAsync(new[] {endpoint}, job, instance, contentType);
        }

        /// <summary>
        /// Push metrics to multiple pushgateway endpoints (fault-tolerance)
        /// </summary>
        /// <param name="endpoints">multiple pushgateway enpoints (fault-tolerance)</param>
        /// <param name="job">job name</param>
        /// <param name="instance">instance name</param>
        /// <param name="contentType">content-type</param>
        /// <returns></returns>
        public async Task PushAsync(string[] endpoints, string job, string instance, string contentType = ContentType)
        {
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
            var tasks = new List<Task<HttpResponseMessage>>(endpoints.Length);

            foreach (var endpoint in endpoints)
            {
                if (string.IsNullOrEmpty(endpoint))
                {
                    throw new ArgumentNullException(nameof(endpoint));
                }

                var url = $"{endpoint.TrimEnd('/')}/job/{job}";
                if (!string.IsNullOrEmpty(instance))
                {
                    url = $"{url}/instance/{instance}";
                }

                if (!Uri.TryCreate(url, UriKind.Absolute, out var targetUrl))
                {
                    throw new ArgumentException("Endpoint must be a valid url", nameof(endpoint));
                }

                tasks.Add(httpClient.PostAsync(targetUrl, streamContent));
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                var response = await task;
                response.EnsureSuccessStatusCode();
            }
        }
    }
}