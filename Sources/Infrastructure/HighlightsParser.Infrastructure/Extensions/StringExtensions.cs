namespace HighlightsParser.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static List<string> SplitByLineExcludingEmptyLines(this string str)
        {
            var result = str
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .ToList()
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            return result;
        }

        public static List<string> SplitByDividerExcludingEmptyLines(this string str)
        {
            var result = str
                .Split("==========")
                .ToList()
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            return result;
        }

        public static string RemoveInvalidCharactersForFileName(this string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();

            var result = string.Concat(fileName.Split(invalidChars));

            return result;
        }
    }
}
