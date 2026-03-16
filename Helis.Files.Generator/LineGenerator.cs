
namespace Helis.Files.Generator
{
    internal class LineGenerator : ILineGenerator
    {
        private const string CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%,1234567890";
        private const int MaxLineLength = 200;
        private const int MinLineLength = 10;
        private static readonly string[] Strings = new[]
        {
            "Apple", "Banana is yellow", "Cherry is the best", "Date palm tree",
            "Elderberry jam", "Fig spread", "Grape juice", "Honeydew melon",
            "Jackfruit surprise", "Kiwi fruit", "Lemon zest", "Mango lassi",
            "Nectarine blend", "Orange juice fresh", "Papaya smoothie", "Quince preserve",
            "Raspberry tart", "Strawberry field", "Tangerine sweet", "Ugli fruit",
            "Vanilla bean", "Watermelon slice", "Xigua variant", "Yam root",
            "Zucchini bread"
        };
        private readonly Random _random = new Random();        
        private readonly char[] _buffer  = new char[MaxLineLength];

        
        public string GenerateLineFromList()
        {
            string text = Strings[_random.Next(Strings.Length)];
            return $"{_random.NextInt64()}. {text}";
        }

        ///<remarks>Not thread-safe.</remarks>
        public string GenerateRandomLine()
        {
            int size = _random.Next(MinLineLength, MaxLineLength);
            for (int i = 0; i < size; i++)
            {
                _buffer[i] = CharSet[_random.Next(CharSet.Length)];
            }
            return $"{_random.NextInt64()}. {new string(_buffer, 0, size)}";
        }
    }
}
