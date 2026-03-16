using System.Diagnostics;

namespace Helis.Files.Generator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var parser = new GeneratorCommandLineParser();
                var options = parser.Parse(args);

                Console.WriteLine($"Generating file: {options.OutputFile}");
                Console.WriteLine($"Target size: {options.FileSizeInBytes.FormatBytes()}");

                var generator = new FileGenerator();
                var stopwatch = Stopwatch.StartNew();
                long linesGenerated = generator.GenerateFile(options);
                stopwatch.Stop();

                var fileInfo = new FileInfo(options.OutputFile);
                Console.WriteLine($"Generated {linesGenerated:N0} lines");
                Console.WriteLine($"File size: {fileInfo.Length.FormatBytes()}");
                Console.WriteLine($"Time elapsed: {stopwatch.Elapsed:hh\\:mm\\:ss}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}
