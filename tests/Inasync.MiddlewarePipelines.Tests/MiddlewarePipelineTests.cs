using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.MiddlewarePipelines.Tests {

    [TestClass]
    public class MiddlewarePipelineTests {

        [TestMethod]
        public void Build_HandlerIsNull() {
            TestRun("by interfaces", () => MiddlewarePipeline.Build(new SpyMiddleware[0], null));
            TestRun("by delegates", () => MiddlewarePipeline.Build(new MiddlewareFunc<DummyContext, Task>[0], null));

            void TestRun(string desc, Func<Func<DummyContext, Task>> targetCode) => new TestCaseRunner()
                .Run(targetCode)
                .Verify((actual, _) => { }, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void Build_MiddlewaresIsNull() {
            TestRun("by interfaces", () => MiddlewarePipeline.Build((SpyMiddleware[])null, _ => Task.CompletedTask));
            TestRun("by delegates", () => MiddlewarePipeline.Build((MiddlewareFunc<DummyContext, Task>[])null, _ => Task.CompletedTask));

            void TestRun(string desc, Func<Func<DummyContext, Task>> targetCode) => new TestCaseRunner()
                .Run(targetCode)
                .Verify((actual, _) => { }, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void Build_MiddlewaresIsEmpty() {
            Func<DummyContext, Task> nullHandler = _ => Task.CompletedTask;

            TestRun("by interfaces", () => MiddlewarePipeline.Build(new SpyMiddleware[0], nullHandler));
            TestRun("by delegates", () => MiddlewarePipeline.Build(new MiddlewareFunc<DummyContext, Task>[0], nullHandler));

            void TestRun(string desc, Func<Func<DummyContext, Task>> targetCode) => new TestCaseRunner()
                .Run(targetCode)
                .Verify((actual, _) => {
                    Assert.AreEqual(nullHandler, actual);
                }, (Type)null);
        }

        [TestMethod]
        public void Build() {
            Action TestCase(int testNumber, Task[] middlewareTasks, Task handlerTask, (Task task, Func<SpyMiddleware[], SpyHandler, SpyComponent[]> components) expected) => () => {
                TestRun($"No.{testNumber} by interfaces", (middlewares, handler) => MiddlewarePipeline.Build(middlewares, handler.InvokeAsync));
                TestRun($"No.{testNumber} by delegates", (middlewares, handler) => MiddlewarePipeline.Build(middlewares.Select(m => m.Delegate), handler.InvokeAsync));

                void TestRun(string desc, Func<SpyMiddleware[], SpyHandler, Func<DummyContext, Task>> targetCode) {
                    var invokedComponents = new List<SpyComponent>();
                    var middlewares = middlewareTasks.Select(result => new SpyMiddleware(invokedComponents, result)).ToArray();
                    var handler = new SpyHandler(invokedComponents, handlerTask);
                    var expectedComponents = expected.components(middlewares, handler);

                    new TestCaseRunner()
                        .Run(() => targetCode(middlewares, handler))
                        .Verify((actual, _) => {
                            var context = new DummyContext();
                            var actualTask = actual(context);

                            Assert.AreEqual(expected.task, actualTask, desc);
                            CollectionAssert.AreEqual(expectedComponents, invokedComponents, desc);
                            Assert.IsTrue(invokedComponents.All(x => x.ActualContext == context), desc);
                        }, (Type)null);
                }
            };

            var hTask = Task.FromResult(Rand.Int());
            var m0Task = Task.FromResult(Rand.Int());
            new[] {
                TestCase( 0, middlewareTasks:new Task[]{ null  , null }, handlerTask:hTask, expected:(hTask , components:(ms, h) => new SpyComponent[]{ ms[0], ms[1], h }) ),
                TestCase( 1, middlewareTasks:new Task[]{ m0Task, null }, handlerTask:hTask, expected:(m0Task, components:(ms, h) => new SpyComponent[]{ ms[0]           }) ),
            }.Run();
        }

        #region Helpers

        private class SpyMiddleware : SpyComponent, IMiddleware<DummyContext, Task> {

            public SpyMiddleware(List<SpyComponent> invokedComponents, Task result = null) : base(invokedComponents, result) {
            }

            public MiddlewareFunc<DummyContext, Task> Delegate => InvokeAsync;

            public Task InvokeAsync(DummyContext context, Func<DummyContext, Task> next) => InvokeAsync(context) ?? next(context);
        }

        #endregion Helpers
    }
}
