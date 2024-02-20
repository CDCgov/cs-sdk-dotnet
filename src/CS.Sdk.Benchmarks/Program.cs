// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;

namespace CS.Sdk.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // BenchmarkRunner.Run<DebatcherBenchmarks100>();
            // BenchmarkRunner.Run<DebatcherBenchmarks1000>();
            BenchmarkRunner.Run<DebatcherBenchmarks10k>();
        }
    }
}


