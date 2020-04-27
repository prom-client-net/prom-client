using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using Prometheus.Client.Benchmarks.Summary;

namespace Prometheus.Client.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(null,
            //     DefaultConfig.Instance
            //         .With(Job.Default.With(CsProjCoreToolchain.NetCoreApp30)));

            var test = new SummaryCreation();
            test.Setup();
            test.CreationWithLabels();

            for (var i = 0; i < 1000; i++)
            {
                test.Creation(i);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
