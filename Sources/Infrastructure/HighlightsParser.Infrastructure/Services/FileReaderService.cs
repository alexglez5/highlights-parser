using HighlightsParser.Infrastructure.Extensions;
using HighlightsParser.Infrastructure.Logging;

namespace HighlightsParser.Infrastructure.Services
{
    public interface IFileReaderService
    {
        Task<List<string>> ReadFromFile(string fileFullPath);
    }

    public class FileReaderService : IFileReaderService
    {
        private readonly IAppLogger _logger;

        public FileReaderService(IAppLogger logger)
        {
            _logger = logger;
        }

        public async Task<List<string>> ReadFromFile(string fileFullPath)
        {
            var result = new List<string>();

            try
            {
                var contents = await File.ReadAllTextAsync(fileFullPath);

                result = contents.SplitByDividerExcludingEmptyLines();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexpected error reading from file. File path: {filePath}.", fileFullPath);
            }

            return result;
        }
    }
}
