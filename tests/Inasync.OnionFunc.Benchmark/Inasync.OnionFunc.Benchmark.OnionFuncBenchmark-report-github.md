``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
Frequency=3320390 Hz, Resolution=301.1694 ns, Timer=TSC
.NET Core SDK=2.2.101
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                 Method | Categories | Wraps |          Mean |      Error |     StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------- |----------- |------ |--------------:|-----------:|-----------:|------:|-------:|------:|------:|----------:|
|           **Build_ByNext** |      **Build** |     **0** |     **0.8955 ns** |  **0.0113 ns** |  **0.0100 ns** |  **1.00** |      **-** |     **-** |     **-** |         **-** |
| Build_ByContextAndNext |      Build |     0 |     0.8808 ns |  0.0096 ns |  0.0085 ns |  0.98 |      - |     - |     - |         - |
|                        |            |       |               |            |            |       |        |       |       |           |
|             Run_ByNext |        Run |     0 |     3.9162 ns |  0.0136 ns |  0.0127 ns |  1.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |     0 |     3.9596 ns |  0.0127 ns |  0.0106 ns |  1.01 |      - |     - |     - |         - |
|                        |            |       |               |            |            |       |        |       |       |           |
|           **Build_ByNext** |      **Build** |     **1** |    **17.9753 ns** |  **0.0724 ns** |  **0.0677 ns** |  **1.00** | **0.0280** |     **-** |     **-** |      **88 B** |
| Build_ByContextAndNext |      Build |     1 |    20.7621 ns |  0.0613 ns |  0.0512 ns |  1.15 | 0.0305 |     - |     - |      96 B |
|                        |            |       |               |            |            |       |        |       |       |           |
|             Run_ByNext |        Run |     1 |     4.9880 ns |  0.0190 ns |  0.0177 ns |  1.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |     1 |     6.4235 ns |  0.0212 ns |  0.0198 ns |  1.29 |      - |     - |     - |         - |
|                        |            |       |               |            |            |       |        |       |       |           |
|           **Build_ByNext** |      **Build** |    **10** |   **191.4028 ns** |  **1.2275 ns** |  **1.1482 ns** |  **1.00** | **0.2797** |     **-** |     **-** |     **880 B** |
| Build_ByContextAndNext |      Build |    10 |   223.0861 ns |  0.8902 ns |  0.7891 ns |  1.17 | 0.3049 |     - |     - |     960 B |
|                        |            |       |               |            |            |       |        |       |       |           |
|             Run_ByNext |        Run |    10 |    42.1868 ns |  0.0596 ns |  0.0465 ns |  1.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |    10 |    60.2221 ns |  0.0808 ns |  0.0756 ns |  1.43 |      - |     - |     - |         - |
|                        |            |       |               |            |            |       |        |       |       |           |
|           **Build_ByNext** |      **Build** |   **100** | **1,860.5737 ns** | **19.1863 ns** | **17.9469 ns** |  **1.00** | **2.7962** |     **-** |     **-** |    **8800 B** |
| Build_ByContextAndNext |      Build |   100 | 2,168.3152 ns | 15.3326 ns | 13.5919 ns |  1.17 | 3.0479 |     - |     - |    9600 B |
|                        |            |       |               |            |            |       |        |       |       |           |
|             Run_ByNext |        Run |   100 |   290.7669 ns |  1.3736 ns |  1.2849 ns |  1.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |        Run |   100 |   480.1908 ns |  2.1884 ns |  2.0470 ns |  1.65 |      - |     - |     - |         - |
