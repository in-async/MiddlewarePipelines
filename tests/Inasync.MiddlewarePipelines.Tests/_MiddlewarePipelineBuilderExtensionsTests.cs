//using System;
//using System.Threading.Tasks;
//using Inasync;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Inasync.MiddlewarePipelines.Tests {
//    [TestClass]
//    public class MiddlewarePipelineBuilderExtensionsTests {
//        [TestMethod]
//        public void Add() {
//            var pipeline = new MiddlewarePipelineBuilder<DummyContext, Task>();

//            DummyContext actualMiddlewareContext = null;
//            MiddlewareFunc<DummyContext, Task> middlewareFunc = (context, next) => {
//                actualMiddlewareContext = context;
//                return next(context);
//            };

//            new TestCaseRunner()
//              .Run(() => MiddlewarePipelineBuilderExtensions.Add(pipeline, middlewareFunc))
//              .Verify((actual, _) => {
//                  Assert.AreEqual(pipeline, actual, "戻り値の PipelineBuilder が同一インスタンスではありません。");

//                  var task = Task.FromResult(Rand.Int());
//                  DummyContext actualHandlerContext = null;
//                  Func<DummyContext, Task> handler = ctx => {
//                      actualHandlerContext = ctx;
//                      return task;
//                  };
//                  var context = new DummyContext();
//                  var actualTask = actual.Build(handler)(context);
//                  Assert.AreEqual(task, actualTask, "実行結果が期待値と異なります。");
//                  Assert.AreEqual(context, actualMiddlewareContext, "ミドルウェアの実行時コンテキストが異なります。");
//              }, (Type)null);
//        }
//    }
//}
