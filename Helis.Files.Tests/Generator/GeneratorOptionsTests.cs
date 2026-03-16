using Helis.Files.Generator;

namespace Helis.Files.Tests.Generator
{
    [TestClass]
    public sealed class GeneratorOptionsTests
    {
        [TestMethod]
        public void Constructor_WithOutputFile_SetsOutputFile()
        {
            var options = new GeneratorOptions("test.txt");
            
            Assert.AreEqual("test.txt", options.OutputFile);
        }

        [TestMethod]
        public void Constructor_WithOutputFileAndSize_SetsCorrectly()
        {
            var options = new GeneratorOptions("test.txt", 2048L);
            
            Assert.AreEqual("test.txt", options.OutputFile);
            Assert.AreEqual(2048L, options.FileSizeInBytes);
        }
        
    }
}
