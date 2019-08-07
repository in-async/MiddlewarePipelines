using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.OnionPipelines.Tests {

    [TestClass]
    public class OnionFuncExtensionsTests {

        [TestMethod]
        public void Wrap_ByMiddlewareNext() {
            Func<DummyContext, Task> handler = _ => Task.CompletedTask;

            var invokedMiddleware = false;
            Func<Func<DummyContext, Task>, Func<DummyContext, Task>> middleware = next => {
                invokedMiddleware = true;
                return next;
            };

            new TestCaseRunner()
              .Run(() => OnionFuncExtensions.Wrap(handler, middleware))
              .Verify((actual, _) => {
                  Assert.IsTrue(invokedMiddleware, "ミドルウェアが呼ばれていません。");
              }, (Type)null);
        }

        [TestMethod]
        public void Wrap_ByMiddlewareFunc() {
            var task = Task.FromResult(Rand.Int());
            DummyContext actualHandlerContext = null;
            Func<DummyContext, Task> handler = ctx => {
                actualHandlerContext = ctx;
                return task;
            };

            DummyContext actualMiddlewareContext = null;
            MiddlewareFunc<DummyContext, Task> middlewareFunc = (context, next) => {
                actualMiddlewareContext = context;
                return next(context);
            };

            new TestCaseRunner()
              .Run(() => OnionFuncExtensions.Wrap(handler, middlewareFunc))
              .Verify((actual, _) => {
                  var context = new DummyContext();
                  var actualTask = actual(context);
                  Assert.AreEqual(task, actualTask, "実行結果が期待値と異なります。");
                  Assert.AreEqual(context, actualMiddlewareContext, "ミドルウェアの実行時コンテキストが異なります。");
              }, (Type)null);
        }

        [TestMethod]
        public void Wrap_and_Invoke() {
            {
                var invokedFactories = new List<SpyComponent>();
                var invokedComponents = new List<SpyComponent>();
                var factory1 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var factory2 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var factory3 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var task = Task.FromResult(Rand.Int());
                var handler = new SpyHandler(invokedComponents, task);

                var expectedTask = task;
                var expectedFactories = new SpyComponent[] { factory3, factory2, factory1 };
                var expectedComponents = new SpyComponent[] { factory1, factory2, factory3, handler };

                new TestCaseRunner("middlewares 及び handler が順番通りに呼ばれる")
                    .Run(() => new Func<DummyContext, Task>(handler.InvokeAsync)
                        .Wrap(factory3.Create)
                        .Wrap(factory2.Create)
                        .Wrap(factory1.Create)
                    )
                    .Verify((actual, desc) => {
                        var context = new DummyContext();
                        var actualTask = actual(context);

                        Assert.AreEqual(expectedTask, actualTask, desc);
                        CollectionAssert.AreEqual(expectedFactories, invokedFactories, $"{desc}: ファクトリーの呼び出し順序が一致しません。");
                        CollectionAssert.AreEqual(expectedComponents, invokedComponents, $"{desc}: コンポーネントの実行順序が一致しません。");
                        Assert.IsTrue(invokedComponents.All(x => x.ActualContext == context), desc);
                    }, (Type)null);
            }

            {
                var invokedFactories = new List<SpyComponent>();
                var invokedComponents = new List<SpyComponent>();
                var task2 = Task.FromResult(Rand.Int());
                var factory1 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var factory2 = new SpyMiddlewareFactory(invokedFactories, invokedComponents, task2);
                var factory3 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var handler = new SpyHandler(invokedComponents, Task.FromResult(Rand.Int()));

                var expectedTask = task2;
                var expectedFactories = new SpyComponent[] { factory3, factory2, factory1 };
                var expectedComponents = new SpyComponent[] { factory1, factory2 };

                new TestCaseRunner("middleware2 でショートサーキット")
                    .Run(() => new Func<DummyContext, Task>(handler.InvokeAsync)
                        .Wrap(factory3.Create)
                        .Wrap(factory2.Create)
                        .Wrap(factory1.Create)
                    )
                    .Verify((actual, desc) => {
                        var context = new DummyContext();
                        var actualTask = actual(context);

                        Assert.AreEqual(expectedTask, actualTask, desc);
                        CollectionAssert.AreEqual(expectedFactories, invokedFactories, $"{desc}: ファクトリーの呼び出し順序が一致しません。");
                        CollectionAssert.AreEqual(expectedComponents, invokedComponents, $"{desc}: コンポーネントの実行順序が一致しません。");
                        Assert.IsTrue(invokedComponents.All(x => x.ActualContext == context), desc);
                    }, (Type)null);
            }
        }

        #region Helpers

        private class SpyMiddlewareFactory : SpyComponent {
            private readonly List<SpyComponent> _invokedFactories;

            public SpyMiddlewareFactory(List<SpyComponent> invokedFactories, List<SpyComponent> invokedComponents, Task result = null) : base(invokedComponents, result) {
                _invokedFactories = invokedFactories;
            }

            public Func<DummyContext, Task> Create(Func<DummyContext, Task> next) {
                _invokedFactories.Add(this);
                return context => InvokeAsync(context) ?? next(context);
            }
        }

        #endregion Helpers
    }
}
