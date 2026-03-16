using Helis.Files.Sorter.Impl;

namespace Helis.Files.Tests.Sorter
{
    [TestClass]
    public sealed class LineComparerTests
    {
        private readonly LineComparer _comparer = new LineComparer();
        
        [TestMethod]
        [DataRow(null, null)]
        [DataRow("12345. apple", "12345. apple")]
        public void Compare_IdenticalLines_ReturnsZero(string line1, string line2)
        {
            int result = _comparer.Compare(line1, line2);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [DataRow("555. data", "111. data", "555 should be greater than 111")]
        [DataRow("123. zebra", "456. apple", "zebra should be greater than apple")]
        [DataRow("789. apple", "456. apple", "123 should be less than 456")]
        [DataRow("123. test", null,"")]
        public void Compare_FirstIsGreaterUseCases_ComparesCorrectly(string line1, string line2, string message)
        {
            int result = _comparer.Compare(line1, line2);
            Assert.IsGreaterThan(0, result, message);
        }

        [TestMethod]
        [DataRow("1. Apple", "2. apple", "Apple (uppercase) should be less than apple (lowercase)")]//upper case and lower case
        [DataRow("100. test@123", "200. test@456", "test@123 should be less than test@456")] //special characters
        [DataRow("999999999. test", "1000000000. test", "999999999 should be less than 1000000000")] //large numbers
        [DataRow("12. test", "1234. test", "12 should be less than 1234")] //different number lengths
        [DataRow("123. apple", "456. banana", "apple should be less than banana")] //different strings
        [DataRow("123. apple", "456. apple", "123 should be less than 456")] //numbers with same length
        [DataRow(null, "123. test","")]
        public void Compare_FirstIsLessUseCases_ComparesCorrectly(string line1, string line2, string message)
        {
            int result = _comparer.Compare(line1, line2);
            Assert.IsLessThan(0, result, message);
        }
    }
}
