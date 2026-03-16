using Helis.Files.Sorter;
using Helis.Files.Sorter.Impl;

namespace Helis.Files.Tests.Sorter
{
    [TestClass]
    public sealed class MergeSortTests
    {
        private SorterOptions _options = null!;
        private string _tempDirectory = null!;

        [TestInitialize]
        public void Setup()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), $"MergeSortTests_{Guid.NewGuid()}");
            Directory.CreateDirectory(_tempDirectory);
            
            _options = new SorterOptions(
                Path.Combine(_tempDirectory, "input.txt"),
                Path.Combine(_tempDirectory, "output.txt"))
            {
                MaxDegreeOfParallelism = 2,
                MergeFilesPerRun = 2
            };
        }

        

        [TestMethod]
        public async Task MergeChunksAsync_NoChunks_CreatesEmptyFile()
        {
            var mergeSort = new MergeSort();
            var chunkFiles = new List<string>();
            
            await mergeSort.MergeChunksAsync(_options, chunkFiles);
            
            Assert.IsTrue(File.Exists(_options.OutputFile));
            string content = await File.ReadAllTextAsync(_options.OutputFile);
            Assert.AreEqual("", content);
        }

        [TestMethod]
        public async Task MergeChunksAsync_SingleChunk_CopiesFile()
        {
            var mergeSort = new MergeSort();
            string chunkFile = Path.Combine(_tempDirectory, "chunk1.txt");
            await File.WriteAllLinesAsync(chunkFile, new[] { "1. test", "2. data" });
            
            var chunkFiles = new List<string> { chunkFile };
            
            await mergeSort.MergeChunksAsync(_options, chunkFiles);
            
            Assert.IsTrue(File.Exists(_options.OutputFile));
            string content = await File.ReadAllTextAsync(_options.OutputFile);
            StringAssert.Contains(content, "1. test");
            StringAssert.Contains(content, "2. data");
        }

        [TestMethod]
        public async Task MergeChunksAsync_TwoSortedChunks_MergesCorrectly()
        {
            var mergeSort = new MergeSort();
            
            string chunk1 = Path.Combine(_tempDirectory, "chunk1.txt");
            string chunk2 = Path.Combine(_tempDirectory, "chunk2.txt");
            
            await File.WriteAllLinesAsync(chunk1, new[] { "1. apple", "3. cherry" });
            await File.WriteAllLinesAsync(chunk2, new[] { "2. banana", "4. date" });
            
            var chunkFiles = new List<string> { chunk1, chunk2 };
            
            await mergeSort.MergeChunksAsync(_options, chunkFiles);

            Assert.IsTrue(File.Exists(_options.OutputFile));
            string[] lines = await File.ReadAllLinesAsync(_options.OutputFile);

            Assert.HasCount(4, lines);
            Assert.AreEqual("1. apple", lines[0]);
            Assert.AreEqual("2. banana", lines[1]);
            Assert.AreEqual("3. cherry", lines[2]);
            Assert.AreEqual("4. date", lines[3]);
        }

        [TestMethod]
        public async Task MergeChunksAsync_MultipleSortedChunks_MergesCorrectly()
        {
            var mergeSort = new MergeSort();
            
            string chunk1 = Path.Combine(_tempDirectory, "chunk_merge_1.txt");
            string chunk2 = Path.Combine(_tempDirectory, "chunk_merge_2.txt");
            string chunk3 = Path.Combine(_tempDirectory, "chunk_merge_3.txt");
            
            await File.WriteAllLinesAsync(chunk1, new[] { "1. aaa", "7. ggg" });
            await File.WriteAllLinesAsync(chunk2, new[] { "3. ccc", "5. eee" });
            await File.WriteAllLinesAsync(chunk3, new[] { "2. bbb", "4. ddd", "6. fff" });
            
            var chunkFiles = new List<string> { chunk1, chunk2, chunk3 };
            
            await mergeSort.MergeChunksAsync(_options, chunkFiles);

            Assert.IsTrue(File.Exists(_options.OutputFile));
            string[] lines = await File.ReadAllLinesAsync(_options.OutputFile);

            Assert.HasCount(7, lines);
            Assert.AreEqual("1. aaa", lines[0]);
            Assert.AreEqual("2. bbb", lines[1]);
            Assert.AreEqual("3. ccc", lines[2]);
            Assert.AreEqual("4. ddd", lines[3]);
            Assert.AreEqual("5. eee", lines[4]);
            Assert.AreEqual("6. fff", lines[5]);
            Assert.AreEqual("7. ggg", lines[6]);
        }

        [TestMethod]
        public async Task MergeChunksAsync_ChunksWithSameStringDifferentNumbers_MergesCorrectly()
        {
            var mergeSort = new MergeSort();
            
            string chunk1 = Path.Combine(_tempDirectory, "chunk_numbers_1.txt");
            string chunk2 = Path.Combine(_tempDirectory, "chunk_numbers_2.txt");
            
            await File.WriteAllLinesAsync(chunk1, new[] { "10. test", "30. test" });
            await File.WriteAllLinesAsync(chunk2, new[] { "20. test", "40. test" });
            
            var chunkFiles = new List<string> { chunk1, chunk2 };
            
            await mergeSort.MergeChunksAsync(_options, chunkFiles);

            Assert.IsTrue(File.Exists(_options.OutputFile));
            string[] lines = await File.ReadAllLinesAsync(_options.OutputFile);

            Assert.HasCount(4, lines);
            Assert.AreEqual("10. test", lines[0]);
            Assert.AreEqual("20. test", lines[1]);
            Assert.AreEqual("30. test", lines[2]);
            Assert.AreEqual("40. test", lines[3]);
        }

        
        [TestMethod]
        public async Task MergeChunksAsync_EmptyChunks_HandlesGracefully()
        {
            var mergeSort = new MergeSort();
            
            string chunk1 = Path.Combine(_tempDirectory, "chunk1.txt");
            string chunk2 = Path.Combine(_tempDirectory, "chunk2.txt");
            
            await File.WriteAllTextAsync(chunk1, "");
            await File.WriteAllLinesAsync(chunk2, new[] { "1. test" });
            
            var chunkFiles = new List<string> { chunk1, chunk2 };
            
            await mergeSort.MergeChunksAsync(_options, chunkFiles);

            Assert.IsTrue(File.Exists(_options.OutputFile));
            string[] lines = await File.ReadAllLinesAsync(_options.OutputFile);

            Assert.HasCount(1, lines);
            Assert.AreEqual("1. test", lines[0]);
        }

        [TestMethod]
        public async Task MergeChunksAsync_LargeNumberOfChunks_MergesInMultipleRounds()
        {
            var mergeSort = new MergeSort();
            var chunkFiles = new List<string>();
            
            // Create 5 chunks that will require multiple merge rounds
            for (int i = 0; i < 5; i++)
            {
                string chunk = Path.Combine(_tempDirectory, $"chunk_large_case_{i}.txt");
                await File.WriteAllLinesAsync(chunk, new[] { $"{i}. item{i}" });
                chunkFiles.Add(chunk);
            }
            
            await mergeSort.MergeChunksAsync(_options, chunkFiles);

            Assert.IsTrue(File.Exists(_options.OutputFile));
            string[] lines = await File.ReadAllLinesAsync(_options.OutputFile);

            Assert.HasCount(5, lines);
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual($"{i}. item{i}", lines[i]);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(_tempDirectory))
            {
                try
                {
                    Directory.Delete(_tempDirectory, true);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }
    }
}
