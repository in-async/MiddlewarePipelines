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
Assert.AreEqual(10_000, handler(10_000));

var middlewares = new MiddlewareFunc<decimal, decimal>[]{
    (price, next) => {
        Assert.AreEqual(10_000, price);

        var nextPrice = next(price * (1 - .30m));
        Assert.AreEqual(2_100, nextPrice);
        return nextPrice;
    },
    (price, next) => {
        Assert.AreEqual(7_000, price);

        var nextPrice = next(price * (1 - .40m));
        Assert.AreEqual(2_100, nextPrice);
        return nextPrice;
    },
    (price, next) => {
        Assert.AreEqual(4_200, price);

        var nextPrice = next(price * (1 - .50m));
        Assert.AreEqual(2_100, nextPrice);
        return nextPrice;
    },
};
Func<decimal, decimal> pipeline = OnionPipeline.Build(middlewares, handler);
Assert.AreEqual(2_100, pipeline(10_000));
```


## Licence
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
