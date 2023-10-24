using HighlightsParser.ApplicationCore.Services;
using HighlightsParser.Infrastructure.Logging;
using HighlightsParser.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace HighlightsParser.ConsoleApp
{
    public class Program
    {
        public static async Task Main()
        {
            var logger = (ILogger)new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            IAppLogger appLogger = new AppLogger(logger);

            var serviceProvider = new ServiceCollection()
                .AddSingleton(appLogger)
                .AddScoped<IFileReaderService, FileReaderService>()
                .AddScoped<IFileWriterService, FileWriterService>()
                .AddScoped<IHighlightsParsingService, HighlightsParsingService>()
                .AddScoped<ITimeProvider, TimeProvider>()
                .AddScoped<IHighlightsProcessingService, HighlightsProcessingService>()
                .BuildServiceProvider();

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            
            var inputFileFullPath = configuration.GetSection("InputFileFullPath").Value;
            var outputFolderFullPath = configuration.GetSection("OutputFolderFullPath").Value;

            var highlightsProcessingService = serviceProvider.GetService<IHighlightsProcessingService>();

            appLogger.Information("Starting highlights parsing...");

            await highlightsProcessingService.ParseHighlightsFromInputFileToOutputFiles(
                inputFileFullPath,
                outputFolderFullPath);

            appLogger.Information("Highlights parsing completed.");
        }
    }
}