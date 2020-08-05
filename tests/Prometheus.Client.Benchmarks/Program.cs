using BenchmarkDotNet.Running;

namespace Prometheus.Client.Benchmarks
{
    class Program
    {
        static void Main()
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();
        }
    }
}
