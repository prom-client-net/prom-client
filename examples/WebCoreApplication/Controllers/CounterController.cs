using System.Text;
using Microsoft.AspNetCore.Mvc;
using Prometheus.Client;

namespace WebCoreApplication.Controllers
{
    [Route("[controller]")]
    public class CounterController : Controller
    {
        readonly Counter _counter = Metrics.CreateCounter("myCounter", "some help about this");

        [HttpGet]
        public IActionResult Get()
        {
            _counter.Inc();
            return Ok();
        }
    }
}
