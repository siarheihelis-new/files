using BenchmarkDotNet.Attributes;
using Helis.Files.Generator;
using Helis.Files.Sorter;
using Helis.Files.Sorter.Impl;

namespace Helis.Files.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(
        launchCount: 1,
        warmupCount: 1, 
        iterationCount: 3)]
    public class SorterBenchmarks
    {
        private string _tempDirectory = String.Empty;
        private SorterOptions _options = null!;
        [Params(100, 250, 500)]
        public int SizeInMb { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), $"files_benchmark_{Guid.NewGuid()}");
            Directory.CreateDirectory(_tempDirectory);
            var inputFilePath = Path.Combine(_tempDirectory, $"input{SizeInMb}.txt");
            var outputFilePath = Path.Combine(_tempDirectory, $"output{SizeInMb}.txt");
            _options = new SorterOptions(inputFilePath, outputFilePath);

            Console.WriteLine($"Generating benchmark file of size {SizeInMb} MB...");
            var generatorOptions = new GeneratorOptions(inputFilePath, SizeInMb * 1024 * 1024);
            var generator = new FileGenerator();            
            generator.GenerateFile(generatorOptions);
            Console.WriteLine("Benchmark file ready");
        }

        [Benchmark]
        public async Task SortLargeFile()
        {
            
            var sorter = new FileSorter();
            await sorter.Sort(_options);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
        }
    }
}
