using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.OnionPipelines.Tests {

    [TestClass]
    public class UsageTest {

        [TestMethod]
        public void Usage_OnionPipeline() {
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
        }

        [TestMethod]
        public void Usage_OnionFunc() {
            Func<decimal, decimal> handler = price => price;
            Assert.AreEqual(10_000, handler(10_000));

            Func<decimal, decimal> pipeline = handler
                .Wrap((price, next) => {
                    Assert.AreEqual(4_200, price);

                    var nextPrice = next(price * (1 - .50m));
                    Assert.AreEqual(2_100, nextPrice);
                    return nextPrice;
                })
                .Wrap((price, next) => {
                    Assert.AreEqual(7_000, price);

                    var nextPrice = next(price * (1 - .40m));
                    Assert.AreEqual(2_100, nextPrice);
                    return nextPrice;
                })
                .Wrap((price, next) => {
                    Assert.AreEqual(10_000, price);

                    var nextPrice = next(price * (1 - .30m));
                    Assert.AreEqual(2_100, nextPrice);
                    return nextPrice;
                })
                ;
            Assert.AreEqual(2_100, pipeline(10_000));
        }
    }
}
