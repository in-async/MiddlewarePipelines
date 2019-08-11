# OnionFunc
[![Build status](https://ci.appveyor.com/api/projects/status/yrwpc36qmfpf1k4n/branch/master?svg=true)](https://ci.appveyor.com/project/inasync/onionfunc/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Inasync.OnionFunc.svg)](https://www.nuget.org/packages/Inasync.OnionFunc/)

***OnionFunc*** は middleware pattern を実装する為のスーパーシンプルな .NET ライブラリです。


## Target Frameworks
- .NET Standard 2.0+
- .NET Standard 1.0+
- .NET Framework 4.5+


## Usage
```cs
Func<decimal, decimal> handler = price => price;
Assert.AreEqual(1_000, handler(1_000));

Func<decimal, decimal> pipeline = handler
    .Wrap((price, next) => next(price - 200))
    .Wrap((price, next) => next(price * (1 - 0.3m)))
    ;
Assert.AreEqual(500, pipeline(1_000));
```


## Licence
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
