using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.OnionFunc.Tests {

    [TestClass]
    public class Usage {

        [TestMethod]
        public void Usage_Readme() {
            Func<string, string> handler = context => context + "|";
            Assert.AreEqual(">|", handler(">"));

            Func<string, string> pipeline = handler
                .Wrap((context, next) => next(context + "b") + "B")
                .Wrap((context, next) => next(context + "a") + "A")
                ;
            Assert.AreEqual(">ab|BA", pipeline(">"));
        }

        [TestMethod]
        public void Usage_AsPipeline() {
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
