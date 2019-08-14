using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Inasync.OnionFunc.Benchmark {

    internal class Program {

        private static void Main(string[] args) => BenchmarkRunner.Run<OnionFuncBenchmark>();
    }

    internal class BenchmarkConfig : ManualConfig {

        public BenchmarkConfig() {
            // Jobs
            Add(Job.Core);
            //Add(Job.Clr);
            //Add(Job.ShortRun.WithWarmupCount(1).WithIterationCount(1).With(Runtime.Core).With(Jit.RyuJit).With(Platform.X64));

            // Columns
            Add(MemoryDiagnoser.Default);
            //Add(StatisticColumn.Min);
            //Add(StatisticColumn.Max);
            //Add(RankColumn.Arabic);

            // Exporters
            Add(MarkdownExporter.GitHub);
            Add(CsvExporter.Default);
            //Add(RPlotExporter.Default);
        }
    }

    [Config(typeof(BenchmarkConfig))]
    [CategoriesColumn, GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class OnionFuncBenchmark {
        private readonly Func<object, Task> _handler = context => Task.CompletedTask;
        private Func<object, Task> _pipelineByNext;
        private Func<object, Task> _pipelineByContextAndNext;

        [Params(0, 1, 10, 100)]
        public int Wraps { get; set; }

        [GlobalSetup]
        public void Setup() {
            _pipelineByNext = _handler;
            for (var i = 0; i < Wraps; i++) {
                _pipelineByNext = _pipelineByNext.Wrap(next => context => next(context));
            }

            _pipelineByContextAndNext = _handler;
            for (var i = 0; i < Wraps; i++) {
                _pipelineByContextAndNext = _pipelineByContextAndNext.Wrap((context, next) => next(context));
            }
        }

        [BenchmarkCategory("Build"), Benchmark(Baseline = true)]
        public Func<object, Task> Build_ByNext() {
            Func<object, Task> pipeline = _handler;

            for (var i = 0; i < Wraps; i++) {
                pipeline = pipeline.Wrap(next => context => next(context));
            }

            return pipeline;
        }

        [BenchmarkCategory("Build"), Benchmark]
        public Func<object, Task> Build_ByContextAndNext() {
            Func<object, Task> pipeline = _handler;

            for (var i = 0; i < Wraps; i++) {
                pipeline = pipeline.Wrap((context, next) => next(context));
            }

            return pipeline;
        }

        [BenchmarkCategory("Run"), Benchmark(Baseline = true)]
        public Task Run_ByNext() => _pipelineByNext(null);

        [BenchmarkCategory("Run"), Benchmark]
        public Task Run_ByContextAndNext() => _pipelineByContextAndNext(null);
    }

    internal static class OnionFuncExtensions {

        public static Func<T, TResult> Wrap<T, TResult>(this Func<T, TResult> onionFunc, Func<T, Func<T, TResult>, TResult> middleware) {
            return context => middleware(context, onionFunc);
        }
    }
}
