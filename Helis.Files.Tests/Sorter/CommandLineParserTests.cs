using Helis.Files.Sorter;

namespace Helis.Files.Tests.Sorter
{
    [TestClass]
    public sealed class CommandLineParserTests
    {
        [TestMethod]
        public void Parse_MinimumArguments_CreatesSorterOptions()
        {
            string inputFile = "input.txt";
            string outputFile = "output.txt";
            string[] args = { inputFile, outputFile };
            
            var options = CommandLineParser.Parse(args);
            
            Assert.IsNotNull(options);
            Assert.AreEqual(inputFile, options.InputFile);
            Assert.AreEqual(outputFile, options.OutputFile);
        }

        [TestMethod]
        public void Parse_WithMemoryArgument_SetsMaxMemory()
        {
            string[] args = { "input.txt", "output.txt", "64" };
            
            var options = CommandLineParser.Parse(args);
            
            Assert.IsNotNull(options);
            Assert.AreEqual("input.txt", options.InputFile);
            Assert.AreEqual("output.txt", options.OutputFile);
            Assert.AreEqual(64 * 1024 * 1024, options.MaxMemoryPerChunk);
        }

        [TestMethod]
        public void Parse_WithSmallMemory_SetsCorrectly()
        {
            string[] args = { "input.txt", "output.txt", "1" };
            
            var options = CommandLineParser.Parse(args);
            
            Assert.AreEqual(1 * 1024 * 1024, options.MaxMemoryPerChunk);
        }

        [TestMethod]
        public void Parse_WithLargeMemory_SetsCorrectly()
        {
            string[] args = { "input.txt", "output.txt", "512" };
            
            var options = CommandLineParser.Parse(args);
            
            Assert.AreEqual(512 * 1024 * 1024, options.MaxMemoryPerChunk);
        }

        [TestMethod]
        public void Parse_NoArguments_ThrowsArgumentException()
        {
            string[] args = Array.Empty<string>();

            try
            {
                CommandLineParser.Parse(args);
                Assert.Fail("Expected ArgumentException to be thrown");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Usage");
            }
        }

        [TestMethod]
        public void Parse_OneArgument_ThrowsArgumentException()
        {
            string[] args = { "input.txt" };

            try
            {
                CommandLineParser.Parse(args);
                Assert.Fail("Expected ArgumentException to be thrown");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Usage");
            }
        }

        [TestMethod]
        public void Parse_InvalidMemoryValue_ThrowsArgumentException()
        {
            string[] args = { "input.txt", "output.txt", "invalid" };

            try
            {
                CommandLineParser.Parse(args);
                Assert.Fail("Expected ArgumentException to be thrown");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Invalid memory value");
            }
        }

        [TestMethod]
        public void Parse_NegativeMemoryValue_ThrowsArgumentException()
        {
            string[] args = { "input.txt", "output.txt", "-10" };

            try
            {
                CommandLineParser.Parse(args);
                Assert.Fail("Expected ArgumentException to be thrown");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Invalid memory value");
            }
        }

        [TestMethod]
        public void Parse_ZeroMemoryValue_ThrowsArgumentException()
        {
            string[] args = { "input.txt", "output.txt", "0" };

            try
            {
                CommandLineParser.Parse(args);
                Assert.Fail("Expected ArgumentException to be thrown");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Invalid memory value");
            }
        }

        [TestMethod]
        public void Parse_DifferentFilePaths_SetsCorrectly()
        {
            string[] args = { "/path/to/input.txt", "/path/to/output.txt" };
            
            var options = CommandLineParser.Parse(args);
            
            Assert.AreEqual("/path/to/input.txt", options.InputFile);
            Assert.AreEqual("/path/to/output.txt", options.OutputFile);
        }
    }
}
