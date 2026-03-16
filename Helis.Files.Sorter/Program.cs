using System.Diagnostics;
using Helis.Files.Sorter.Impl;
using Helis.Files.Generator;

namespace Helis.Files.Sorter
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var options = CommandLineParser.Parse(args);

                if (!File.Exists(options.InputFile))
                {
                    Console.Error.WriteLine($"Input file not found: {options.InputFile}");
                    Environment.Exit(1);
                }

                Console.WriteLine($"Sorting file: {options.InputFile}");
                Console.WriteLine($"Output file: {options.OutputFile}");
                Console.WriteLine($"Max memory: {options.MaxMemoryPerChunk}");

                var sorter = new FileSorter();
                var stopwatch = Stopwatch.StartNew();
                await sorter.Sort(options);
                stopwatch.Stop();

                var inputInfo = new FileInfo(options.InputFile);
                var outputInfo = new FileInfo(options.OutputFile);
                Console.WriteLine($"Sorted successfully");
                Console.WriteLine($"Input: {inputInfo.Length.FormatBytes()}");
                Console.WriteLine($"Output: {outputInfo.Length.FormatBytes()}");
                Console.WriteLine($"Time elapsed: {stopwatch.Elapsed:hh\\:mm\\:ss}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);
                Environment.Exit(1);
            }
        }
    }
}
