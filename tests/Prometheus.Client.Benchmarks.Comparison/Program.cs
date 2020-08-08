using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Prometheus.Client.Benchmarks.Comparison
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args,
                DefaultConfig.Instance
                    .With(MemoryDiagnoser.Default)
                    .With(new Job
                    {
                        Run =
                        {
                            RunStrategy = RunStrategy.Monitoring,
                            IterationCount = 20,
                            WarmupCount = 2,
                        }
                    })
                    .With(BenchmarkLogicalGroupRule.ByCategory)
            );
        }
    }
}
