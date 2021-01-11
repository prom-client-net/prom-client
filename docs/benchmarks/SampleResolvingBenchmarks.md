# Sample Resolving Benchmarks

## Legend
* **ResolveLabeled** - performs 10 000 resolving of the sample (call WithLabels method) with 1000 unique combination of labels.

## Benchmark Results

<details>
  <summary>Counter</summary>
  
|                    Method |     Mean |     Error |    StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------- |---------:|----------:|----------:|------:|--------:|------:|------:|------:|----------:|
|   ResolveLabeled_Baseline | 3.880 ms | 0.0897 ms | 0.1033 ms |  1.00 |    0.00 |     - |     - |     - | 2000000 B |
|      ResolveLabeled_Array | 2.498 ms | 0.0426 ms | 0.0491 ms |  0.64 |    0.02 |     - |     - |     - |  640000 B |
|      ResolveLabeled_Tuple | 2.310 ms | 0.0335 ms | 0.0386 ms |  0.60 |    0.02 |     - |     - |     - |         - |
| ResolveLabeled_Int64Array | 2.499 ms | 0.0331 ms | 0.0382 ms |  0.64 |    0.02 |     - |     - |     - |  640000 B |
| ResolveLabeled_Int64Tuple | 2.202 ms | 0.0214 ms | 0.0247 ms |  0.57 |    0.02 |     - |     - |     - |         - |
  
</details>

<details>
  <summary>Gauge</summary>

|                    Method |     Mean |     Error |    StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------- |---------:|----------:|----------:|------:|--------:|------:|------:|------:|----------:|
|   ResolveLabeled_Baseline | 3.846 ms | 0.0494 ms | 0.0569 ms |  1.00 |    0.00 |     - |     - |     - | 2000000 B |
|      ResolveLabeled_Array | 2.524 ms | 0.0318 ms | 0.0367 ms |  0.66 |    0.01 |     - |     - |     - |  640000 B |
|      ResolveLabeled_Tuple | 2.193 ms | 0.0161 ms | 0.0185 ms |  0.57 |    0.01 |     - |     - |     - |         - |
| ResolveLabeled_Int64Array | 2.652 ms | 0.0423 ms | 0.0487 ms |  0.69 |    0.02 |     - |     - |     - |  640000 B |
| ResolveLabeled_Int64Tuple | 2.355 ms | 0.0573 ms | 0.0660 ms |  0.61 |    0.02 |     - |     - |     - |         - |

</details>

<details>
  <summary>Histogram</summary>

|                  Method |     Mean |     Error |    StdDev | Ratio | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------ |---------:|----------:|----------:|------:|------:|------:|------:|----------:|
| ResolveLabeled_Baseline | 3.955 ms | 0.0681 ms | 0.0784 ms |  1.00 |     - |     - |     - | 2000000 B |
|    ResolveLabeled_Array | 2.545 ms | 0.0250 ms | 0.0288 ms |  0.64 |     - |     - |     - |  640000 B |
|   ResolveLabeled_Tuples | 2.257 ms | 0.0194 ms | 0.0224 ms |  0.57 |     - |     - |     - |         - |

</details>

<details>
  <summary>Summary</summary>

|                  Method |     Mean |     Error |    StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------ |---------:|----------:|----------:|------:|--------:|------:|------:|------:|----------:|
| ResolveLabeled_Baseline | 3.963 ms | 0.1823 ms | 0.2100 ms |  1.00 |    0.00 |     - |     - |     - | 2000000 B |
|    ResolveLabeled_Array | 2.429 ms | 0.0478 ms | 0.0551 ms |  0.61 |    0.03 |     - |     - |     - |  640000 B |
|    ResolveLabeled_Tuple | 2.338 ms | 0.2923 ms | 0.3366 ms |  0.59 |    0.09 |     - |     - |     - |         - |

</details>

## Conclusions
Resolving labels via ValueTuple shows no allocation at all, params array also has some optimization on our side. 
