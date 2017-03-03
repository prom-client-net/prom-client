using System.Linq;
using System.Threading.Tasks;
using Prometheus.Client.Advanced;

#if COREFX

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Primitives;

#else
using Owin;
#endif

namespace Prometheus.Client.Owin
{
    public static class PrometheusExtensions
    {
#if COREFX

        public static IApplicationBuilder UsePrometheusServer(this IApplicationBuilder app, PrometheusOptions options = null)
        {
            if (options == null)
                options = new PrometheusOptions();

            if (options.CollectorRegistry == PrometheusCollectorRegistry.Instance)
            {
                if (options.Collectors != null && !options.Collectors.Any() && options.CollectorLocator != null)
                    options.Collectors.AddRange(options.CollectorLocator.Get());

                PrometheusCollectorRegistry.Instance.RegisterOnDemandCollectors(options.Collectors);
            }

            app.Map(string.Format("/{0}", options.MapPath), coreapp =>
            {
                coreapp.Run(async context =>
                {
                    var req = context.Request;
                    var response = context.Response;

                    StringValues acceptHeaders;
                    req.Headers.TryGetValue("Accept", out acceptHeaders);
                    var contentType = ScrapeHandler.GetContentType(acceptHeaders);

                    response.ContentType = contentType;

                    using (var outputStream = response.Body)
                    {
                        var collected = options.CollectorRegistry.CollectAll();
                        ScrapeHandler.ProcessScrapeRequest(collected, contentType, outputStream);
                    }

                    await Task.FromResult(0).ConfigureAwait(false);
                });
            });

            return app;
        }

#else

        public static IAppBuilder UsePrometheusServer(this IAppBuilder app, PrometheusOptions options = null)
        {
            if (options == null)
                options = new PrometheusOptions();

            if (options.CollectorRegistry == PrometheusCollectorRegistry.Instance)
            {
                if (options.Collectors != null && !options.Collectors.Any() && options.CollectorLocator != null)
                    options.Collectors.AddRange(options.CollectorLocator.Get());

                PrometheusCollectorRegistry.Instance.RegisterOnDemandCollectors(options.Collectors);
            }

            app.Map($"/{options.MapPath}", coreapp =>
            {
                coreapp.Run(async context =>
                {
                    var req = context.Request;
                    var response = context.Response;

                    var acceptHeader = req.Headers.Get("Accept");
                    var acceptHeaders = acceptHeader?.Split(',');
                    var contentType = ScrapeHandler.GetContentType(acceptHeaders);

                    response.ContentType = contentType;

                    using (var outputStream = response.Body)
                    {
                        var collected = options.CollectorRegistry.CollectAll();
                        ScrapeHandler.ProcessScrapeRequest(collected, contentType, outputStream);
                    }

                    await Task.FromResult(0).ConfigureAwait(false);
                });
            });

            return app;
        }
#endif
    }
}