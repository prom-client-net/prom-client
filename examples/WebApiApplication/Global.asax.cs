using System.Web.Http;

using Prometheus.Client.Collectors;

namespace WebApiApplication
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            CollectorRegistry.Instance.RegisterOnDemandCollectors(new IOnDemandCollector[] { new DotNetStatsCollector(), new WindowsDotNetStatsCollector(), new PerfCounterCollector()   });
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
