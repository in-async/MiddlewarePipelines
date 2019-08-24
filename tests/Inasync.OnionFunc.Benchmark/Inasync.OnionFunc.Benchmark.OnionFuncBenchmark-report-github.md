``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
Frequency=3320400 Hz, Resolution=301.1685 ns, Timer=TSC
.NET Core SDK=2.2.101
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                 Method | Categories | Layers |          Mean |      Error |     StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------- |----------- |------- |--------------:|-----------:|-----------:|------:|--------:|-------:|------:|------:|----------:|
|           **Build_ByNext** |      **Build** |      **0** |     **0.9341 ns** |  **0.0214 ns** |  **0.0200 ns** |  **1.00** |    **0.00** |      **-** |     **-** |     **-** |         **-** |
| Build_ByContextAndNext |      Build |      0 |     0.8851 ns |  0.0175 ns |  0.0155 ns |  0.95 |    0.02 |      - |     - |     - |         - |
|                        |            |        |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |        Run |      0 |     3.9397 ns |  0.0141 ns |  0.0132 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |      0 |     3.9371 ns |  0.0136 ns |  0.0127 ns |  1.00 |    0.01 |      - |     - |     - |         - |
|                        |            |        |               |            |            |       |         |        |       |       |           |
|           **Build_ByNext** |      **Build** |      **1** |    **18.4800 ns** |  **0.0723 ns** |  **0.0641 ns** |  **1.00** |    **0.00** | **0.0280** |     **-** |     **-** |      **88 B** |
| Build_ByContextAndNext |      Build |      1 |    20.8399 ns |  0.0998 ns |  0.0934 ns |  1.13 |    0.01 | 0.0305 |     - |     - |      96 B |
|                        |            |        |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |        Run |      1 |     5.0323 ns |  0.0258 ns |  0.0241 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |      1 |     6.4408 ns |  0.0172 ns |  0.0160 ns |  1.28 |    0.01 |      - |     - |     - |         - |
|                        |            |        |               |            |            |       |         |        |       |       |           |
|           **Build_ByNext** |      **Build** |     **10** |   **190.0551 ns** |  **2.0328 ns** |  **1.9014 ns** |  **1.00** |    **0.00** | **0.2797** |     **-** |     **-** |     **880 B** |
| Build_ByContextAndNext |      Build |     10 |   218.1109 ns |  2.8733 ns |  2.6877 ns |  1.15 |    0.02 | 0.3049 |     - |     - |     960 B |
|                        |            |        |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |        Run |     10 |    42.3780 ns |  0.0652 ns |  0.0544 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |     10 |    60.5150 ns |  0.1763 ns |  0.1649 ns |  1.43 |    0.00 |      - |     - |     - |         - |
|                        |            |        |               |            |            |       |         |        |       |       |           |
|           **Build_ByNext** |      **Build** |    **100** | **1,854.2049 ns** |  **7.4642 ns** |  **6.9820 ns** |  **1.00** |    **0.00** | **2.7962** |     **-** |     **-** |    **8800 B** |
| Build_ByContextAndNext |      Build |    100 | 2,160.1044 ns | 17.5995 ns | 15.6015 ns |  1.16 |    0.01 | 3.0479 |     - |     - |    9600 B |
|                        |            |        |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |        Run |    100 |   290.4317 ns |  0.9990 ns |  0.9345 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |    100 |   484.5375 ns |  1.1508 ns |  0.9610 ns |  1.67 |    0.01 |      - |     - |     - |         - |
