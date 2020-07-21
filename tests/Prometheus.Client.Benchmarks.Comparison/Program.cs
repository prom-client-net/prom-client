using System;
using System.Diagnostics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using Prometheus.Client.Benchmarks.Comparison.Counter;

namespace Prometheus.Client.Benchmarks.Comparison
{
    internal class Program
    {
        private static void Main()
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(null,
                DefaultConfig.Instance
                    .With(Job.Default.With(CsProjCoreToolchain.NetCoreApp30)));

            // var bc = new CounterGeneralUseCaseBenchmarks();
            // bc.Setup();
            // bc.Counter_WithLabelsAndSamplesTuple();
            // bc.Setup();
            //
            // bc.Counter_WithLabelsAndSamplesTuple();
            //
            // Console.WriteLine("Done");
        }
    }
}
