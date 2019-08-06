# MiddlewarePipelines
[![Build status](https://ci.appveyor.com/api/projects/status/yrwpc36qmfpf1k4n/branch/master?svg=true)](https://ci.appveyor.com/project/inasync/middlewarepipelines/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Inasync.MiddlewarePipelines.svg)](https://www.nuget.org/packages/Inasync.MiddlewarePipelines/)

***MiddlewarePipelines*** は、特定のドメインに依存せずに middleware pattern を実装する為のシンプルな .NET ライブラリです。


## Target Frameworks
- .NET Standard 2.0+
- .NET Standard 1.0+
- .NET Framework 4.5+


## Usage
```cs
Func<decimal, Task<decimal>> handler = price => Task.FromResult(price);
Assert.AreEqual(10_000, handler(10_000).Result);

var middlewares = new MiddlewareFunc<decimal, Task<decimal>>[]{
    async (price, next) => {
        Assert.AreEqual(10_000, price);

        var nextPrice = await next(price * (1 - .30m));
        Assert.AreEqual(2_100, nextPrice);
        return nextPrice;
    },
    async (price, next) => {
        Assert.AreEqual(7_000, price);

        var nextPrice = await next(price * (1 - .40m));
        Assert.AreEqual(2_100, nextPrice);
        return nextPrice;
    },
    async (price, next) => {
        Assert.AreEqual(4_200, price);

        var nextPrice = await next(price * (1 - .50m));
        Assert.AreEqual(2_100, nextPrice);
        return nextPrice;
    },
};
Func<decimal, Task<decimal>> pipeline = MiddlewarePipeline.Build(middlewares, handler);
Assert.AreEqual(2_100, pipeline(10_000).Result);
```


## Licence
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
