using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TextParser.Models;
using TextParser.Services;
using Serilog;
using ILogger = Serilog.ILogger;

namespace TextParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = (ILogger)new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("C:\\applogs\\TextParserLogFile.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(logger)
                .AddScoped<IKindleHighlightsParsingService, KindleHighlightsParsingService>()
                .BuildServiceProvider();

            logger.Debug("Highlights parsing starting...");

            var kindleHighlightsParsingService = serviceProvider.GetService<IKindleHighlightsParsingService>();

            var result = await kindleHighlightsParsingService.ParseKindleHighlights(new KindleHighlightsParsingCommand
            {
                InputFilePath = args[0],
                InputFileName = args[1],
                OutputFilePath = args[2],
                OutputFileName = args[3]
            });

            var message = result ? "Highlights Parsing successfully completed." : "Highlights parsing failed.";
            logger.Debug(message);
        }
    }
}
