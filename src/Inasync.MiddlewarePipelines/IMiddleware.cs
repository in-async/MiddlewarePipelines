using System;
using System.Threading.Tasks;

namespace Inasync.MiddlewarePipelines {

    /// <summary>
    /// ミドルウェア パイプラインを構成するコンポーネント インターフェース。
    /// </summary>
    /// <typeparam name="TContext">パイプラインの実行時コンテキストの型。非 <c>null</c>。</typeparam>
    /// <typeparam name="TTask">パイプラインの実行により生み出されたタスクの型。非 <c>null</c>。</typeparam>
    public interface IMiddleware<TContext, TTask>
        where TTask : Task {

        /// <summary>
        /// ミドルウェアで定義されている処理を実行します。
        /// </summary>
        /// <param name="context">パイプラインの実行時コンテキスト。常に非 <c>null</c>。</param>
        /// <param name="next">パイプラインの後続のコンポーネントを表すデリゲート。常に非 <c>null</c>。呼ばない事により残りのコンポーネントをショートサーキットできます。</param>
        /// <returns>このミドルウェア コンポーネント以降の処理を表すタスク。常に非 <c>null</c>。</returns>
        TTask InvokeAsync(TContext context, Func<TContext, TTask> next);
    }

    /// <summary>
    /// ミドルウェアで定義されている処理を実行します。
    /// </summary>
    /// <typeparam name="TContext">パイプラインの実行時コンテキストの型。非 <c>null</c>。</typeparam>
    /// <typeparam name="TTask">パイプラインの実行により生み出されたタスクの型。非 <c>null</c>。</typeparam>
    /// <param name="context">パイプラインの実行時コンテキスト。常に非 <c>null</c>。</param>
    /// <param name="next">パイプラインの後続のコンポーネントを表すデリゲート。常に非 <c>null</c>。呼ばない事により残りのコンポーネントをショートサーキットできます。</param>
    /// <returns>このミドルウェア コンポーネント以降の処理を表すタスク。常に非 <c>null</c>。</returns>
    public delegate TTask MiddlewareFunc<TContext, TTask>(TContext context, Func<TContext, TTask> next);
}
