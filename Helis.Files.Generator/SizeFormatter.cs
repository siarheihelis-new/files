namespace Helis.Files.Generator
{
    public static class SizeFormatter
    {
        public static string FormatBytes(this long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        public static long ToFileSize(this string size)
        {
            if (long.TryParse(size, out long bytes))
            {
                return bytes;
            }

            size = size.ToUpper().Trim();
            if (size.Length < 2)
            {
                throw new ArgumentException($"Invalid size format: {size}");
            }

            if (!double.TryParse(size[..^2], out double value))
            {
                throw new ArgumentException($"Invalid numeric value in size: {size}");
            }

            return size.EndsWith("GB", StringComparison.OrdinalIgnoreCase) ? (long)(value * 1024 * 1024 * 1024) :
                   size.EndsWith("MB", StringComparison.OrdinalIgnoreCase) ? (long)(value * 1024 * 1024) :
                   size.EndsWith("KB", StringComparison.OrdinalIgnoreCase) ? (long)(value * 1024) :
                   throw new ArgumentException($"Unknown size unit: {size}");
        }
    }
}
