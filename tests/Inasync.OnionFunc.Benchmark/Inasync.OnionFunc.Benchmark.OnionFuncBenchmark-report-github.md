``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
Frequency=3320400 Hz, Resolution=301.1685 ns, Timer=TSC
.NET Core SDK=2.2.101
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                 Method | Categories | Wears |          Mean |      Error |     StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------- |----------- |------ |--------------:|-----------:|-----------:|------:|--------:|-------:|------:|------:|----------:|
|           **Build_ByNext** |      **Build** |     **0** |     **0.8856 ns** |  **0.0184 ns** |  **0.0172 ns** |  **1.00** |    **0.00** |      **-** |     **-** |     **-** |         **-** |
| Build_ByContextAndNext |      Build |     0 |     0.9124 ns |  0.0148 ns |  0.0139 ns |  1.03 |    0.03 |      - |     - |     - |         - |
|                        |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |        Run |     0 |     3.9246 ns |  0.0222 ns |  0.0208 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |     0 |     3.9241 ns |  0.0255 ns |  0.0238 ns |  1.00 |    0.01 |      - |     - |     - |         - |
|                        |            |       |               |            |            |       |         |        |       |       |           |
|           **Build_ByNext** |      **Build** |     **1** |    **18.4700 ns** |  **0.2876 ns** |  **0.2691 ns** |  **1.00** |    **0.00** | **0.0280** |     **-** |     **-** |      **88 B** |
| Build_ByContextAndNext |      Build |     1 |    21.5071 ns |  0.1233 ns |  0.1153 ns |  1.16 |    0.02 | 0.0305 |     - |     - |      96 B |
|                        |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |        Run |     1 |     5.0032 ns |  0.0252 ns |  0.0235 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |     1 |     6.4555 ns |  0.0493 ns |  0.0462 ns |  1.29 |    0.01 |      - |     - |     - |         - |
|                        |            |       |               |            |            |       |         |        |       |       |           |
|           **Build_ByNext** |      **Build** |    **10** |   **196.0113 ns** |  **0.9251 ns** |  **0.8653 ns** |  **1.00** |    **0.00** | **0.2797** |     **-** |     **-** |     **880 B** |
| Build_ByContextAndNext |      Build |    10 |   226.5722 ns |  3.5745 ns |  3.3436 ns |  1.16 |    0.02 | 0.3049 |     - |     - |     960 B |
|                        |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |        Run |    10 |    42.3492 ns |  0.1884 ns |  0.1762 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |    10 |    60.4057 ns |  0.2296 ns |  0.2036 ns |  1.43 |    0.01 |      - |     - |     - |         - |
|                        |            |       |               |            |            |       |         |        |       |       |           |
|           **Build_ByNext** |      **Build** |   **100** | **1,902.0283 ns** | **10.7239 ns** | **10.0311 ns** |  **1.00** |    **0.00** | **2.7962** |     **-** |     **-** |    **8800 B** |
| Build_ByContextAndNext |      Build |   100 | 2,181.8436 ns | 33.0097 ns | 30.8773 ns |  1.15 |    0.02 | 3.0479 |     - |     - |    9600 B |
|                        |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |        Run |   100 |   289.3288 ns |  1.1650 ns |  1.0898 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |   100 |   481.4897 ns |  2.5055 ns |  2.3436 ns |  1.66 |    0.01 |      - |     - |     - |         - |
