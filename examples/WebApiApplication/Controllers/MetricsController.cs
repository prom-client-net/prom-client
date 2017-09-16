using System.Web;
using System.Web.Http;
using Prometheus.Client;
using Prometheus.Client.Collectors;

namespace WebApiApplication.Controllers
{
    public class MetricsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var registry = CollectorRegistry.Instance;
            var acceptHeaders = Request.Headers.GetValues("Accept");
            var contentType = ScrapeHandler.GetContentType(acceptHeaders);
            var reponse = HttpContext.Current.Response;
            reponse.ContentType = contentType;
            reponse.StatusCode = 200;

            using (var outputStream = reponse.OutputStream)
            {
                var collected = registry.CollectAll();
                ScrapeHandler.ProcessScrapeRequest(collected, contentType, outputStream);
            }

            return Ok();

        }
    }
}
