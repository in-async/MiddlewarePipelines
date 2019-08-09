using System;
using System.Collections.Generic;
using System.Linq;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.OnionFunc.Tests {

    [TestClass]
    public class OnionPipelineTests {

        [TestMethod]
        public void Build_HandlerIsNull() {
            TestRun("by interfaces", () => OnionPipeline.Build(new SpyMiddleware[0], null));
            TestRun("by delegates", () => OnionPipeline.Build(new MiddlewareFunc<DummyContext, DummyResult>[0], null));

            void TestRun(string desc, Func<Func<DummyContext, DummyResult>> targetCode) => new TestCaseRunner()
                .Run(targetCode)
                .Verify((actual, _) => { }, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void Build_MiddlewaresIsNull() {
            TestRun("by interfaces", () => OnionPipeline.Build((SpyMiddleware[])null, _ => new DummyResult()));
            TestRun("by delegates", () => OnionPipeline.Build((MiddlewareFunc<DummyContext, DummyResult>[])null, _ => new DummyResult()));

            void TestRun(string desc, Func<Func<DummyContext, DummyResult>> targetCode) => new TestCaseRunner()
                .Run(targetCode)
                .Verify((actual, _) => { }, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void Build_MiddlewaresIsEmpty() {
            Func<DummyContext, DummyResult> nullHandler = _ => new DummyResult();

            TestRun("by interfaces", () => OnionPipeline.Build(new SpyMiddleware[0], nullHandler));
            TestRun("by delegates", () => OnionPipeline.Build(new MiddlewareFunc<DummyContext, DummyResult>[0], nullHandler));

            void TestRun(string desc, Func<Func<DummyContext, DummyResult>> targetCode) => new TestCaseRunner()
                .Run(targetCode)
                .Verify((actual, _) => {
                    Assert.AreEqual(nullHandler, actual);
                }, (Type)null);
        }

        [TestMethod]
        public void Build() {
            Action TestCase(int testNumber, DummyResult[] middlewareResults, DummyResult handlerResult, (DummyResult result, Func<SpyMiddleware[], SpyHandler, SpyComponent[]> components) expected) => () => {
                TestRun($"No.{testNumber} by interfaces", (middlewares, handler) => OnionPipeline.Build(middlewares, handler.Invoke));
                TestRun($"No.{testNumber} by delegates", (middlewares, handler) => OnionPipeline.Build(middlewares.Select(m => m.Delegate), handler.Invoke));

                void TestRun(string desc, Func<SpyMiddleware[], SpyHandler, Func<DummyContext, DummyResult>> targetCode) {
                    var invokedComponents = new List<SpyComponent>();
                    var middlewares = middlewareResults.Select(result => new SpyMiddleware(invokedComponents, result)).ToArray();
                    var handler = new SpyHandler(invokedComponents, handlerResult);
                    var expectedComponents = expected.components(middlewares, handler);

                    new TestCaseRunner()
                        .Run(() => targetCode(middlewares, handler))
                        .Verify((actual, _) => {
                            var context = new DummyContext();
                            var actualResult = actual(context);

                            Assert.AreEqual(expected.result, actualResult, desc);
                            CollectionAssert.AreEqual(expectedComponents, invokedComponents, desc);
                            Assert.IsTrue(invokedComponents.All(x => x.ActualContext == context), desc);
                        }, (Type)null);
                }
            };

            var hResult = new DummyResult();
            var m0Result = new DummyResult();
            new[] {
                TestCase( 0, middlewareResults:new DummyResult[]{ null    , null }, handlerResult:hResult, expected:(hResult , components:(ms, h) => new SpyComponent[]{ ms[0], ms[1], h }) ),
                TestCase( 1, middlewareResults:new DummyResult[]{ m0Result, null }, handlerResult:hResult, expected:(m0Result, components:(ms, h) => new SpyComponent[]{ ms[0]           }) ),
            }.Run();
        }

        #region Helpers

        private class SpyMiddleware : SpyComponent, IMiddleware<DummyContext, DummyResult> {

            public SpyMiddleware(List<SpyComponent> invokedComponents, DummyResult result = null) : base(invokedComponents, result) {
            }

            public MiddlewareFunc<DummyContext, DummyResult> Delegate => Invoke;

            public DummyResult Invoke(DummyContext context, Func<DummyContext, DummyResult> next) => Invoke(context) ?? next(context);
        }

        #endregion Helpers
    }
}
