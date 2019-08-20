using System;
using System.Collections.Generic;
using System.Linq;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.OnionFunc.Tests {

    [TestClass]
    public class OnionFuncExtensionsTests {

        [TestMethod]
        public void Wear_ByMiddlewareFunc() {
            Func<DummyContext, DummyResult> middlewareResult = context => new DummyResult();

            Action TestCase(int testNumber, Func<DummyContext, DummyResult> _handler, SpyMiddleware _middleware, Type expectedExceptionType = null) => () => {
                new TestCaseRunner()
                    .Run(() => OnionFuncExtensions.Wear(_handler, _middleware?.Delegate))
                    .Verify((actual, desc) => {
                        Assert.AreEqual(middlewareResult, actual, desc);
                        Assert.AreEqual(_handler, _middleware.ActualNext, desc);
                    }, expectedExceptionType);
            };

            Func<DummyContext, DummyResult> handler = _ => new DummyResult();
            var middleware = new SpyMiddleware(middlewareResult);
            new[]{
                TestCase( 0, null   , middleware, typeof(ArgumentNullException)),
                TestCase( 1, handler, null      , typeof(ArgumentNullException)),
                TestCase( 2, handler, middleware),
            }.Run();
        }

        [TestMethod]
        public void Wear_ByMiddlewareInterface() {
            Func<DummyContext, DummyResult> middlewareResult = context => new DummyResult();

            Action TestCase(int testNumber, Func<DummyContext, DummyResult> _handler, SpyMiddleware _middleware, Type expectedExceptionType = null) => () => {
                new TestCaseRunner()
                    .Run(() => OnionFuncExtensions.Wear(_handler, _middleware))
                    .Verify((actual, desc) => {
                        Assert.AreEqual(middlewareResult, actual, desc);
                        Assert.AreEqual(_handler, _middleware.ActualNext, desc);
                    }, expectedExceptionType);
            };

            Func<DummyContext, DummyResult> handler = _ => new DummyResult();
            var middleware = new SpyMiddleware(middlewareResult);
            new[]{
                TestCase( 0, null   , middleware, typeof(ArgumentNullException)),
                TestCase( 1, handler, null      , typeof(ArgumentNullException)),
                TestCase( 2, handler, middleware),
            }.Run();
        }

        [TestMethod]
        public void Wears_and_Invoke() {
            {
                var invokedFactories = new List<SpyComponent>();
                var invokedComponents = new List<SpyComponent>();
                var factory1 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var factory2 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var factory3 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var handlerResult = new DummyResult();
                var handler = new SpyHandler(invokedComponents, handlerResult);

                var expectedResult = handlerResult;
                var expectedFactories = new SpyComponent[] { factory1, factory2, factory3 };
                var expectedComponents = new SpyComponent[] { factory3, factory2, factory1, handler };

                new TestCaseRunner("middlewares 及び handler が順番通りに呼ばれる")
                    .Run(() => new Func<DummyContext, DummyResult>(handler.Invoke)
                        .Wear(factory1.Create)
                        .Wear(factory2.Create)
                        .Wear(factory3.Create)
                    )
                    .Verify((actual, desc) => {
                        var context = new DummyContext();
                        var actualResult = actual(context);

                        Assert.AreEqual(expectedResult, actualResult, desc);
                        CollectionAssert.AreEqual(expectedFactories, invokedFactories, $"{desc}: ファクトリーの呼び出し順序が一致しません。");
                        CollectionAssert.AreEqual(expectedComponents, invokedComponents, $"{desc}: コンポーネントの実行順序が一致しません。");
                        Assert.IsTrue(invokedComponents.All(x => x.ActualContext == context), desc);
                    }, (Type)null);
            }

            {
                var invokedFactories = new List<SpyComponent>();
                var invokedComponents = new List<SpyComponent>();
                var result2 = new DummyResult();
                var factory1 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var factory2 = new SpyMiddlewareFactory(invokedFactories, invokedComponents, result2);
                var factory3 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var handler = new SpyHandler(invokedComponents, new DummyResult());

                var expectedResult = result2;
                var expectedFactories = new SpyComponent[] { factory1, factory2, factory3 };
                var expectedComponents = new SpyComponent[] { factory3, factory2 };

                new TestCaseRunner("middleware2 でショートサーキット")
                    .Run(() => new Func<DummyContext, DummyResult>(handler.Invoke)
                        .Wear(factory1.Create)
                        .Wear(factory2.Create)
                        .Wear(factory3.Create)
                    )
                    .Verify((actual, desc) => {
                        var context = new DummyContext();
                        var actualResult = actual(context);

                        Assert.AreEqual(expectedResult, actualResult, desc);
                        CollectionAssert.AreEqual(expectedFactories, invokedFactories, $"{desc}: ファクトリーの呼び出し順序が一致しません。");
                        CollectionAssert.AreEqual(expectedComponents, invokedComponents, $"{desc}: コンポーネントの実行順序が一致しません。");
                        Assert.IsTrue(invokedComponents.All(x => x.ActualContext == context), desc);
                    }, (Type)null);
            }
        }

        #region Helpers

        private sealed class SpyMiddleware : IMiddleware<DummyContext, DummyResult> {
            private readonly Func<DummyContext, DummyResult> _result;

            public SpyMiddleware(Func<DummyContext, DummyResult> result) => _result = result;

            public MiddlewareFunc<DummyContext, DummyResult> Delegate => Invoke;
            public Func<DummyContext, DummyResult> ActualNext { get; private set; }

            public Func<DummyContext, DummyResult> Invoke(Func<DummyContext, DummyResult> next) {
                ActualNext = next;
                return _result;
            }
        }

        private class SpyMiddlewareFactory : SpyComponent {
            private readonly List<SpyComponent> _invokedFactories;

            public SpyMiddlewareFactory(List<SpyComponent> invokedFactories, List<SpyComponent> invokedComponents, DummyResult result = null) : base(invokedComponents, result) {
                _invokedFactories = invokedFactories;
            }

            public Func<DummyContext, DummyResult> Create(Func<DummyContext, DummyResult> next) {
                _invokedFactories.Add(this);
                return context => Invoke(context) ?? next(context);
            }
        }

        #endregion Helpers
    }
}
