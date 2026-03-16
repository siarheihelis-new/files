namespace Helis.Files.Generator
{
    /// <summary>
    /// Interface for file generation. 
    /// </summary>
    internal interface IFileGenerator
    {
        /// <summary>
        /// Generates a file based on the provided options and returns the amount of generated lines.
        /// </summary>
        /// <param name="options">Generator options</param>
        /// <returns>Amout of generated lines</returns>
        long GenerateFile(GeneratorOptions options) ;
    }
}
