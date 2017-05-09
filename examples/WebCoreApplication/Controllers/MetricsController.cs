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
        public IActionResult Get()
        {
            var registry = CollectorRegistry.Instance;
            var acceptHeaders = Request.Headers["Accept"];
            var contentType = ScrapeHandler.GetContentType(acceptHeaders);
            Response.ContentType = contentType;
            string content;

            using (var outputStream = new MemoryStream())
            {
                var collected = registry.CollectAll();
                ScrapeHandler.ProcessScrapeRequest(collected, contentType, outputStream);
                content = Encoding.UTF8.GetString(outputStream.ToArray());
            }

            return Ok(content);
        }
    }
}
