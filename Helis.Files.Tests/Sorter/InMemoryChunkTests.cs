using Helis.Files.Sorter;
using Helis.Files.Sorter.Impl;

namespace Helis.Files.Tests.Sorter
{
    [TestClass]
    public sealed class InMemoryChunkTests
    {
        private readonly SorterOptions _options = new SorterOptions("input.txt", "output.txt")
        {
            MaxInMemoryChunkSize = 10,
            MaxMemoryPerChunk = 1000
        };


        [TestMethod]
        public void NewChunk_IsEmpty()
        {
            using var chunk = new InMemoryChunk(_options);
            
            Assert.IsTrue(chunk.IsEmpty);
            Assert.AreEqual(0, chunk.Count);
        }

        [TestMethod]
        public void Add_SingleLine_IncrementsCount()
        {
            using var chunk = new InMemoryChunk(_options);
            
            chunk.Add("1. test");
            
            Assert.IsFalse(chunk.IsEmpty);
            Assert.AreEqual(1, chunk.Count);
        }

        [TestMethod]
        public void Add_MultipleLines_IncrementsCount()
        {
            using var chunk = new InMemoryChunk(_options);
            
            chunk.Add("1. apple");
            chunk.Add("2. banana");
            chunk.Add("3. cherry");
            
            Assert.AreEqual(3, chunk.Count);
        }

        [TestMethod]
        public void IsFull_WhenCountReachesMax_ReturnsTrue()
        {
            using var chunk = new InMemoryChunk(_options);
            
            for (int i = 0; i < _options.MaxInMemoryChunkSize; i++)
            {
                chunk.Add($"{i}. test");
            }
            
            Assert.IsTrue(chunk.IsFull);
        }


        [TestMethod]
        public void Sort_OrdersLinesCorrectly()
        {
            using var chunk = new InMemoryChunk(_options);
            
            chunk.Add("3. zebra");
            chunk.Add("1. apple");
            chunk.Add("2. banana");
            
            chunk.Sort();

            using (var ms = new MemoryStream())
            {
                using var writer = new StreamWriter(ms);
                chunk.WriteTo(writer);
                writer.Flush();

                ms.Position = 0;
                using (var reader = new StreamReader(ms))
                {

                    Assert.AreEqual("1. apple", reader.ReadLine());
                    Assert.AreEqual("2. banana", reader.ReadLine());
                    Assert.AreEqual("3. zebra", reader.ReadLine());
                }
            }
        }

        [TestMethod]
        public void WriteTo_WritesAllLines()
        {
            using var chunk = new InMemoryChunk(_options);
            
            chunk.Add("1. first");
            chunk.Add("2. second");
            chunk.Add("3. third");

            using (var ms = new MemoryStream())
            {
                using var writer = new StreamWriter(ms);
                chunk.WriteTo(writer);
                writer.Flush();

                ms.Position = 0;
                using var reader = new StreamReader(ms);
                string content = reader.ReadToEnd();

                StringAssert.Contains(content, "1. first");
                StringAssert.Contains(content, "2. second");
                StringAssert.Contains(content, "3. third");
            }
        }

        [TestMethod]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            var chunk = new InMemoryChunk(_options);
            chunk.Add("1. test");

            chunk.Dispose();
            chunk.Dispose(); // Should not throw
        }

        [TestMethod]
        public void Sort_ComplexComparison_OrdersCorrectly()
        {
            using var chunk = new InMemoryChunk(_options);
            
            chunk.Add("100. banana");
            chunk.Add("5. apple");
            chunk.Add("50. apple");
            chunk.Add("10. cherry");
            
            chunk.Sort();

            using (var ms = new MemoryStream())
            {
                using var writer = new StreamWriter(ms);
                chunk.WriteTo(writer);
                writer.Flush();

                ms.Position = 0;
                using (var reader = new StreamReader(ms))
                {

                    Assert.AreEqual("5. apple", reader.ReadLine());
                    Assert.AreEqual("50. apple", reader.ReadLine());
                    Assert.AreEqual("100. banana", reader.ReadLine());
                    Assert.AreEqual("10. cherry", reader.ReadLine());
                }
            }
        }
    }
}
