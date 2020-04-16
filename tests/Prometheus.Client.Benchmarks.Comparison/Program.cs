using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;

namespace Prometheus.Client.Benchmarks.Comparison
{
    internal class Program
    {
        private static void Main()
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(null,
                DefaultConfig.Instance
                    .With(Job.Default.With(CsProjCoreToolchain.NetCoreApp30)));
        }
    }
}
