using BenchmarkDotNet.Running;

namespace Helis.Files.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SorterBenchmarks>();
        }
    }
}
