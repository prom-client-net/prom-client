# Comparison Benchmarks

There are number of benchmarks in Prometheus.Client.Benchmarks.Comparison project dedicated to compare performance and memory allocation of the Prometheus.Client library with prometheus-net. All benchamrks were done for Prometheus.Client v 4.0.0 and prometheus-net 3.6.0.

## General structure

All benchmarks are grouped by usage aspect (for example: metrics creation, collecting, etc).

Each benchmark has multiple measures:
* **baseline** - it's measure of the scenario implemented by prometheus-net library.
* **array** - implementation by Prometheus.Client library by providing labels as a params array.
* **tuple** - implementation by Prometheus.Client library by providing labels via tuple (new labels API introduced in v4).
* for Counter and Gauge there are additional measures with **Int64** suffix for metrics based on int64 value.

## Benchmarks List

* [General use case](GeneralUseCase.md)
* [Collecting benchmarks](CollectingBenchmarks.md)
* [Metric creation](CreationBenchmarks.md)
* [Sample resolving benchmarks](SampleResolvinfBenchmarks.md)
* [Sample benchmarks](SampleBenchmarks.md)

## Hardware

All benchmarks were created for the following hardware.

```

BenchmarkDotNet=v0.12.0, OS=ubuntu 20.04
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.101
  [Host]     : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT
  Job-GLKCBR : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT

```

Fill free to contact us if there is any additional benchmarks that should be done or any errors in the results interpretation.
