# OnionFunc
[![Build status](https://ci.appveyor.com/api/projects/status/yrwpc36qmfpf1k4n/branch/master?svg=true)](https://ci.appveyor.com/project/inasync/onionfunc/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Inasync.OnionFunc.svg)](https://www.nuget.org/packages/Inasync.OnionFunc/)

***OnionFunc*** は、関数に別の処理をラッピングするヘルパーを提供するだけの、スーパーシンプルな .NET ライブラリです。

これによって middleware pattern によるパイプラインの構築を簡単にします。


## Target Frameworks
- .NET Standard 2.0+
- .NET Standard 1.0+
- .NET Framework 4.5+


## Usage
```cs
Func<string, string> handler = value => value + "|";
Assert.AreEqual(">|", handler(">"));

Func<string, string> pipeline = handler
    .Wrap((value, next) => next(value + "b") + "B")
    .Wrap((value, next) => next(value + "a") + "A")
    ;
Assert.AreEqual(">ab|BA", pipeline(">"));
```


## Licence
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
