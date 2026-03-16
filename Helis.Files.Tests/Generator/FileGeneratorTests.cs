using Helis.Files.Generator;

namespace Helis.Files.Tests.Generator
{
    [TestClass]
    public sealed class FileGeneratorTests
    {
        private IFileGenerator _fileGenerator = null!;
        private string _tempFilePath = null!;

        [TestInitialize]
        public void Setup()
        {
            _fileGenerator = new FileGenerator();
            _tempFilePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.txt");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        [TestMethod]
        public void GenerateFile_CreatesFile()
        {
            var options = new GeneratorOptions(_tempFilePath, 1024);
            
            _fileGenerator.GenerateFile(options);
            
            Assert.IsTrue(File.Exists(_tempFilePath), "File should be created");
        }

        [TestMethod]
        public void GenerateFile_ReturnsNumberOfLines()
        {
            var options = new GeneratorOptions(_tempFilePath, 1024);
            
            long linesGenerated = _fileGenerator.GenerateFile(options);
            
            Assert.IsGreaterThan(0, linesGenerated, "Should return number of lines generated");
        }

        [TestMethod]
        public void GenerateFile_FileSizeApproximatelyMatchesRequest()
        {
            long requestedSize = 10 * 1024;
            var options = new GeneratorOptions(_tempFilePath, requestedSize);
            
            _fileGenerator.GenerateFile(options);
            var fileInfo = new FileInfo(_tempFilePath);
            
            Assert.IsGreaterThanOrEqualTo(requestedSize, fileInfo.Length, $"File size {fileInfo.Length} should be at least {requestedSize}");
            Assert.IsLessThan(requestedSize * 1.5, fileInfo.Length, $"File size {fileInfo.Length} should not exceed {requestedSize * 1.5}");
        }

        [TestMethod]
        public void GenerateFile_GeneratedLinesCountMatchesFileContent()
        {
            var options = new GeneratorOptions(_tempFilePath, 5 * 1024);
            
            long reportedLines = _fileGenerator.GenerateFile(options);
            long actualLines = File.ReadLines(_tempFilePath).Count();
            
            Assert.AreEqual(reportedLines, actualLines, "Reported line count should match actual line count");
        }

        [TestMethod]
        public void GenerateFile_AllLinesHaveCorrectFormat()
        {
            var options = new GeneratorOptions(_tempFilePath, 2 * 1024);
            
            _fileGenerator.GenerateFile(options);
            var lines = File.ReadAllLines(_tempFilePath);
            
            foreach (var line in lines)
            {
                Assert.Contains(". ", line, $"Line '{line}' should contain '. ' separator");
                string numberPart = line.Split('.')[0];
                Assert.IsTrue(long.TryParse(numberPart, out _), "Line '{line}' should start with a number");
            }
        }

        [TestMethod]
        public void GenerateFile_OverwritesExistingFile()
        {
            File.WriteAllText(_tempFilePath, "Existing content");
            var options = new GeneratorOptions(_tempFilePath, 1024);
            
            _fileGenerator.GenerateFile(options);
            string content = File.ReadAllText(_tempFilePath);
            
            Assert.DoesNotContain("Existing content", content, "File should be overwritten");
        }        
    }
}
