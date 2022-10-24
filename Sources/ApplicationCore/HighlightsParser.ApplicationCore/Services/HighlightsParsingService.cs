using HighlightsParser.ApplicationCore.Models;
using HighlightsParser.Infrastructure.Extensions;
using HighlightsParser.Infrastructure.Logging;

namespace HighlightsParser.ApplicationCore.Services
{
    public interface IHighlightsParsingService
    {
        List<ParsingResult> Parse(List<string> multiLineHighlights);
    }

    public class HighlightsParsingService : IHighlightsParsingService
    {
        private readonly IAppLogger _logger;

        public HighlightsParsingService(IAppLogger logger)
        {
            _logger = logger;
        }

        public List<ParsingResult> Parse(List<string> multiLineHighlights)
        {
            var result = new List<ParsingResult>();

            if (!multiLineHighlights.Any())
            {
                return result;
            }

            var highlightsByTitleDictionary = new Dictionary<string, List<string>>();            
            foreach (var multiLineHighlight in multiLineHighlights)
            {
                var highlightLines = multiLineHighlight.SplitByLineExcludingEmptyLines();

                if (highlightLines.Count != 3)
                {
                    this._logger.Warning(
                        "Ignoring multi line highlight with less than 3 lines. Content: {multiLineHighlight}",
                        multiLineHighlight);
                    continue;
                }

                var title = highlightLines[0];
                var highlight = highlightLines[2];
                if (highlightsByTitleDictionary.ContainsKey(title))
                {
                    highlightsByTitleDictionary[title].Add(highlight);
                }
                else
                {
                    highlightsByTitleDictionary.Add(title, new List<string> { highlight });
                }
            }

            foreach (var linesByTitle in highlightsByTitleDictionary)
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
