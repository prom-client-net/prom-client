using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Prometheus.Client.Benchmarks.Comparison
{
    internal class Program
    {
        private static void Main()
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(null,
                DefaultConfig.Instance
                    .With(MemoryDiagnoser.Default)
                    .With(new Job
                    {
                        Run =
                        {
                            RunStrategy = RunStrategy.ColdStart,
                            IterationCount = 20,
                        }
                    })
                    .With(BenchmarkLogicalGroupRule.ByCategory)
            );
        }
    }
}
