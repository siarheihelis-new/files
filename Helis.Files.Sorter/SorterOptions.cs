namespace Helis.Files.Sorter
{
    public class SorterOptions
    {
        public SorterOptions(string inputFile, string outputFile)
        {
            InputFile = inputFile;
            OutputFile = outputFile;
            BaseTempDirectory = Path.Combine(Path.GetTempPath(), $"sorter_{Guid.NewGuid()}");            
        }
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public int MergeFilesPerRun { get; set; } = 32;
        public int MaxMemoryPerChunk { get; set; } = 32 * 1024 * 1024;
        public int MaxInMemoryChunkSize { get; set; } = 1_000_000;
        public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount; // Default to number of CPU cores. Probably it will be better to divide it by 2
        public int BufferSize { get; set; } = 1024*1024; // Default is 1Mb
        public string BaseTempDirectory { 
            get;
            set
            {
                field = value;
                if (!Directory.Exists(BaseTempDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(BaseTempDirectory);
                    }
                    catch
                    {
                        //If we can't create directory, we should fallback to system temp directory to avoid failure of whole sorting process
                        field = Path.GetTempPath();
                    }
                }
                
            }
        }

    }
}
