using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TextParser.Models;
using TextParser.Services;

namespace TextParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddScoped<IKindleHighlightsParsingService, KindleHighlightsParsingService>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            logger.LogInformation("Highlights Parsing starting...log"); // todo: verify logger output location
            Console.WriteLine("Highlights parsing starting...");
            
            var kindleHighlightsParsingService = serviceProvider.GetService<IKindleHighlightsParsingService>();

            var result = await kindleHighlightsParsingService.ParseKindleHighlights(new KindleHighlightsParsingCommand
            {
                InputFilePath = args[0],
                InputFileName = args[1],
                OutputFilePath = args[2],
                OutputFileName = args[3]
            });

            Console.WriteLine(result ? "Highlights Parsing successfully completed." : "Highlights parsing failed.");
        }
    }
}
