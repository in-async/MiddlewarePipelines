//using System.Diagnostics;
//using System.Threading.Tasks;

//namespace Inasync.MiddlewarePipelines {
//    /// <summary>
//    /// ミドルウェアをパイプラインに追加するヘルパー拡張メソッド クラス。
//    /// </summary>
//    internal static class MiddlewarePipelineBuilderExtensions {
//        /// <summary>
//        /// ミドルウェア コンポーネントをパイプラインに追加します。
//        /// </summary>
//        /// <typeparam name="TContext">パイプラインの実行時コンテキストの型。非 <c>null</c>。</typeparam>
//        /// <typeparam name="TTask">パイプラインの実行により生み出されたタスクの型。非 <c>null</c>。</typeparam>
//        /// <param name="pipeline">ミドルウェアの追加対象となるパイプライン。常に非 <c>null</c>。</param>
//        /// <param name="middleware">ミドルウェアを構成するデリゲート。常に非 <c>null</c>。</param>
//        /// <returns>自身を表す <see cref="MiddlewarePipelineBuilder{TFunc, TContext}"/>。常に非 <c>null</c>。</returns>
//        public static MiddlewarePipelineBuilder<TContext, TTask> Add<TContext, TTask>(
//              this MiddlewarePipelineBuilder<TContext, TTask> pipeline
//            , MiddlewareFunc<TContext, TTask> middleware
//        ) where TTask : Task {
//            Debug.Assert(pipeline != null);
//            Debug.Assert(middleware != null);

//            return pipeline.Add(next => {
//                Debug.Assert(next != null);

//                return context => {
//                    Debug.Assert(context != null);

//                    return middleware(context, next);
//                };
//            });
//        }
//    }
//}
