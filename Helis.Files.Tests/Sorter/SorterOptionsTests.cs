using Helis.Files.Sorter;

namespace Helis.Files.Tests.Sorter
{
    [TestClass]
    public sealed class SorterOptionsTests
    {
        [TestMethod]
        public void Constructor_SetsInputAndOutputFiles()
        {
            var options = new SorterOptions("input.txt", "output.txt");
            
            Assert.AreEqual("input.txt", options.InputFile);
            Assert.AreEqual("output.txt", options.OutputFile);
        }
       
        [TestMethod]
        public void InputFile_CanBeModified()
        {
            var options = new SorterOptions("input.txt", "output.txt")
            {
                InputFile = "new_input.txt"
            };
            
            Assert.AreEqual("new_input.txt", options.InputFile);
        }

        [TestMethod]
        public void OutputFile_CanBeModified()
        {
            var options = new SorterOptions("input.txt", "output.txt")
            {
                OutputFile = "new_output.txt"
            };
            
            Assert.AreEqual("new_output.txt", options.OutputFile);
        }

        [TestMethod]
        public void AllProperties_CanBeSetViaInitializer()
        {
            var options = new SorterOptions("in.txt", "out.txt")
            {
                MergeFilesPerRun = 8,
                MaxMemoryPerChunk = 16 * 1024 * 1024,
                MaxInMemoryChunkSize = 100_000,
                MaxDegreeOfParallelism = 2,
                BufferSize = 512
            };
            
            Assert.AreEqual(8, options.MergeFilesPerRun);
            Assert.AreEqual(16 * 1024 * 1024, options.MaxMemoryPerChunk);
            Assert.AreEqual(100_000, options.MaxInMemoryChunkSize);
            Assert.AreEqual(2, options.MaxDegreeOfParallelism);
            Assert.AreEqual(512, options.BufferSize);
        }
    }
}
