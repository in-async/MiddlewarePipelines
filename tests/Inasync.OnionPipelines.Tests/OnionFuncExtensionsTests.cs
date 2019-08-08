using System;
using System.Collections.Generic;
using System.Linq;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.OnionPipelines.Tests {

    [TestClass]
    public class OnionFuncExtensionsTests {

        [TestMethod]
        public void Wrap_ByMiddlewareNext() {
            {
                Action TestCase(int testNumber, Func<DummyContext, DummyResult> handler, Func<Func<DummyContext, DummyResult>, Func<DummyContext, DummyResult>> middleware, Type expectedExceptionType = null) => () => {
                    new TestCaseRunner()
                      .Run(() => OnionFuncExtensions.Wrap(handler, middleware))
                      .Verify((actual, desc) => { }, expectedExceptionType);
                };

                new[]{
                    TestCase( 0, handler:null        , middleware:_ => default, typeof(ArgumentNullException)),
                    TestCase( 1, handler:_ => default, middleware:null        , typeof(ArgumentNullException)),
                    TestCase( 2, handler:_ => default, middleware:_ => default),
                }.Run();
            }
            {
                Func<DummyContext, DummyResult> handler = _ => new DummyResult();

                Func<DummyContext, DummyResult> actualNext = null;
                Func<Func<DummyContext, DummyResult>, Func<DummyContext, DummyResult>> middleware = next => {
                    actualNext = next;
                    return next;
                };

                new TestCaseRunner()
                  .Run(() => OnionFuncExtensions.Wrap(handler, middleware))
                  .Verify((actual, desc) => {
                      Assert.AreEqual(actual, handler, desc);
                      Assert.AreEqual(actualNext, handler, desc);
                  }, (Type)null);
            }
        }

        [TestMethod]
        public void Wrap_ByMiddlewareFunc() {
            var middlewareResult = new DummyResult();

            Action TestCase(int testNumber, Func<DummyContext, DummyResult> _handler, SpyMiddleware _middleware, Type expectedExceptionType = null) => () => {
                new TestCaseRunner()
                    .Run(() => OnionFuncExtensions.Wrap(_handler, _middleware?.Delegate))
                    .Verify((actual, desc) => {
                        var context = new DummyContext();
                        var actualResult = actual(context);

                        Assert.AreEqual(middlewareResult, actualResult, desc);
                        Assert.AreEqual((context, _handler), _middleware.ActualParams, desc);
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
        public void Wrap_ByMiddlewareInterface() {
            var middlewareResult = new DummyResult();

            Action TestCase(int testNumber, Func<DummyContext, DummyResult> _handler, SpyMiddleware _middleware, Type expectedExceptionType = null) => () => {
                new TestCaseRunner()
                    .Run(() => OnionFuncExtensions.Wrap(_handler, _middleware))
                    .Verify((actual, desc) => {
                        var context = new DummyContext();
                        var actualResult = actual(context);

                        Assert.AreEqual(middlewareResult, actualResult, desc);
                        Assert.AreEqual((context, _handler), _middleware.ActualParams, desc);
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
        public void Wraps_and_Invoke() {
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
                        .Wrap(factory1.Create)
                        .Wrap(factory2.Create)
                        .Wrap(factory3.Create)
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
                        .Wrap(factory1.Create)
                        .Wrap(factory2.Create)
                        .Wrap(factory3.Create)
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
            private readonly DummyResult _result;

            public SpyMiddleware(DummyResult result) => _result = result;

            public MiddlewareFunc<DummyContext, DummyResult> Delegate => Invoke;
            public (DummyContext context, Func<DummyContext, DummyResult> next) ActualParams { get; private set; }

            public DummyResult Invoke(DummyContext context, Func<DummyContext, DummyResult> next) {
                ActualParams = (context, next);
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
