namespace Helis.Files.Tests
{
    [TestClass]
    public sealed class IdeaProvalTests
    {
        [TestMethod]
        [DataRow("2345", "1234")]
        [DataRow("7891234", "4567891")]        
        public void NumberString_CanBeCompared_WithoutParse_GreaterNumbersTest(string firstNumber, string secondNumber)
        {
            if(firstNumber.Length != secondNumber.Length)
            {
                Assert.Inconclusive("Numbers must have the same length");
            }
            int actualResult = firstNumber.AsSpan().SequenceCompareTo(secondNumber.AsSpan());
            Assert.IsGreaterThan(0, actualResult, "Comparison works incorrectly");
        }

        [TestMethod]
        [DataRow("1234", "2345")]
        [DataRow("4567891", "7891234")] 
        public void NumberString_CanBeCompared_WithoutParse_LessNumbersTest(string firstNumber, string secondNumber)
        {

            if (firstNumber.Length != secondNumber.Length)
            {
                Assert.Inconclusive("Numbers must have the same length");
            }
            int actualResult = firstNumber.AsSpan().SequenceCompareTo(secondNumber.AsSpan());
            Assert.IsLessThan(0, actualResult, "Comparison works incorrectly");
        }

        [TestMethod]
        [DataRow("123456789", "123456789")]
        public void NumberString_CanBeCompared_WithoutParse_EqualNumbersTest(string firstNumber, string secondNumber)
        {
            if (firstNumber.Length != secondNumber.Length)
            {
                Assert.Inconclusive("Numbers must have the same length");
            }
            int actualResult = firstNumber.AsSpan().SequenceCompareTo(secondNumber.AsSpan());
            Assert.AreEqual(0, actualResult, "Comparison works incorrectly");
        }
    }
}
