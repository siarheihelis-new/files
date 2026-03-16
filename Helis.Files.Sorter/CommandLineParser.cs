namespace Helis.Files.Sorter
{
    internal static class CommandLineParser
    {
        public static SorterOptions Parse(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException("Usage: Helis.Files.Sorter <inputFile> <outputFile> [maxMemoryMB]");
            }
            var options = new SorterOptions(args[0], args[1]);

            if (args.Length > 2)
            {
                if (!int.TryParse(args[2], out int maxMemoryMB) || maxMemoryMB <= 0)
                {
                    throw new ArgumentException($"Invalid memory value: {args[2]}. Must be a positive integer.");
                }
                options.MaxMemoryPerChunk = maxMemoryMB*1024*1024;
            }

            return options;
        }
    }
}
