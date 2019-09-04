using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;

namespace Prometheus.Client.Benchmarks.Comparison
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // var b = new Counter.SampleBenchmarks();
            // b.Setup();
            // b.Counter_IncDefault();
            //
            // for(int i = 0; i < 100; i++)
            //     b.Counter_IncDefault();
            //
            // Console.ReadLine();

            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(null,
                DefaultConfig.Instance
                    .With(Job.Default.With(CsProjCoreToolchain.NetCoreApp30)));

            //BenchmarkRunner.Run<SerializationBenchmarks>();
            //BenchmarkRunner.Run<LabelBenchmarks>();
            //BenchmarkRunner.Run<HttpExporterBenchmarks>();
            //BenchmarkRunner.Run<SummaryBenchmarks>();
        }
    }
}
