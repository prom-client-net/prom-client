using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Prometheus.Client.Contracts;

namespace Prometheus.Client
{
    public class MetricPushService : IMetricPushService
    {
        private HttpClient _httpClient;

        private const string ContentType = "text/plain; version=0.0.4";

        protected virtual HttpMessageHandler MessageHandler => new HttpClientHandler();

        public MetricPushService()
        {
            _httpClient = new HttpClient(MessageHandler);
        }

        /// <summary>
        /// Push metrics to single pushgateway endpoint
        /// </summary>
        /// <param name="metricFamilies">Collection of metrics</param>
        /// <param name="endpoint">PushGateway endpoint</param>
        /// <param name="job">job name</param>
        /// <param name="instance">instance name</param>
        /// <param name="contentType">content-type</param>
        /// <returns></returns>
        public async Task PushAsync(IEnumerable<CMetricFamily> metricFamilies, string endpoint, string job, string instance, string contentType)
        {
            await PushAsync(metricFamilies, new[] { endpoint }, job, instance, contentType).ConfigureAwait(false);
        }

        /// <summary>
        /// Push metrics to single pushgateway endpoint
        /// </summary>
        /// <param name="metrics">Collection of metrics</param>
        /// <param name="endpoints">PushGateway endpoints - fault-tolerance</param>
        /// <param name="job">job name</param>
        /// <param name="instance">instance name</param>
        /// <param name="contentType">content-type</param>
        /// <returns></returns>
        public async Task PushAsync(IEnumerable<CMetricFamily> metrics, string[] endpoints, string job, string instance, string contentType)
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            var cntType = ContentType;
            if (!string.IsNullOrEmpty(contentType))
            {
                cntType = contentType;
            }
            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            var tasks = new List<Task<HttpResponseMessage>>(endpoints.Length);
            var streamsToDispose = new List<Stream>();

            foreach (var endpoint in endpoints)
            {
                var memoryStream = new MemoryStream();
                streamsToDispose.Add(memoryStream);
                ScrapeHandler.ProcessScrapeRequest(metrics, cntType, memoryStream);
                memoryStream.Position = 0;
                var debugString = Encoding.UTF8.GetString(memoryStream.ToArray());
                System.Diagnostics.Debug.WriteLine(debugString);

                if (string.IsNullOrEmpty(endpoint))
                {
                    throw new ArgumentNullException(nameof(endpoint));
                }

                var url = $"{endpoint.TrimEnd('/')}/metrics/job/{job}";
                if (!string.IsNullOrEmpty(instance))
                {
                    url = $"{url}/instance/{instance}";
                }

                if (!Uri.TryCreate(url, UriKind.Absolute, out var targetUrl))
                {
                    throw new ArgumentException("Endpoint must be a valid url", nameof(endpoint));
                }

                var streamContent = new StreamContent(memoryStream);
                tasks.Add(_httpClient.PostAsync(targetUrl, streamContent));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            Exception exception = null;
            foreach (var task in tasks)
            {
                var response = await task;
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            streamsToDispose.ForEach(s => s.Dispose());

            if (exception != null)
            {
                throw exception;
            }
        }
    }

    public interface IMetricPushService
    {
        /// <summary>
        /// Push metrics to single pushgateway endpoint
        /// </summary>
        /// <param name="metricFamilies">Collection of metrics</param>
        /// <param name="endpoint">PushGateway endpoint</param>
        /// <param name="job">job name</param>
        /// <param name="instance">instance name</param>
        /// <param name="contentType">content-type</param>
        /// <returns></returns>
        Task PushAsync(IEnumerable<CMetricFamily> metricFamilies, string endpoint, string job, string instance,
            string contentType);
        /// <summary>
        /// Push metrics to single pushgateway endpoint
        /// </summary>
        /// <param name="metrics">Collection of metrics</param>
        /// <param name="endpoints">PushGateway endpoints - fault-tolerance</param>
        /// <param name="job">job name</param>
        /// <param name="instance">instance name</param>
        /// <param name="contentType">content-type</param>
        /// <returns></returns>
        Task PushAsync(IEnumerable<CMetricFamily> metrics, string[] endpoints, string job, string instance,
            string contentType);
    }
}
