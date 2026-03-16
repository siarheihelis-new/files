namespace Helis.Files.Generator
{
    public class FileGenerator : IFileGenerator
    {
        private readonly ILineGenerator _lineGenerator = new LineGenerator();
        public long GenerateFile(GeneratorOptions options)
        {
            long currentSize = 0;
            long linesGenerated = 0;
            int stepForDuplicates = (int)Math.Round(100.0 / options.PercentOfDuplicateLines);
            int bytesForNewLine = System.Text.Encoding.UTF8.GetByteCount(Environment.NewLine);
            using (var writer = new StreamWriter(options.OutputFile, false, System.Text.Encoding.UTF8, options.BufferSize))
            {
                while (currentSize < options.FileSizeInBytes)
                {
                    string line;
                    if (linesGenerated % stepForDuplicates == 0)
                    {
                        //write line from pre-defined list to have duplicates
                        line = _lineGenerator.GenerateLineFromList();
                    }
                    else
                    {
                        line = _lineGenerator.GenerateRandomLine();
                    }
                    writer.WriteLine(line);
                    currentSize += System.Text.Encoding.UTF8.GetByteCount(line) + bytesForNewLine;
                    linesGenerated++;

                    if (linesGenerated % 100_000 == 0)
                    {
                        //Hardcoded console write will be removed in future, but for now it is useful to track progress when generating large files
                        Console.Write($"\rProgress: {currentSize.FormatBytes()} / {options.FileSizeInBytes.FormatBytes()}");
                    }
                }
                Console.Write($"\rProgress: {currentSize.FormatBytes()} / {options.FileSizeInBytes.FormatBytes()}");
            }
            Console.WriteLine();
            return linesGenerated;
        }

    }
}
