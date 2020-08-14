# Prometheus.Client

[![NuGet](https://img.shields.io/nuget/v/Prometheus.Client.svg)](https://www.nuget.org/packages/Prometheus.Client)
[![NuGet](https://img.shields.io/nuget/dt/Prometheus.Client.svg)](https://www.nuget.org/packages/Prometheus.Client)
[![Gitter](https://img.shields.io/gitter/room/PrometheusClientNet/community.svg)](https://gitter.im/PrometheusClientNet/community)

[![CodeFactor](https://www.codefactor.io/repository/github/prometheusclientnet/prometheus.client/badge)](https://www.codefactor.io/repository/github/prometheusclientnet/prometheus.client)
[![CI](https://github.com/PrometheusClientNet/Prometheus.Client/workflows/CI/badge.svg)](https://github.com/PrometheusClientNet/Prometheus.Client/actions?query=workflow%3ACI)
[![codecov](https://codecov.io/gh/PrometheusClientNet/Prometheus.Client/branch/master/graph/badge.svg)](https://codecov.io/gh/PrometheusClientNet/Prometheus.Client)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT) 

.NET Client library for [prometheus.io](https://prometheus.io/)  
Supports: 
<img src="https://img.shields.io/badge/.netstandard-2.0-green.svg"></img>
<img src="https://img.shields.io/badge/.netstandard-2.1-green.svg"></img>
<img src="https://img.shields.io/badge/.netcore-2.2-green.svg"></img>

It was started as a fork of [prometheus-net](https://github.com/prometheus-net/prometheus-net), but over time the library was evolved into a different product. Our main goals:
- Keep possibility of rapid development.
- Extensibility is one of the core values, together with performance and minimal allocation.
- We are open for suggestions and new ideas, contribution is always welcomed.

## Performance comparison with prometheus-net
![General use case benchmarks](/docs/benchmarks/generalcase.png)
Find more details on [benchmarks description](/docs/benchmarks/GeneralUseCase.md)

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
    public async Task Get()
    {
        Response.StatusCode = 200;
        using (var outputStream = Response.Body)
        {
            await ScrapeHandler.ProcessAsync(Metrics.DefaultCollectorRegistry, outputStream);
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
counter.WithLabels("GET", "/").Inc();
counter.WithLabels("POST", "/cancel").Inc();
```

Since v4 there is alternative new way to provide a labels via ValueTuple that allow to reduce memory allocation:
```csharp
var counter = Metrics.CreateCounter("myCounter", "help text", labelNames: ("method", "endpoint"));
counter.WithLabels(("GET", "/")).Inc();
counter.WithLabels(("POST", "/cancel")).Inc();
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

## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).
