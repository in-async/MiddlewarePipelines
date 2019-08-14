``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
Frequency=3320390 Hz, Resolution=301.1694 ns, Timer=TSC
.NET Core SDK=2.2.101
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3416.0
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|                 Method |  Job | Runtime | Categories | Wraps |          Mean |      Error |     StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------- |----- |-------- |----------- |------ |--------------:|-----------:|-----------:|------:|--------:|-------:|------:|------:|----------:|
|           **Build_ByNext** |  **Clr** |     **Clr** |      **Build** |     **0** |     **0.5850 ns** |  **0.0186 ns** |  **0.0174 ns** |  **1.00** |    **0.00** |      **-** |     **-** |     **-** |         **-** |
| Build_ByContextAndNext |  Clr |     Clr |      Build |     0 |     0.5775 ns |  0.0176 ns |  0.0156 ns |  0.99 |    0.05 |      - |     - |     - |         - |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |  Clr |     Clr |        Run |     0 |     7.2317 ns |  0.0380 ns |  0.0355 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |  Clr |     Clr |        Run |     0 |     7.2973 ns |  0.0635 ns |  0.0594 ns |  1.01 |    0.01 |      - |     - |     - |         - |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|           Build_ByNext | Core |    Core |      Build |     0 |     0.9115 ns |  0.0217 ns |  0.0203 ns |  1.00 |    0.00 |      - |     - |     - |         - |
| Build_ByContextAndNext | Core |    Core |      Build |     0 |     0.9108 ns |  0.0260 ns |  0.0243 ns |  1.00 |    0.03 |      - |     - |     - |         - |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext | Core |    Core |        Run |     0 |     3.9186 ns |  0.0152 ns |  0.0135 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext | Core |    Core |        Run |     0 |     3.9225 ns |  0.0169 ns |  0.0159 ns |  1.00 |    0.01 |      - |     - |     - |         - |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|           **Build_ByNext** |  **Clr** |     **Clr** |      **Build** |     **1** |    **16.9615 ns** |  **0.1202 ns** |  **0.1125 ns** |  **1.00** |    **0.00** | **0.0280** |     **-** |     **-** |      **88 B** |
| Build_ByContextAndNext |  Clr |     Clr |      Build |     1 |    19.8608 ns |  0.0781 ns |  0.0693 ns |  1.17 |    0.01 | 0.0305 |     - |     - |      96 B |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |  Clr |     Clr |        Run |     1 |     8.3781 ns |  0.0171 ns |  0.0151 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |  Clr |     Clr |        Run |     1 |     9.7227 ns |  0.0202 ns |  0.0179 ns |  1.16 |    0.00 |      - |     - |     - |         - |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|           Build_ByNext | Core |    Core |      Build |     1 |    18.1717 ns |  0.0978 ns |  0.0867 ns |  1.00 |    0.00 | 0.0280 |     - |     - |      88 B |
| Build_ByContextAndNext | Core |    Core |      Build |     1 |    21.1832 ns |  0.1173 ns |  0.1040 ns |  1.17 |    0.01 | 0.0305 |     - |     - |      96 B |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext | Core |    Core |        Run |     1 |     4.9969 ns |  0.0175 ns |  0.0155 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext | Core |    Core |        Run |     1 |     6.4262 ns |  0.0213 ns |  0.0199 ns |  1.29 |    0.01 |      - |     - |     - |         - |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|           **Build_ByNext** |  **Clr** |     **Clr** |      **Build** |    **10** |   **180.6905 ns** |  **0.6564 ns** |  **0.6140 ns** |  **1.00** |    **0.00** | **0.2797** |     **-** |     **-** |     **880 B** |
| Build_ByContextAndNext |  Clr |     Clr |      Build |    10 |   210.2961 ns |  2.3702 ns |  2.2170 ns |  1.16 |    0.01 | 0.3049 |     - |     - |     960 B |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |  Clr |     Clr |        Run |    10 |    47.7211 ns |  0.1903 ns |  0.1780 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |  Clr |     Clr |        Run |    10 |    62.7102 ns |  0.4164 ns |  0.3895 ns |  1.31 |    0.01 |      - |     - |     - |         - |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|           Build_ByNext | Core |    Core |      Build |    10 |   190.4519 ns |  1.3557 ns |  1.2681 ns |  1.00 |    0.00 | 0.2797 |     - |     - |     880 B |
| Build_ByContextAndNext | Core |    Core |      Build |    10 |   223.2906 ns |  2.3889 ns |  2.2345 ns |  1.17 |    0.01 | 0.3049 |     - |     - |     960 B |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext | Core |    Core |        Run |    10 |    42.3852 ns |  0.1270 ns |  0.1188 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext | Core |    Core |        Run |    10 |    60.9239 ns |  0.1919 ns |  0.1795 ns |  1.44 |    0.01 |      - |     - |     - |         - |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|           **Build_ByNext** |  **Clr** |     **Clr** |      **Build** |   **100** | **1,756.9309 ns** |  **5.3162 ns** |  **4.9728 ns** |  **1.00** |    **0.00** | **2.7962** |     **-** |     **-** |    **8800 B** |
| Build_ByContextAndNext |  Clr |     Clr |      Build |   100 | 2,020.9489 ns | 28.2808 ns | 26.4539 ns |  1.15 |    0.02 | 3.0479 |     - |     - |    9600 B |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext |  Clr |     Clr |        Run |   100 |   295.1236 ns |  1.2047 ns |  1.1269 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext |  Clr |     Clr |        Run |   100 |   484.0248 ns |  1.1592 ns |  1.0843 ns |  1.64 |    0.01 |      - |     - |     - |         - |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|           Build_ByNext | Core |    Core |      Build |   100 | 1,854.6751 ns |  3.8223 ns |  3.5753 ns |  1.00 |    0.00 | 2.7962 |     - |     - |    8800 B |
| Build_ByContextAndNext | Core |    Core |      Build |   100 | 2,341.5158 ns | 20.1262 ns | 18.8261 ns |  1.26 |    0.01 | 3.0479 |     - |     - |    9600 B |
|                        |      |         |            |       |               |            |            |       |         |        |       |       |           |
|             Run_ByNext | Core |    Core |        Run |   100 |   291.4474 ns |  1.6901 ns |  1.4983 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|   Run_ByContextAndNext | Core |    Core |        Run |   100 |   482.5567 ns |  2.0029 ns |  1.7755 ns |  1.66 |    0.01 |      - |     - |     - |         - |
