using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.MiddlewarePipelines.Tests {

    [TestClass]
    public class UsageTest {

        [TestMethod]
        public void Usage() {
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
        }
    }
}
