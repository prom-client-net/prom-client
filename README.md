# Prometheus.Client

[![MyGet](https://img.shields.io/myget/phnx47-beta/vpre/Prometheus.Client.svg)](https://www.myget.org/feed/phnx47-beta/package/nuget/Prometheus.Client)
[![NuGet](https://img.shields.io/nuget/v/Prometheus.Client.svg)](https://www.nuget.org/packages/Prometheus.Client)
[![NuGet](https://img.shields.io/nuget/dt/Prometheus.Client.svg)](https://www.nuget.org/packages/Prometheus.Client)

[![Build status](https://ci.appveyor.com/api/projects/status/cyvjrbn46ju827a9/branch/master?svg=true)](https://ci.appveyor.com/project/PrometheusClientNet/prometheus-client/branch/master)
[![AppVeyor tests](https://img.shields.io/appveyor/tests/PrometheusClientNet/prometheus-client.svg)](https://ci.appveyor.com/project/PrometheusClientNet/prometheus-client/build/tests)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT) 

.NET Client library for [prometheus.io](https://prometheus.io/)  

It's a fork of [prometheus-net](https://github.com/prometheus-net/prometheus-net), 
prometheus-net didn't develop a long time. This is alternative was created with the possibility of rapid development:

- Support <img src="https://img.shields.io/badge/.netstandard-1.3-green.svg"></img> 
and <img src="https://img.shields.io/badge/.netstandard-2.0-green.svg"></img>.
- More Extensions. Extensions extracted to packages. 
- There are differences in the internal implementation: MetricServer, MetricPusher, Middleware.
- Each build is publish to MyGet. There is an opportunity to test development versions.
- All developments in prometheus-net will be analyzed and will implement as necessary.
- The goal is to be able to flexibly write extensions. And keep the speed of development.


## Installation

    dotnet add package Prometheus.Client
	

## Configuration

[Examples](https://github.com/PrometheusClientNet/Prometheus.Client.Examples)

[Prometheus Docs](https://prometheus.io/docs/introduction/overview/)


With Prometheus.Client.AspNetCore:

```csharp

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer();
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
        Response.StatusCode = 200;
        using (var outputStream = Response.Body)
        {
            ScrapeHandler.Process(CollectorRegistry.Instance, outputStream);
        }
    }
}

```

For collect http requests, use Prometheus.Client.HttpRequestDurations.
It does not depend of Prometheus.Client.AspNetCore, however together it's very convenient to use:

```csharp

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer();
    app.UsePrometheusRequestDurations(); 
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

## Extensions
	
AspNetCore Middleware: [Prometheus.Client.AspNetCore](https://github.com/PrometheusClientNet/Prometheus.Client.AspNetCore)	
	
	dotnet add package Prometheus.Client.AspNetCore

Standalone host: [Prometheus.Client.MetricServer](https://github.com/PrometheusClientNet/Prometheus.Client.MetricServer)

	dotnet add package Prometheus.Client.MetricServer
	
Push metrics to a PushGateaway: [Prometheus.Client.MetricPusher](https://github.com/PrometheusClientNet/Prometheus.Client.MetricPusher)

	dotnet add package Prometheus.Client.MetricPusher

Collect http requests duration: [Prometheus.Client.HttpRequestDurations](https://github.com/PrometheusClientNet/Prometheus.Client.HttpRequestDurations)

	dotnet add package Prometheus.Client.HttpRequestDurations

## Contribute

Contributions to the package are always welcome!

* Report any bugs or issues you find on the [issue tracker](https://github.com/PrometheusClientNet/Prometheus.Client/issues).
* You can grab the source code at the package's [git repository](https://github.com/PrometheusClientNet/Prometheus.Client).

## Support

If you are having problems, send a mail to [prometheus@phnx47.net](mailto://prometheus@phnx47.net). I will try to help you.

I would also very much appreciate your support by buying me a coffee.

<a href="https://www.buymeacoffee.com/phnx47" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/yellow_img.png" alt="Buy Me A Coffee" style="height: auto !important;width: auto !important;" ></a>

## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).
