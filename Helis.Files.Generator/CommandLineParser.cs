namespace Helis.Files.Generator
{
    public class GeneratorCommandLineParser
    {
        public GeneratorOptions Parse(string[] args)
        {
            if(args.Length == 0)
            {
                throw new ArgumentException("No arguments provided. Please specify the output file and optionally the file size.");
            }
            var options = new GeneratorOptions(args[0]);
            if (args.Length > 1)
            {
                options.FileSizeInBytes = args[1].ToFileSize();
            }

            return options;
        }        
    }
}
