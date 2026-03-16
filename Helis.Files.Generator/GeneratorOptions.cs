namespace Helis.Files.Generator
{
    public class GeneratorOptions
    {
        public GeneratorOptions(string outputFile)
        {
            OutputFile = outputFile;
        }

        public GeneratorOptions(string outputFile, long fileSizeInBytes)
        {
            OutputFile = outputFile;
            FileSizeInBytes = fileSizeInBytes;
        }
        /// <summary>
        /// Path to output file. If the file already exists, it will be overwritten.
        /// </summary>
        public string OutputFile { get; set; }

        /// <summary>
        /// Gets or sets the size of the file, in bytes. The default value is 1 GB.
        /// </summary>
        public long FileSizeInBytes { get; set; } = 1024 * 1024 * 1024;// 1GB default

        public double PercentOfDuplicateLines { get; set; } = 0.1; // Default 0.1% 1 duplicate line per 1000 lines
        public int BufferSize { get; set; } = 65536; // Default buffer size for StreamWriter    
    }
}
