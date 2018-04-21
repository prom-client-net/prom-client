# Prometheus.Client

[![MyGet](https://img.shields.io/myget/phnx47-beta/vpre/Prometheus.Client.svg)](https://www.myget.org/feed/phnx47-beta/package/nuget/Prometheus.Client)
[![NuGet Badge](https://buildstats.info/nuget/Prometheus.Client)](https://www.nuget.org/packages/Prometheus.Client/) 

[![codecov](https://codecov.io/gh/phnx47/Prometheus.Client/branch/master/graph/badge.svg)](https://codecov.io/gh/phnx47/Prometheus.Client)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/7e275fc9eb5d4f47896a3b6eb28c8536)](https://www.codacy.com/app/phnx47/Prometheus.Client?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=phnx47/Prometheus.Client&amp;utm_campaign=Badge_Grade)
[![Build status](https://ci.appveyor.com/api/projects/status/cyvjrbn46ju827a9/branch/master?svg=true)](https://ci.appveyor.com/project/PrometheusClientNet/prometheus-client/branch/master)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT) 

.NET Client library(unofficial) for [prometheus.io](https://prometheus.io/)  

Support .net45, .netstandard1.3 and .netstandard2.0

It's a fork of [prometheus-net](https://github.com/prometheus-net/prometheus-net)

## Installation

    dotnet add package Prometheus.Client


Extension for WEB: [Prometheus.Client.Owin](https://github.com/phnx47/Prometheus.Client.Owin)

	dotnet add package Prometheus.Client.Owin

Extension for Standalone host: [Prometheus.Client.MetricServer](https://github.com/phnx47/Prometheus.Client.MetricServer)

	dotnet add package Prometheus.Client.MetricServer

Extension for collect http request duration from all requests: [Prometheus.Client.HttpRequestDurations](https://github.com/phnx47/Prometheus.Client.HttpRequestDurations)

	dotnet add package Prometheus.Client.HttpRequestDurations

## Quik start

[Examples](https://github.com/PrometheusClientNet/Prometheus.Client.Examples)

[Prometheus Docs](https://prometheus.io/docs/introduction/overview/)


With Prometheus.Client.Owin:

```csharp

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    var options = new PrometheusOptions();
    app.UsePrometheusServer(options);
}

```

Without extensions:

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
