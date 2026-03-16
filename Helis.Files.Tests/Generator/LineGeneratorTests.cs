using Helis.Files.Generator;

namespace Helis.Files.Tests.Generator
{
    [TestClass]
    public sealed class LineGeneratorTests
    {
        private ILineGenerator _lineGenerator = null!;

        [TestInitialize]
        public void Setup()
        {
            _lineGenerator = new LineGenerator();
        }

        [TestMethod]
        public void GenerateLineFromList_ReturnsNonEmptyString()
        {
            string result = _lineGenerator.GenerateLineFromList();
            
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void GenerateLineFromList_ContainsDotSeparator()
        {
            string result = _lineGenerator.GenerateLineFromList();
            
            Assert.Contains(". ", result, "Line should contain '. ' separator");
        }

        [TestMethod]
        public void GenerateLineFromList_StartsWithNumber()
        {
            string result = _lineGenerator.GenerateLineFromList();
            string numberPart = result.Split('.')[0];
            
            Assert.IsTrue(long.TryParse(numberPart, out _), "Line should start with a number");
        }

        [TestMethod]
        public void GenerateLineFromList_MultipleCallsReturnDifferentNumbers()
        {
            var results = new HashSet<string>();
            for (int i = 0; i < 100; i++)
            {
                results.Add(_lineGenerator.GenerateLineFromList().Split('.')[0]);
            }
            
            Assert.IsGreaterThan(1, results.Count, "Multiple calls should generate different numbers");
        }

        [TestMethod]
        public void GenerateRandomLine_ReturnsNonEmptyString()
        {
            string result = _lineGenerator.GenerateRandomLine();
            
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void GenerateRandomLine_ContainsDotSeparator()
        {
            string result = _lineGenerator.GenerateRandomLine();
            
            Assert.Contains(". ", result, "Line should contain '. ' separator");
        }

        [TestMethod]
        public void GenerateRandomLine_StartsWithNumber()
        {
            string result = _lineGenerator.GenerateRandomLine();
            string numberPart = result.Split('.')[0];
            
            Assert.IsTrue(long.TryParse(numberPart, out _), "Line should start with a number");
        }

        [TestMethod]
        public void GenerateRandomLine_TextPartLengthWithinExpectedRange()
        {
            string result = _lineGenerator.GenerateRandomLine();
            string textPart = result.Split(new[] { ". " }, StringSplitOptions.None)[1];
            
            Assert.IsTrue(textPart.Length >= 10 && textPart.Length <= 200, 
                $"Text part length should be between 10 and 200 characters, but was {textPart.Length}");
        }

        [TestMethod]
        public void GenerateRandomLine_MultipleCallsReturnDifferentContent()
        {
            var results = new HashSet<string>();
            for (int i = 0; i < 100; i++)
            {
                results.Add(_lineGenerator.GenerateRandomLine());
            }
            
            Assert.IsGreaterThan(90, results.Count, "Multiple calls should generate different content");
        }        
    }
}
