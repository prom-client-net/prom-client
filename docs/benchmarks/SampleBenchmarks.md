# Sample Benchmarks

## Legend
Benchmarks were created for each of the value observation methods. Some methods also has default values and micro-optimization, so there are additional benchmarks with _Default_ suffix.
Each benchmark 

## Benchmark Results

<details>
  <summary>Counter</summary>
  
Each benchmark has 10 000 000 observations.

|              Method |      Mean |    Error |   StdDev |    Median | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------- |----------:|---------:|---------:|----------:|------:|--------:|------:|------:|------:|----------:|
| IncDefault_Baseline | 147.66 ms | 1.983 ms | 2.284 ms | 146.21 ms |  1.00 |    0.00 |     - |     - |     - |         - |
|          IncDefault | 146.82 ms | 1.489 ms | 1.715 ms | 145.90 ms |  0.99 |    0.02 |     - |     - |     - |         - |
|    IncDefault_Int64 |  86.51 ms | 1.179 ms | 1.358 ms |  86.67 ms |  0.59 |    0.01 |     - |     - |     - |         - |
|                     |           |          |          |           |       |         |       |       |       |           |
|        Inc_Baseline | 150.48 ms | 3.195 ms | 3.679 ms | 150.36 ms |  1.00 |    0.00 |     - |     - |     - |         - |
|                 Inc | 149.80 ms | 1.791 ms | 2.062 ms | 150.76 ms |  1.00 |    0.02 |     - |     - |     - |         - |
|           Inc_Int64 |  91.95 ms | 1.128 ms | 1.299 ms |  91.09 ms |  0.61 |    0.02 |     - |     - |     - |         - |
  
</details>

<details>
  <summary>Gauge</summary>

Each benchmark has 10 000 000 observations.

|              Method |      Mean |    Error |   StdDev |    Median | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------- |----------:|---------:|---------:|----------:|------:|--------:|------:|------:|------:|----------:|
| IncDefault_Baseline | 158.44 ms | 3.879 ms | 4.468 ms | 158.01 ms |  1.00 |    0.00 |     - |     - |     - |         - |
|          IncDefault | 148.08 ms | 1.753 ms | 2.019 ms | 148.89 ms |  0.94 |    0.03 |     - |     - |     - |         - |
|    IncDefault_Int64 |  86.82 ms | 0.753 ms | 0.868 ms |  87.14 ms |  0.55 |    0.01 |     - |     - |     - |         - |
|                     |           |          |          |           |       |         |       |       |       |           |
|        Inc_Baseline | 156.63 ms | 1.946 ms | 2.241 ms | 157.18 ms |  1.00 |    0.00 |     - |     - |     - |         - |
|                 Inc | 147.29 ms | 1.228 ms | 1.414 ms | 146.77 ms |  0.94 |    0.01 |     - |     - |     - |         - |
|           Inc_Int64 |  88.62 ms | 1.021 ms | 1.176 ms |  87.89 ms |  0.57 |    0.01 |     - |     - |     - |         - |
|                     |           |          |          |           |       |         |       |       |       |           |
| DecDefault_Baseline | 154.92 ms | 1.757 ms | 2.023 ms | 153.81 ms |  1.00 |    0.00 |     - |     - |     - |         - |
|          DecDefault | 147.49 ms | 1.701 ms | 1.959 ms | 145.82 ms |  0.95 |    0.02 |     - |     - |     - |         - |
|    DecDefault_Int64 |  86.75 ms | 1.103 ms | 1.270 ms |  87.62 ms |  0.56 |    0.01 |     - |     - |     - |         - |
|                     |           |          |          |           |       |         |       |       |       |           |
|        Dec_Baseline | 155.86 ms | 2.025 ms | 2.332 ms | 154.09 ms |  1.00 |    0.00 |     - |     - |     - |         - |
|                 Dec | 149.84 ms | 1.993 ms | 2.295 ms | 150.97 ms |  0.96 |    0.03 |     - |     - |     - |         - |
|           Dec_Int64 |  89.29 ms | 1.155 ms | 1.331 ms |  89.95 ms |  0.57 |    0.01 |     - |     - |     - |         - |
|                     |           |          |          |           |       |         |       |       |       |           |
|        Set_Baseline |  93.83 ms | 0.050 ms | 0.058 ms |  93.81 ms |  1.00 |    0.00 |     - |     - |     - |         - |
|                 Set |  96.51 ms | 0.968 ms | 1.115 ms |  97.12 ms |  1.03 |    0.01 |     - |     - |     - |         - |
|           Set_Int64 |  88.67 ms | 1.130 ms | 1.301 ms |  87.73 ms |  0.94 |    0.01 |     - |     - |     - |         - |

</details>

<details>
  <summary>Histogram</summary>

Each benchmark has 1 000 000 observations.

|                       Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------------- |----------:|---------:|---------:|------:|--------:|------:|------:|------:|----------:|
|             Observe_Baseline |  34.85 ms | 0.933 ms | 1.075 ms |  1.00 |    0.00 |     - |     - |     - |         - |
|                      Observe |  33.41 ms | 1.929 ms | 2.221 ms |  0.96 |    0.03 |     - |     - |     - |         - |
|                              |           |          |          |       |         |       |       |       |           |
| ManyBuckets_Observe_Baseline | 130.58 ms | 1.892 ms | 2.179 ms |  1.00 |    0.00 |     - |     - |     - |         - |
|          ManyBuckets_Observe |  65.26 ms | 3.880 ms | 4.468 ms |  0.50 |    0.03 |     - |     - |     - |         - |

</details>

<details>
  <summary>Summary</summary>

Each benchmark has 100 000 observations.

|           Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |----------:|---------:|---------:|------:|--------:|------:|------:|------:|----------:|
| Observe_Baseline | 100.02 ms | 2.123 ms | 2.445 ms |  1.00 |    0.00 |     - |     - |     - |   64000 B |
|          Observe |  82.64 ms | 2.036 ms | 2.344 ms |  0.83 |    0.02 |     - |     - |     - |         - |

</details>

## Conclusions
Int64 Counter and Gauge show significant improvement in observation performance. Double-based metrics has relatively same performance.
