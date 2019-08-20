using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.OnionFunc.Tests {

    [TestClass]
    public class Usage {

        [TestMethod]
        public void Usage_Readme() {
            Func<int, bool> handler = num => true;
            Assert.AreEqual(true, handler(24));

            Func<int, bool> pipeline = handler
                .Wear(next => num => (num % 3 == 0) && next(num))
                .Wear(next => num => (num % 4 == 0) && next(num))
                ;
            Assert.AreEqual(true, pipeline(24));
            Assert.AreEqual(false, pipeline(30));
        }

        [TestMethod]
        public void Usage_AsPipeline() {
            Func<decimal, decimal> handler = price => price;
            Assert.AreEqual(10_000, handler(10_000));

            Func<decimal, decimal> pipeline = handler
                .Wear(next => price => {
                    Assert.AreEqual(4_200, price);

                    var nextPrice = next(price * (1 - .50m));
                    Assert.AreEqual(2_100, nextPrice);
                    return nextPrice;
                })
                .Wear(next => price => {
                    Assert.AreEqual(7_000, price);

                    var nextPrice = next(price * (1 - .40m));
                    Assert.AreEqual(2_100, nextPrice);
                    return nextPrice;
                })
                .Wear(next => price => {
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
