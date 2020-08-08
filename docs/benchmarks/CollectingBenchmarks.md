# Metric Collecting Benchmarks

## Legend
* **Collecting** - system generates 100 counters with 100 unique samples for each and performs scrape

## Benchmark Results

<details>
  <summary>Counter</summary>
  
```
|              Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------- |----------:|---------:|---------:|------:|--------:|------:|------:|------:|----------:|
| Collecting_Baseline |  5.877 ms | 1.340 ms | 1.543 ms |  1.00 |    0.00 |     - |     - |     - | 406.96 KB |
|          Collecting | 15.325 ms | 4.407 ms | 5.075 ms |  2.72 |    0.83 |     - |     - |     - |   6.57 KB |
```
  
</details>

<details>
  <summary>Gauge</summary>

```
|              Method |      Mean |    Error |   StdDev |    Median | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------- |----------:|---------:|---------:|----------:|------:|--------:|------:|------:|------:|----------:|
| Collecting_Baseline |  5.907 ms | 1.401 ms | 1.614 ms |  6.648 ms |  1.00 |    0.00 |     - |     - |     - | 406.93 KB |
|          Collecting | 17.514 ms | 5.210 ms | 5.999 ms | 13.524 ms |  3.04 |    0.76 |     - |     - |     - |   6.57 KB |
```

</details>

<details>
  <summary>Histogram</summary>

```
|              Method |      Mean |    Error |   StdDev | Ratio | RatioSD |     Gen 0 | Gen 1 | Gen 2 |  Allocated |
|-------------------- |----------:|---------:|---------:|------:|--------:|----------:|------:|------:|-----------:|
| Collecting_Baseline |  73.26 ms | 6.958 ms | 8.013 ms |  1.00 |    0.00 | 1000.0000 |     - |     - | 5405.19 KB |
|          Collecting | 192.72 ms | 1.541 ms | 1.775 ms |  2.65 |    0.21 |         - |     - |     - |     7.6 KB |
```

</details>

<details>
  <summary>Summary</summary>

```
|              Method |     Mean |    Error |   StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 |  Allocated |
|-------------------- |---------:|---------:|---------:|------:|--------:|------:|------:|------:|-----------:|
| Collecting_Baseline | 17.16 ms | 2.583 ms | 2.975 ms |  1.00 |    0.00 |     - |     - |     - | 1031.99 KB |
|          Collecting | 77.84 ms | 1.061 ms | 1.222 ms |  4.64 |    0.61 |     - |     - |     - |    6.66 KB |
```

</details>

## Conclusions
Current version of the library shows significant less allocation, but much poorer performance with comparison to prometheus-net. Profiling indicates that most of the time spend on converting strings to Utf8 encoding. Upcoming Net5 release includes Uft8String class that should help in this case.
