# Metrics Creation Benchmarks

### Legend

* **Single** - 10 000 calls to create the same metric without labels. Should return the same instance, so allocation should be minimal.
* **SingleWithLabels** - 10 000 calls to create the same metric with labels, but without any samples creation. Should return the same instance, so allocation should be minimal.
* **SingleWithSharedLabels** - 10 000 calls to create the same metric with labels by providing shared array of the label names.
* **Many** - creates 10 000 different metrics without labels.
* **ManyWithLabels** - create 10 000 different metrics with labels.

### Benchmarks

<details>
  <summary>Counter</summary>

|                          Method |      Mean |     Error |    StdDev | Ratio | RatioSD |     Gen 0 |     Gen 1 | Gen 2 |  Allocated |
|-------------------------------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|------:|-----------:|
|                 Single_Baseline |  5.387 ms | 1.1263 ms | 1.2971 ms |  1.00 |    0.00 |         - |         - |     - |  1121944 B |
|                          Single |  1.985 ms | 0.2944 ms | 0.3390 ms |  0.39 |    0.11 |         - |         - |     - |      728 B |
|                    Single_Int64 |  2.128 ms | 0.0964 ms | 0.1111 ms |  0.42 |    0.13 |         - |         - |     - |      728 B |
|                                 |           |           |           |       |         |           |           |       |            |
|       SingleWithLabels_Baseline | 10.183 ms | 2.1404 ms | 2.4649 ms |  1.00 |    0.00 |         - |         - |     - |  1921944 B |
|          SingleWithLabels_Array |  3.382 ms | 0.3140 ms | 0.3616 ms |  0.36 |    0.12 |         - |         - |     - |   508592 B |
|          SingleWithLabels_Tuple |  4.105 ms | 0.1840 ms | 0.2119 ms |  0.43 |    0.13 |         - |         - |     - |     1648 B |
|     SingleWithLabels_Int64Array |  2.960 ms | 0.1510 ms | 0.1739 ms |  0.31 |    0.10 |         - |         - |     - |   508720 B |
|     SingleWithLabels_Int64Tuple |  3.901 ms | 0.2521 ms | 0.2903 ms |  0.41 |    0.14 |         - |         - |     - |     1648 B |
|                                 |           |           |           |       |         |           |           |       |            |
| SingleWithSharedLabels_Baseline |  8.114 ms | 2.2603 ms | 2.6030 ms |  1.00 |    0.00 |         - |         - |     - |  1441944 B |
|          SingleWithSharedLabels |  2.914 ms | 0.1623 ms | 0.1870 ms |  0.40 |    0.12 |         - |         - |     - |    28592 B |
|    SingleWithSharedLabels_Int64 |  2.849 ms | 0.1444 ms | 0.1663 ms |  0.39 |    0.12 |         - |         - |     - |    28720 B |
|                                 |           |           |           |       |         |           |           |       |            |
|                   Many_Baseline | 46.619 ms | 3.5926 ms | 4.1373 ms |  1.00 |    0.00 | 3000.0000 | 1000.0000 |     - | 17243528 B |
|                            Many | 16.445 ms | 1.7709 ms | 2.0393 ms |  0.35 |    0.04 | 1000.0000 |         - |     - |  6414912 B |
|                      Many_Int64 | 17.026 ms | 1.3670 ms | 1.5742 ms |  0.37 |    0.03 | 1000.0000 |         - |     - |  6414912 B |
|                                 |           |           |           |       |         |           |           |       |            |
|         ManyWithLabels_Baseline | 66.306 ms | 4.6032 ms | 5.3011 ms |  1.00 |    0.00 | 3000.0000 | 1000.0000 |     - | 18102184 B |
|            ManyWithLabels_Array | 38.125 ms | 3.5569 ms | 4.0961 ms |  0.58 |    0.05 | 2000.0000 | 1000.0000 |     - | 14922280 B |
|            ManyWithLabels_Tuple | 43.865 ms | 2.6253 ms | 3.0233 ms |  0.66 |    0.03 | 2000.0000 | 1000.0000 |     - | 15614912 B |
|       ManyWithLabels_Int64Array | 39.144 ms | 1.9684 ms | 2.2668 ms |  0.59 |    0.03 | 2000.0000 | 1000.0000 |     - | 14922408 B |
|       ManyWithLabels_Int64Tuple | 42.778 ms | 1.8482 ms | 2.1284 ms |  0.65 |    0.03 | 2000.0000 | 1000.0000 |     - | 15614912 B | 


</details>

<details>
  <summary>Gauge</summary>

|                            Method |      Mean |     Error |    StdDev | Ratio | RatioSD |     Gen 0 |     Gen 1 | Gen 2 |  Allocated |
|---------------------------------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|------:|-----------:|
|                   Single_Baseline |  5.395 ms | 1.0058 ms | 1.1582 ms |  1.00 |    0.00 |         - |         - |     - |  1121912 B |
|                            Single |  1.858 ms | 0.0744 ms | 0.0857 ms |  0.36 |    0.10 |         - |         - |     - |      728 B |
|                      Single_Int64 |  1.872 ms | 0.0489 ms | 0.0563 ms |  0.37 |    0.10 |         - |         - |     - |      728 B |
|                                   |           |           |           |       |         |           |           |       |            |
|         SingleWithLabels_Baseline |  9.376 ms | 0.8831 ms | 1.0170 ms |  1.00 |    0.00 |         - |         - |     - |  1921912 B |
|            SingleWithLabels_Array |  2.924 ms | 0.1037 ms | 0.1194 ms |  0.32 |    0.05 |         - |         - |     - |   508592 B |
|            SingleWithLabels_Tuple |  3.974 ms | 0.1662 ms | 0.1914 ms |  0.43 |    0.08 |         - |         - |     - |     1648 B |
|       SingleWithLabels_Int64Array |  3.076 ms | 0.1825 ms | 0.2102 ms |  0.34 |    0.08 |         - |         - |     - |   508720 B |
|       SingleWithLabels_Int64Tuple |  4.213 ms | 0.1482 ms | 0.1707 ms |  0.46 |    0.08 |         - |         - |     - |     1648 B |
|                                   |           |           |           |       |         |           |           |       |            |
|   SingleWithSharedLabels_Baseline | 10.125 ms | 1.9680 ms | 2.2663 ms |  1.00 |    0.00 |         - |         - |     - |  1441912 B |
|      SingleWithSharedLabels_Array |  2.670 ms | 0.1614 ms | 0.1858 ms |  0.28 |    0.09 |         - |         - |     - |    28592 B |
| SingleWithSharedLabels_Int64Array |  2.814 ms | 0.1775 ms | 0.2044 ms |  0.30 |    0.11 |         - |         - |     - |    28384 B |
|                                   |           |           |           |       |         |           |           |       |            |
|                     Many_Baseline | 39.273 ms | 3.6984 ms | 4.2591 ms |  1.00 |    0.00 | 2000.0000 | 1000.0000 |     - | 17000944 B |
|                              Many | 16.283 ms | 1.4340 ms | 1.6514 ms |  0.42 |    0.04 | 1000.0000 |         - |     - |  6414912 B |
|                        Many_Int64 | 17.036 ms | 1.3380 ms | 1.5408 ms |  0.44 |    0.05 | 1000.0000 |         - |     - |  6414912 B |
|                                   |           |           |           |       |         |           |           |       |            |
|           ManyWithLabels_Baseline | 60.511 ms | 3.3177 ms | 3.8207 ms |  1.00 |    0.00 | 3000.0000 | 1000.0000 |     - | 17924520 B |
|              ManyWithLabels_Array | 37.991 ms | 1.6095 ms | 1.8535 ms |  0.63 |    0.02 | 2000.0000 | 1000.0000 |     - | 14922280 B |
|              ManyWithLabels_Tuple | 41.824 ms | 2.3347 ms | 2.6887 ms |  0.69 |    0.03 | 2000.0000 | 1000.0000 |     - | 15614912 B |
|         ManyWithLabels_Int64Array | 38.847 ms | 1.5547 ms | 1.7904 ms |  0.64 |    0.04 | 2000.0000 | 1000.0000 |     - | 14922408 B |
|         ManyWithLabels_Int64Tuple | 42.357 ms | 1.7864 ms | 2.0572 ms |  0.70 |    0.04 | 2000.0000 | 1000.0000 |     - | 15614912 B | 

</details>

<details>
  <summary>Histogram</summary>

|                          Method |      Mean |     Error |    StdDev | Ratio | RatioSD |     Gen 0 |     Gen 1 | Gen 2 |  Allocated |
|-------------------------------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|------:|-----------:|
|                 Single_Baseline |  5.025 ms | 0.6478 ms | 0.7460 ms |  1.00 |    0.00 |         - |         - |     - |  1122248 B |
|                          Single |  1.907 ms | 0.1376 ms | 0.1585 ms |  0.39 |    0.08 |         - |         - |     - |      816 B |
|                                 |           |           |           |       |         |           |           |       |            |
|       SingleWithLabels_Baseline |  9.886 ms | 1.6353 ms | 1.8832 ms |  1.00 |    0.00 |         - |         - |     - |  2002280 B |
|          SingleWithLabels_Array |  2.915 ms | 0.0983 ms | 0.1133 ms |  0.31 |    0.08 |         - |         - |     - |   508376 B |
|          SingleWithLabels_Tuple |  3.863 ms | 0.2551 ms | 0.2938 ms |  0.41 |    0.11 |         - |         - |     - |     1768 B |
|                                 |           |           |           |       |         |           |           |       |            |
| SingleWithSharedLabels_Baseline |  8.171 ms | 2.0931 ms | 2.4105 ms |  1.00 |    0.00 |         - |         - |     - |  1522280 B |
|          SingleWithSharedLabels |  2.848 ms | 0.1120 ms | 0.1289 ms |  0.38 |    0.10 |         - |         - |     - |    28376 B |
|                                 |           |           |           |       |         |           |           |       |            |
|                   Many_Baseline | 55.796 ms | 3.7634 ms | 4.3339 ms |  1.00 |    0.00 | 3000.0000 | 1000.0000 |     - | 20192368 B |
|                            Many | 18.018 ms | 2.1176 ms | 2.4386 ms |  0.32 |    0.03 | 1000.0000 |         - |     - |  7294912 B |
|                                 |           |           |           |       |         |           |           |       |            |
|         ManyWithLabels_Baseline | 72.071 ms | 5.3968 ms | 6.2150 ms |  1.00 |    0.00 | 3000.0000 | 1000.0000 |     - | 21368080 B |
|            ManyWithLabels_Array | 39.936 ms | 2.5998 ms | 2.9939 ms |  0.55 |    0.02 | 2000.0000 | 1000.0000 |     - | 16121944 B |
|            ManyWithLabels_Tuple | 43.566 ms | 2.8220 ms | 3.2498 ms |  0.61 |    0.02 | 2000.0000 | 1000.0000 |     - | 16814912 B | 

</details>

<details>
  <summary>Summary</summary>

|                          Method |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD |     Gen 0 |     Gen 1 | Gen 2 |  Allocated |
|-------------------------------- |----------:|----------:|----------:|----------:|------:|--------:|----------:|----------:|------:|-----------:|
|                 Single_Baseline |  4.609 ms | 0.2174 ms | 0.2504 ms |  4.515 ms |  1.00 |    0.00 |         - |         - |     - |  1121968 B |
|                          Single |  1.698 ms | 0.0605 ms | 0.0697 ms |  1.685 ms |  0.37 |    0.03 |         - |         - |     - |      904 B |
|                                 |           |           |           |           |       |         |           |           |       |            |
|           SingleLabels_Baseline |  7.324 ms | 1.9180 ms | 2.2088 ms |  8.935 ms |  1.00 |    0.00 |         - |         - |     - |  2162000 B |
|              SingleLabels_Array |  2.343 ms | 0.1610 ms | 0.1855 ms |  2.301 ms |  0.35 |    0.12 |         - |         - |     - |   508800 B |
|              SingleLabels_Tuple |  3.213 ms | 0.1043 ms | 0.1201 ms |  3.217 ms |  0.48 |    0.15 |         - |         - |     - |     1856 B |
|                                 |           |           |           |           |       |         |           |           |       |            |
| SingleWithSharedLabels_Baseline |  7.202 ms | 1.7021 ms | 1.9602 ms |  8.396 ms |  1.00 |    0.00 |         - |         - |     - |  1682000 B |
|          SingleWithSharedLabels |  2.414 ms | 0.1266 ms | 0.1457 ms |  2.375 ms |  0.36 |    0.11 |         - |         - |     - |    28800 B |
|                                 |           |           |           |           |       |         |           |           |       |            |
|                   Many_Baseline | 51.800 ms | 3.9087 ms | 4.5013 ms | 49.757 ms |  1.00 |    0.00 | 3000.0000 | 1000.0000 |     - | 17611160 B |
|                            Many | 18.223 ms | 1.3381 ms | 1.5410 ms | 17.589 ms |  0.35 |    0.02 | 1000.0000 |         - |     - |  8174912 B |
|                                 |           |           |           |           |       |         |           |           |       |            |
|         ManyWithLabels_Baseline | 65.739 ms | 3.6652 ms | 4.2208 ms | 64.706 ms |  1.00 |    0.00 | 3000.0000 | 1000.0000 |     - | 18934680 B |
|            ManyWithLabels_Array | 41.503 ms | 2.4628 ms | 2.8361 ms | 40.307 ms |  0.63 |    0.04 | 2000.0000 | 1000.0000 |     - | 17002280 B |
|            ManyWithLabels_Tuple | 43.879 ms | 2.3097 ms | 2.6599 ms | 42.866 ms |  0.67 |    0.04 | 2000.0000 | 1000.0000 |     - | 17694912 B | 

</details>


### Conclusions
* All measures show significantly better performance and memory allocation.
* Memory usage for the metrics without labels optimized by skipping initialization of labeled items collections.
* Tuple API allow to reduce allocation by avoiding usage of params (which is allocating array internally for each call).
* Shared array usage for params could improve memory usage.
