using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Inasync.MiddlewarePipelines {

    /// <summary>
    /// ミドルウェア パイプラインに関するヘルパー クラス。
    /// </summary>
    public static class MiddlewarePipeline {

        /// <summary>
        /// ハンドラー コンポーネントをラッピングするミドルウェア パイプラインを構築します。
        /// </summary>
        /// <typeparam name="TContext">パイプラインの実行時コンテキストの型。非 <c>null</c>。</typeparam>
        /// <typeparam name="TTask">パイプラインの実行により生み出されたタスクの型。非 <c>null</c>。</typeparam>
        /// <param name="middlewares"><see cref="IMiddleware{TContext, TTask}"/> のコレクション。要素は常に非 <c>null</c>。</param>
        /// <param name="handler">パイプラインの終端に配置されるハンドラー デリゲート。</param>
        /// <returns>
        /// パイプラインのエントリーポイントとなるデリゲート。常に非 <c>null</c>。
        /// <paramref name="middlewares"/> が空の場合は、<paramref name="handler"/> をそのまま返します。
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="middlewares"/> or <paramref name="handler"/> is <c>null</c>.</exception>
        public static Func<TContext, TTask> Build<TContext, TTask>(IEnumerable<IMiddleware<TContext, TTask>> middlewares, Func<TContext, TTask> handler)
            where TTask : Task {
            if (middlewares == null) { throw new ArgumentNullException(nameof(middlewares)); }
            if (handler == null) { throw new ArgumentNullException(nameof(handler)); }

            return Build(middlewares.Select(middleware => {
                Debug.Assert(middleware != null);

                return new MiddlewareFunc<TContext, TTask>(middleware.InvokeAsync);
            }), handler);
        }

        /// <summary>
        /// ハンドラー コンポーネントをラッピングするミドルウェア パイプラインを構築します。
        /// </summary>
        /// <typeparam name="TContext">パイプラインの実行時コンテキストの型。非 <c>null</c>。</typeparam>
        /// <typeparam name="TTask">パイプラインの実行により生み出されたタスクの型。非 <c>null</c>。</typeparam>
        /// <param name="middlewares"><see cref="MiddlewareFunc{TContext, TTask}"/> のコレクション。要素は常に非 <c>null</c>。</param>
        /// <param name="handler">パイプラインの終端に配置されるハンドラー デリゲート。</param>
        /// <returns>
        /// パイプラインのエントリーポイントとなるデリゲート。常に非 <c>null</c>。
        /// <paramref name="middlewares"/> が空の場合は、<paramref name="handler"/> をそのまま返します。
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="middlewares"/> or <paramref name="handler"/> is <c>null</c>.</exception>
        public static Func<TContext, TTask> Build<TContext, TTask>(IEnumerable<MiddlewareFunc<TContext, TTask>> middlewares, Func<TContext, TTask> handler)
            where TTask : Task {
            if (middlewares == null) { throw new ArgumentNullException(nameof(middlewares)); }
            if (handler == null) { throw new ArgumentNullException(nameof(handler)); }

            var pipeline = new MiddlewarePipelineBuilder<TContext, TTask>();
            foreach (var middleware in middlewares) {
                Debug.Assert(middleware != null);

                pipeline.Add(next => {
                    Debug.Assert(next != null);

                    return context => {
                        Debug.Assert(context != null);

                        return middleware(context, next);
                    };
                });
            }
            return pipeline.Build(handler);
        }
    }
}
