namespace Helis.Files.Sorter.Impl
{
    internal class LineComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                return -1;
            }
            if (y == null) return 1;
            // Assuming the line format is correct <Number>. <String>
            ReadOnlySpan<char> firstSpan = x.AsSpan();
            int firstIndex = firstSpan.IndexOf('.');
            var firstString = firstSpan[(firstIndex + 2)..];

            ReadOnlySpan<char> secondSpan = y.AsSpan();
            int secondIndex = secondSpan.IndexOf('.');
            var secondString = secondSpan[(secondIndex + 2)..];

            int stringComparisonResult = firstString.SequenceCompareTo(secondString);
            //We need to compare number part if strings are equal
            if (stringComparisonResult == 0)
            {
                // We should not compare numbers as strings for case when strings has different length
                // We can just compare index of dot
                if (secondIndex != firstIndex)
                {
                    return firstIndex.CompareTo(secondIndex);
                }
                else
                {
                    //For case if we have numbers with same string and same length
                    //we can compare strings without parsing them to numbers
                    //This performance improvement done with assumptions that input is correct and we have no leading 0 or empty characters
                    var firstNumber = firstSpan[..firstIndex];
                    var secondNumber = secondSpan[..secondIndex];
                    return firstNumber.SequenceCompareTo(secondNumber);
                }
            }
            return stringComparisonResult;
        }
    }
}
