namespace HighlightsParser.ApplicationCore.Models
{
    public class ParsingResult
    {
        public string Title { get; set; } = string.Empty;

        public List<string> Highlights { get; set; } = new();
    }
}
