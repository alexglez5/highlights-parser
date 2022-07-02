using HighlightsParser.Infrastructure.Logging;

namespace HighlightsParser.Infrastructure.Services
{
    public interface IFileWriterService
    {
        Task WriteToFile(string fileFullPath, string content);
    }

    public class FileWriterService : IFileWriterService
    {
        private readonly IAppLogger _logger;

        public FileWriterService(IAppLogger logger)
        {
            _logger = logger;
        }

        public async Task WriteToFile(string fileFullPath, string content)
        {
            try
            {
                await using var streamWriter = new StreamWriter(fileFullPath, append: false);
                await streamWriter.WriteAsync(content);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Unexpected error writing to file: {fileFullPath}.", fileFullPath);
            }
        }
    }
}
