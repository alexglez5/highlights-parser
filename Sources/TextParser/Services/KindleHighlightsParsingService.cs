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
            var line = await streamReader.ReadLineAsync(); // title

            var outputFilePath = $"{parsingCommand.OutputFilePath}{parsingCommand.OutputFileName}";
            await using var streamWriter = new StreamWriter(@outputFilePath);
            await streamWriter.WriteLineAsync($"# {line}");
            await streamWriter.WriteLineAsync();

            line = await WriteLineWithLocation(streamReader, streamWriter);

            while (line != string.Empty)
            {
                await streamReader.ReadLineAsync(); // title

                line = await WriteLineWithLocation(streamReader, streamWriter);
            }
        }

        private async Task<string> WriteLineWithLocation(StreamReader streamReader, StreamWriter streamWriter)
        {
            var line = await streamReader.ReadLineAsync(); // location line
            if (line == null)
            {
                return string.Empty;
            }
            var words = line.Split(' ').ToList();

            var counter = 0;
            var word = words[0];
            while (word != "Location" && counter < words.Count)
            {
                word = words[counter];
                counter++;
            }

            var number = words[counter].Split('-').First();
            var location = $" @ Location {number}";

            await streamReader.ReadLineAsync(); // blank line
            line = await streamReader.ReadLineAsync();

            await streamWriter.WriteLineAsync($"- {line}{location}");

            line = await streamReader.ReadLineAsync(); // ==========

            return line;
        }
    }
}
