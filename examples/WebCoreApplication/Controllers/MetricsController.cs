using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Prometheus.Client;
using Prometheus.Client.Collectors;


namespace WebCoreApplication.Controllers
{
    [Route("[controller]")]
    public class MetricsController : Controller
    {
        [HttpGet]
        public void Get()
        {
            var registry = CollectorRegistry.Instance;
            var acceptHeaders = Request.Headers["Accept"];
            var contentType = ScrapeHandler.GetContentType(acceptHeaders);
            Response.ContentType = contentType;
            Response.StatusCode = 200;
            using (var outputStream = Response.Body)
            {
                var collected = registry.CollectAll();
                ScrapeHandler.ProcessScrapeRequest(collected, contentType, outputStream);
            }
        }
    }
}
