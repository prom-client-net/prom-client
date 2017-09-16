# Prometheus.Client

[![Build status](https://ci.appveyor.com/api/projects/status/ik6p8hv9he1cl0a9?svg=true)](https://ci.appveyor.com/project/phnx47/prometheus-client) [![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT) [![NuGet Badge](https://buildstats.info/nuget/Prometheus.Client)](https://www.nuget.org/packages/Prometheus.Client/) 

This is .NET version (unofficial). Support .net45 and .netstandart1.3

Its a fork of [prometheus-net](https://github.com/andrasm/prometheus-net)


## Quik start

Nuget package: [Prometheus.Client](https://www.nuget.org/packages/Prometheus.Client)

OWIN: [Prometheus.Client.Owin](https://github.com/phnx47/Prometheus.Client.Owin)

MetricServer: [Prometheus.Client.MetricServer](https://github.com/phnx47/Prometheus.Client.MetricServer)


```csharp

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    var options = new PrometheusOptions();
    app.UsePrometheusServer(options);
}

```

Or Standalone this:

```csharp

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

```



See prometheus [here](http://prometheus.io/)


### Instrumenting

Four types of metric are offered: Counter, Gauge, Summary and Histogram.
See the documentation on [metric types](http://prometheus.io/docs/concepts/metric_types/)
and [instrumentation best practices](http://prometheus.io/docs/practices/instrumentation/#counter-vs.-gauge-vs.-summary)
on how to use them.

### Counter

Counters go up, and reset when the process restarts.


```csharp
var counter = Metrics.CreateCounter("myCounter", "some help about this");
counter.Inc(5.5);
```

### Gauge

Gauges can go up and down.


```csharp
var gauge = Metrics.CreateGauge("gauge", "help text");
gauge.Inc(3.4);
gauge.Dec(2.1);
gauge.Set(5.3);
```

### Summary

Summaries track the size and number of events.

```csharp
var summary = Metrics.CreateSummary("mySummary", "help text");
summary.Observe(5.3);
```

### Histogram

Histograms track the size and number of events in buckets.
This allows for aggregatable calculation of quantiles.

```csharp
var hist = Metrics.CreateHistogram("my_histogram", "help text", buckets: new[] { 0, 0.2, 0.4, 0.6, 0.8, 0.9 });
hist.Observe(0.4);
```

The default buckets are intended to cover a typical web/rpc request from milliseconds to seconds.
They can be overridden passing in the `buckets` argument.

### Labels

All metrics can have labels, allowing grouping of related time series.

See the best practices on [naming](http://prometheus.io/docs/practices/naming/)
and [labels](http://prometheus.io/docs/practices/instrumentation/#use-labels).

Taking a counter as an example:

```csharp
var counter = Metrics.CreateCounter("myCounter", "help text", labelNames: new []{ "method", "endpoint"});
counter.Labels("GET", "/").Inc();
counter.Labels("POST", "/cancel").Inc();
```

## Unit testing
For simple usage the API uses static classes, which - in unit tests - can cause errors like this: "A collector with name '<NAME>' has already been registered!"

To address this you can add this line to your test setup:

```csharp
CollectorRegistry.Instance.Clear();
```
