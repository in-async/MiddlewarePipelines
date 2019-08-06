using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.MiddlewarePipelines.Tests {

    [TestClass]
    public class MiddlewarePipelineBuilderTests {

        [TestMethod]
        public void PipelineBuilder() {
            new TestCaseRunner()
                .Run(() => new MiddlewarePipelineBuilder<DummyContext, Task>())
                .Verify((actual, _) => { }, (Type)null);
        }

        [TestMethod]
        public void Add() {
            var pipeline = new MiddlewarePipelineBuilder<DummyContext, Task>();
            var invokedMiddleware = false;
            Func<Func<DummyContext, Task>, Func<DummyContext, Task>> middleware = next => {
                invokedMiddleware = true;
                return next;
            };

            new TestCaseRunner()
              .Run(() => pipeline.Add(middleware))
              .Verify((actual, _) => {
                  Assert.AreEqual(pipeline, actual, "戻り値の PipelineBuilder が同一インスタンスではありません。");

                  pipeline.Build(__ => Task.CompletedTask);
                  Assert.IsTrue(invokedMiddleware, "ミドルウェアが呼ばれていません。");
              }, (Type)null);
        }

        [TestMethod]
        public void Build() {
            {
                var invokedFactories = new List<SpyComponent>();
                var invokedComponents = new List<SpyComponent>();
                var factory1 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var factory2 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var factory3 = new SpyMiddlewareFactory(invokedFactories, invokedComponents);
                var pipeline = new MiddlewarePipelineBuilder<DummyContext, Task>()
                    .Add(factory1.Create)
                    .Add(factory2.Create)
                    .Add(factory3.Create)
                    ;
                var task = Task.FromResult(Rand.Int());
                var handler = new SpyHandler(invokedComponents, task);
                var expectedTask = task;
                var expectedFactories = new SpyComponent[] { factory3, factory2, factory1 };
                var expectedComponents = new SpyComponent[] { factory1, factory2, factory3, handler };

                new TestCaseRunner("middlewares 及び handler が順番通りに呼ばれる")
                    .Run(() => pipeline.Build(handler.InvokeAsync))
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
                var pipeline = new MiddlewarePipelineBuilder<DummyContext, Task>()
                    .Add(factory1.Create)
                    .Add(factory2.Create)
                    .Add(factory3.Create)
                    ;
                var handler = new SpyHandler(invokedComponents, Task.FromResult(Rand.Int()));
                var expectedTask = task2;
                var expectedFactories = new SpyComponent[] { factory3, factory2, factory1 };
                var expectedComponents = new SpyComponent[] { factory1, factory2 };

                new TestCaseRunner("middleware2 でショートサーキット")
                    .Run(() => pipeline.Build(handler.InvokeAsync))
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
