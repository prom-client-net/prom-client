using System.Text;
using Microsoft.AspNetCore.Mvc;
using Prometheus.Client;

namespace WebCoreApplication.Controllers
{
    [Route("[controller]")]
    public class HistogramController : Controller
    {
        private readonly Histogram _histogram = Metrics.CreateHistogram("test_hist", "help_text", "params1");

        [HttpGet]
        public IActionResult Get()
        {
            _histogram.Labels("test1").Observe(1);
            _histogram.Labels("test2").Observe(2);
            return Ok();
        }
    }
}
