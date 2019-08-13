using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Inasync.OnionFunc.Benchmark {

    internal class Program {

        private static void Main(string[] args) {
            var config = ManualConfig.Create(DefaultConfig.Instance)
                //.With(RPlotExporter.Default)
                .With(MarkdownExporter.GitHub)
                .With(MemoryDiagnoser.Default)
                //.With(StatisticColumn.Min)
                //.With(StatisticColumn.Max)
                //.With(RankColumn.Arabic)
                .With(Job.Core)
                //.With(Job.Clr)
                //.With(Job.ShortRun)
                //.With(Job.ShortRun.With(BenchmarkDotNet.Environments.Platform.X64).WithWarmupCount(1).WithIterationCount(1))
                .WithArtifactsPath(null)
                ;

            BenchmarkRunner.Run<Benchmark>(config);
        }
    }

    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class Benchmark {
        private readonly Func<object, Task> _handler = context => Task.CompletedTask;
        private readonly Func<object, Task> _pipelineByNext;
        private readonly Func<object, Task> _pipelineByContextAndNext;

        public Benchmark() {
            _pipelineByNext = _handler;
            for (var i = 0; i < Wraps; i++) {
                _pipelineByNext = _pipelineByNext.Wrap(next => context => next(context));
            }

            _pipelineByContextAndNext = _handler;
            for (var i = 0; i < Wraps; i++) {
                _pipelineByContextAndNext = _pipelineByContextAndNext.Wrap((context, next) => next(context));
            }
        }

        [Params(0, 1, 10, 100)]
        public int Wraps { get; set; }

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
}
