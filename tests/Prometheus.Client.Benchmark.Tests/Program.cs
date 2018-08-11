using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using RunMode = BenchmarkDotNet.Jobs.RunMode;

namespace Prometheus.Client.Benchmark.Tests
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var config = DefaultConfig.Instance
                .With(new Job("X64-Core-RyuJit", RunMode.Medium, EnvMode.Core)
                {
                    Accuracy = { RemoveOutliers = false },
                    Env = { Runtime = Runtime.Core, Platform = Platform.X64, Jit = Jit.RyuJit }
                }).With(MemoryDiagnoser.Default);

            BenchmarkRunner.Run<CounterBenchmark>(config);
        }
    }

    public class CounterBenchmark
    {
        private readonly Counter _counter;

        public CounterBenchmark()
        {
            _counter = Metrics.CreateCounter("my_counter_labels", "help", "label_one", "label_two", "label_three", "label_four");
        }

        [Benchmark]
        public void Counter_Labels()
        {
            _counter.Labels("label_one_val", "label_two_val", "label_three_val", "label_four_val").Inc();
        }
    }
}