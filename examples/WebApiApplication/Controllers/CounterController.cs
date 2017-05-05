using System.Web.Http;
using Prometheus.Client;

namespace WebApiApplication.Controllers
{
    public class CounterController : ApiController
    {
        readonly Counter _counter = Metrics.CreateCounter("myCounter", "some help about this");

        [HttpGet]
        public IHttpActionResult Get()
        {
            _counter.Inc();
            return Ok();
        }
    }
}
