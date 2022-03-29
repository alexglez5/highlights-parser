using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TextParser.Models;

namespace TextParser.Services
{
    public interface IKindleHighlightsParsingService
    {
        Task<bool> ParseKindleHighlights(KindleHighlightsParsingCommand parsingCommand);
    }

    public class KindleHighlightsParsingService : IKindleHighlightsParsingService
    {
        public async Task<bool> ParseKindleHighlights(KindleHighlightsParsingCommand parsingCommand)
        {
            try
            {
                await this.Parse(parsingCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error occurred. Exception: " + e.Message);
                return false;
            }

            return true;
        }

        private async Task Parse(KindleHighlightsParsingCommand parsingCommand)
        {
            var inputFilePath = $"{parsingCommand.InputFilePath}{parsingCommand.InputFileName}";
            using var streamReader = new StreamReader(inputFilePath);
            
            var outputFilePath = $"{parsingCommand.OutputFilePath}{parsingCommand.OutputFileName}";
            await using var streamWriter = new StreamWriter(@outputFilePath);

            var line = await streamReader.ReadLineAsync(); // title

            await streamWriter.WriteLineAsync($"# {line}");
            await streamWriter.WriteLineAsync();

            line = await WriteHighlightLinesWithLocation(streamReader, streamWriter);

            while (!string.IsNullOrEmpty(line))
            {
                await streamReader.ReadLineAsync(); // skip title

                line = await WriteHighlightLinesWithLocation(streamReader, streamWriter);
            }
        }

        private async Task<string> WriteHighlightLinesWithLocation(StreamReader streamReader, StreamWriter streamWriter)
        {
            var highlightLocation = await ResolveLocation(streamReader);

            await streamReader.ReadLineAsync(); // skip blank line

            var highlightLine = await streamReader.ReadLineAsync();

            while (!highlightLine.IsEndOfHighlight())
            {
                await streamWriter.WriteLineAsync($"- {highlightLine}{highlightLocation}");
                highlightLine = await streamReader.ReadLineAsync();
            }

            return highlightLine;
        }

        private static async Task<string> ResolveLocation(StreamReader streamReader)
        {
            var locationLine = await streamReader.ReadLineAsync();
            if (locationLine == null)
            {
                return string.Empty;
            }

            var words = locationLine.Split(' ').ToList();

            var counter = 0;
            var word = words[0];
            while (word != "Location" && counter < words.Count)
            {
                word = words[counter];
                counter++;
            }

            var locationFirstNumber = words[counter].Split('-').First();
            return $" @ Location {locationFirstNumber}";
        }

    }
    
    internal static class StringExtensions
    {
        public static bool IsEndOfHighlight(this string str)
        {
            return string.IsNullOrEmpty(str) || str.StartsWith("====");
        }
    }
}
