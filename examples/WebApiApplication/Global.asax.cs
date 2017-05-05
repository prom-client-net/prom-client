using System.Web.Http;
using Prometheus.Client.Advanced;

namespace WebApiApplication
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            CollectorRegistry.Instance.RegisterOnDemandCollectors(new[] { new DotNetStatsCollector() });
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
