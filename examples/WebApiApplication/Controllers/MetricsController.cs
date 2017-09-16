using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Web.Http;
using Prometheus.Client;
using Prometheus.Client.Collectors;

namespace WebApiApplication.Controllers
{
    public class MetricsController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var registry = CollectorRegistry.Instance;
            var acceptHeaders = Request.Headers.GetValues("Accept");

            var contentType = ScrapeHandler.GetContentType(acceptHeaders);

         
            string content;

            using (var outputStream = new MemoryStream())
            {
                var collected = registry.CollectAll();
                ScrapeHandler.ProcessScrapeRequest(collected, contentType, outputStream);
                content = Encoding.ASCII.GetString(outputStream.ToArray());
            }


            var response = Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(content, Encoding.UTF8, contentType);


            return response;

        }
    }
}
