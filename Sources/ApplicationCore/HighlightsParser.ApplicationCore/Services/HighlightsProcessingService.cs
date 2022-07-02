using HighlightsParser.Infrastructure.Extensions;
using HighlightsParser.Infrastructure.Logging;
using HighlightsParser.Infrastructure.Services;

namespace HighlightsParser.ApplicationCore.Services
{
    public interface IHighlightsProcessingService
    {
        Task ParseHighlightsFromInputFileToOutputFiles(string inputFileFullPath, string outputFolderFullPath);
    }

    public class HighlightsProcessingService : IHighlightsProcessingService
    {
        private readonly IAppLogger _logger;
        private readonly IFileReaderService _fileReaderService;
        private readonly IHighlightsParsingService _highlightsParsingService;
        private readonly IFileWriterService _fileWriterService;
        private readonly ITimeProvider _timeProvider;

        public HighlightsProcessingService(
            IAppLogger logger,
            IFileReaderService fileReaderService,
            IHighlightsParsingService highlightsParsingService,
            IFileWriterService fileWriterService,
            ITimeProvider timeProvider)
        {
            _logger = logger;
            _fileReaderService = fileReaderService;
            _highlightsParsingService = highlightsParsingService;
            _fileWriterService = fileWriterService;
            _timeProvider = timeProvider;
        }

        public async Task ParseHighlightsFromInputFileToOutputFiles(string inputFileFullPath, string outputFolderFullPath)
        {
            var fileContent = await _fileReaderService.ReadFromFile(inputFileFullPath);

            var parsingResults = _highlightsParsingService.Parse(fileContent);

            var dateForFileName = _timeProvider.GetCurrentDate().ToString("yyyy-MM-dd");
            
            foreach (var parsingResult in parsingResults)
            {
                var validFileName = parsingResult.Title.RemoveInvalidCharactersForFileName();
                var formattedFileName = $"{dateForFileName} {validFileName}.md";
                var outputFileFullPath = $"{outputFolderFullPath}{formattedFileName}";

                var formattedLines = parsingResult.Highlights.Select(h => $"- {h}");
                var newLineSeparatedLines = string.Join(Environment.NewLine, formattedLines);
                var formattedFileContent = 
                    "# " + validFileName + Environment.NewLine + 
                    Environment.NewLine + 
                    newLineSeparatedLines;

                _logger.Information(
                    "Saving {count} highlights into file: {fileName}.",
                    parsingResult.Highlights.Count,
                    validFileName);

                await _fileWriterService.WriteToFile(outputFileFullPath, formattedFileContent);
            }
        }
    }
}
