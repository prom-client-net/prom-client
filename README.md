# Prometheus.Client

[asp-net-core]: https://github.com/prom-client-net/prom-client-aspnetcore
[dependency-injection]: https://github.com/prom-client-net/prom-client-dependencyinjection
[http-request-durations]: https://github.com/prom-client-net/prom-client-httprequestdurations
[metric-pusher]: https://github.com/prom-client-net/prom-client-metricpusher
[metric-pusher-hosted-service]: https://github.com/prom-client-net/prom-client-metricpusher-hostedservice
[health-checks]: https://github.com/prom-client-net/prom-client-healthchecks
[metric-server]: https://github.com/prom-client-net/prom-client-metricserver
[owin]: https://github.com/prom-client-net/prom-client-owin

[![ci](https://img.shields.io/github/actions/workflow/status/prom-client-net/prom-client/ci.yml?branch=main&label=ci&logo=github&style=flat-square)](https://github.com/prom-client-net/prom-client/actions/workflows/ci.yml)
[![nuget](https://img.shields.io/nuget/v/Prometheus.Client?logo=nuget&style=flat-square)](https://www.nuget.org/packages/Prometheus.Client)
[![nuget](https://img.shields.io/nuget/dt/Prometheus.Client?logo=nuget&style=flat-square)](https://www.nuget.org/packages/Prometheus.Client)
[![codecov](https://img.shields.io/codecov/c/github/prom-client-net/prom-client?logo=codecov&style=flat-square)](https://app.codecov.io/gh/prom-client-net/prom-client)
[![license](https://img.shields.io/github/license/prom-client-net/prom-client?style=flat-square)](https://github.com/prom-client-net/prom-client/blob/main/LICENSE)

.NET Client library for [prometheus.io](https://prometheus.io/)

It is hard fork of [prometheus-net](https://github.com/prometheus-net/prometheus-net) from early 2017 that has since evolved into a different library.

Our main goals:

- Keep possibility of rapid development.
- Extensibility is one of the core values, together with performance and minimal allocation.
- We are open for suggestions and new ideas, contribution is always welcomed.

You can find the [benchmark descriptions](https://github.com/prom-client-net/prom-client/blob/main/docs/benchmarks).

## Install

```sh
dotnet add package Prometheus.Client
```

### Extensions

| Name                                                                         | Description                            |
| ---------------------------------------------------------------------------- | -------------------------------------- |
| [Prometheus.Client.AspNetCore][asp-net-core]                                 | ASP.NET Core middleware                |
| [Prometheus.Client.DependencyInjection][dependency-injection]                | Dependency Injection extensions        |
| [Prometheus.Client.HttpRequestDurations][http-request-durations]             | Metrics logging of request durations   |
| [Prometheus.Client.MetricPusher][metric-pusher]                              | Push metrics to Prometheus PushGateway |
| [Prometheus.Client.MetricPusher.HostedService][metric-pusher-hosted-service] | MetricPusher as HostedService          |
| [Prometheus.Client.HealthChecks][health-checks]                              | HealthChecks Publisher                 |
| [Prometheus.Client.MetricServer][metric-server]                              | Standalone Kestrel server              |
| [Prometheus.Client.Owin][owin]                                               | Owin middleware                        |

## Use

[Examples](https://github.com/prom-client-net/prom-examples)

[Prometheus Docs](https://prometheus.io/docs/introduction/overview/)

Add metrics endpoint without extension:

```c#
[Route("[controller]")]
public class MetricsController : Controller
{
    private readonly ICollectorRegistry _registry;

    public MetricsController(ICollectorRegistry registry)
    {
        _registry = registry;
    }

    [HttpGet]
    public async Task Get()
    {
        Response.StatusCode = 200;
        await using var outputStream = Response.Body;
        await ScrapeHandler.ProcessAsync(_registry, outputStream);
    }
}
```

Add metrics endpoint with [Prometheus.Client.AspNetCore][asp-net-core]:

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer();
}
```

`IMetricFactory` and `ICollectorRegistry` can be added to DI container with [Prometheus.Client.DependencyInjection][dependency-injection]:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMetricFactory();
}
```

For collect http requests, use [Prometheus.Client.HttpRequestDurations][http-request-durations].
It does not depend on [Prometheus.Client.AspNetCore][asp-net-core], however together it's very convenient to use:

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer();
    app.UsePrometheusRequestDurations();
}
```

### Instrumenting

Four types of metric are offered: `Counter`, `Gauge`, `Summary` and `Histogram`.
See the documentation on [metric types](http://prometheus.io/docs/concepts/metric_types/)
and [instrumentation best practices](http://prometheus.io/docs/practices/instrumentation/#counter-vs.-gauge-vs.-summary)
on how to use them.

### Counter

Counters go up, and reset when the process restarts.

```c#
var counter = metricFactory.CreateCounter("myCounter", "some help about this");
counter.Inc(5.5);
```

### Gauge

Gauges can go up and down.

```c#
var gauge = metricFactory.CreateGauge("gauge", "help text");
gauge.Inc(3.4);
gauge.Dec(2.1);
gauge.Set(5.3);
```

### Summary

Summaries track the size and number of events.

```c#
var summary = metricFactory.CreateSummary("mySummary", "help text");
summary.Observe(5.3);
```

### Histogram

Histograms track the size and number of events in buckets.
This allows for aggregate calculation of quantiles.

```c#
var hist = metricFactory.CreateHistogram("my_histogram", "help text", buckets: new[] { 0, 0.2, 0.4, 0.6, 0.8, 0.9 });
hist.Observe(0.4);
```

The default buckets are intended to cover a typical web/rpc request from milliseconds to seconds.
They can be overridden passing in the `buckets` argument.

### Labels

All metrics can have labels, allowing grouping of related time series.

See the best practices on [naming](http://prometheus.io/docs/practices/naming/)
and [labels](http://prometheus.io/docs/practices/instrumentation/#use-labels).

Taking a counter as an example:

```c#
var counter = metricFactory.CreateCounter("myCounter", "help text", labelNames: new []{ "method", "endpoint"});
counter.WithLabels("GET", "/").Inc();
counter.WithLabels("POST", "/cancel").Inc();
```

Since v4 there is alternative new way to provide a labels via ValueTuple that allow to reduce memory allocation:

```c#
var counter = metricFactory.CreateCounter("myCounter", "help text", labelNames: ("method", "endpoint"));
counter.WithLabels(("GET", "/")).Inc();
counter.WithLabels(("POST", "/cancel")).Inc();
```

## Contribute

Contributions to the package are always welcome!

- Report any bugs or issues you find on the [issue tracker](https://github.com/prom-client-net/prom-client/issues).
- You can grab the source code at the package's [git repository](https://github.com/prom-client-net/prom-client).

## Supporters

[![JetBrains](https://avatars.githubusercontent.com/u/878437?s=75&v=4)](https://github.com/jetbrains)

We much appreciate free licenses provided by [JetBrains](https://github.com/jetbrains) to support our library.

## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).
