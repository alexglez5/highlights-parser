using HighlightsParser.ApplicationCore.Models;
using HighlightsParser.Infrastructure.Logging;

namespace HighlightsParser.ApplicationCore.Services
{
    public interface IHighlightsParsingService
    {
        List<ParsingResult> Parse(List<string> lines);
    }

    public class HighlightsParsingService : IHighlightsParsingService
    {
        private readonly IAppLogger _logger;

        public HighlightsParsingService(IAppLogger logger)
        {
            _logger = logger;
        }

        public List<ParsingResult> Parse(List<string> lines)
        {
            var result = new List<ParsingResult>();

            if (!lines.Any())
            {
                return result;
            }

            var linesCount = lines.Count;
            if (linesCount % 4 != 0)
            {
                _logger.Error(
                    "Line count must be a multiple of 4, but it is {count} which is not multiple of 4.",
                    linesCount);

                return result;
            }

            var linesByTitleDictionary = new Dictionary<string, List<string>>();
            for (var i = 0; i < linesCount; i += 4)
            {
                var title = lines[i];
                var highlight = lines[i + 2];
                if (linesByTitleDictionary.ContainsKey(title))
                {
                    linesByTitleDictionary[title].Add(highlight);
                }
                else
                {
                    linesByTitleDictionary.Add(title, new List<string> { highlight });
                }
            }

            foreach (var linesByTitle in linesByTitleDictionary)
            {
                var parsingResult = new ParsingResult
                {
                    Title = linesByTitle.Key,
                    Highlights = linesByTitle.Value
                };

                result.Add(parsingResult);
            }

            return result;
        }
    }
}
