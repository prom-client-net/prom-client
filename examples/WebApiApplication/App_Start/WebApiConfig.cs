using System.Web.Http;

namespace WebApiApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
