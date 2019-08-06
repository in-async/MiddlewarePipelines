using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Inasync.MiddlewarePipelines {

    /// <summary>
    /// ミドルウェア パイプラインを構築するクラス。
    /// ミドルウェア コンポーネントはパイプラインに追加された順番に動作します。
    /// </summary>
    /// <typeparam name="TContext">パイプラインの実行時コンテキストの型。非 <c>null</c>。</typeparam>
    /// <typeparam name="TTask">パイプラインの実行により生み出されたタスクの型。非 <c>null</c>。</typeparam>
    internal sealed class MiddlewarePipelineBuilder<TContext, TTask>
        where TTask : Task {
        private readonly IList<Func<Func<TContext, TTask>, Func<TContext, TTask>>> _middlewares = new List<Func<Func<TContext, TTask>, Func<TContext, TTask>>>();

        /// <summary>
        /// ミドルウェア コンポーネントをパイプラインに追加します。
        /// パイプライン実行時、ミドルウェアは追加された順番に動作します。
        /// </summary>
        /// <param name="middleware">パイプラインの各層を構成するコンポーネント デリゲート。常に非 <c>null</c>。デリゲートのパラメーター及び戻り値も常に非 <c>null</c>。</param>
        /// <returns>自身を表す <see cref="MiddlewarePipelineBuilder{TFunc, TContext}"/>。常に非 <c>null</c>。</returns>
        public MiddlewarePipelineBuilder<TContext, TTask> Add(Func<Func<TContext, TTask>, Func<TContext, TTask>> middleware) {
            Debug.Assert(middleware != null);

            _middlewares.Add(middleware);
            return this;
        }

        /// <summary>
        /// ミドルウェア パイプラインを構築します。
        /// </summary>
        /// <param name="handler">パイプラインの終端に配置されるコンポーネント デリゲート。常に非 <c>null</c>。</param>
        /// <returns>パイプラインのエントリーポイントとなるデリゲート。常に非 <c>null</c>。</returns>
        public Func<TContext, TTask> Build(Func<TContext, TTask> handler) {
            Debug.Assert(handler != null);
            var next = handler;

            foreach (var middleware in _middlewares.Reverse()) {
                Debug.Assert(middleware != null);

                next = middleware(next);
                Debug.Assert(next != null);
            }

            return next;
        }
    }
}
