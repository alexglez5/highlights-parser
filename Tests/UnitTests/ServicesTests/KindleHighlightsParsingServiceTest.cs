using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TextParser.Models;
using TextParser.Services;

namespace UnitTests.ServicesTests
{
    [TestFixture]
    public class KindleHighlightsParsingServiceTest
    {
        private IKindleHighlightsParsingService sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new KindleHighlightsParsingService();
        }

        [Test]
        public async Task ParseKindleHighlightsTest()
        {
            // Arrange
            var parsingCommand = new KindleHighlightsParsingCommand
            {
                InputFilePath = "C:\\Projects\\TextParser\\Tests\\UnitTests\\ServicesTests\\",
                InputFileName = "sampleKindleHighlights.txt",
                OutputFilePath = "C:\\Projects\\TextParser\\Tests\\UnitTests\\ServicesTests\\",
                OutputFileName = "expectedOutput.md"
            };

            // Act
            await this.sut.ParseKindleHighlights(parsingCommand);

            // Assert
            var path = $"{parsingCommand.OutputFilePath}{parsingCommand.OutputFileName}";
            using var streamReader = new StreamReader(path);
            var line = await streamReader.ReadLineAsync();
            line.Should().Be("# Awareness: The Perils and Opportunities of Reality (Anthony de Mello)");
            line = await streamReader.ReadLineAsync();
            line.Should().Be(string.Empty);
            line = await streamReader.ReadLineAsync();
            line.Should().Be("- You are never in love with anyone, you’re in love with your prejudiced idea of that person. @ Location 138");
            line = await streamReader.ReadLineAsync();
            line.Should().Be("- Isn’t that how you fall out of love? Your idea changes, doesn’t it? “How could you let me down when I trusted you so much?” you say to someone. Did you really trust them? You never trusted anyone. Come off it! That’s part of society’s brainwashing. You never trust anyone. You only trust your judgment about that person. @ Location 138");
            line = await streamReader.ReadLineAsync();
            line.Should().Be("- The fact is that you don’t like to say, “My judgment was lousy.” That’s not very flattering to you, is it? So you prefer to say, “How could you have let me down?” @ Location 141");
            line = await streamReader.ReadLineAsync();
            line.Should().Be("- People don’t really want to grow up, people don’t really want to change, people don’t really want to be happy. As someone so wisely said to me, “Don’t try to make them happy, you’ll only get in trouble. Don’t try to teach a pig to sing; it wastes your time and it irritates the pig.” @ Location 143");
            line = await streamReader.ReadLineAsync();
            line.Should().Be("- “I’d rather be happy than have you. If I had a choice, no question about it, I’d choose happiness.” How many of you felt selfish when you said this? Many, it seems. See how we’ve been brainwashed? See how we’ve been brainwashed into thinking, “How could I be so selfish?” But look at who’s being selfish. Imagine somebody saying to you, “How could you be so selfish that you’d choose happiness over me?” Would you not feel like responding, “Pardon me, but how could you be so selfish that you would demand I choose you above my own happiness?!” @ Location 158");
            line = await streamReader.ReadLineAsync();
            line.Should().Be("- ON THE PROPER KIND OF SELFISHNESS @ Location 153");
            line = await streamReader.ReadLineAsync();
            line.Should().Be("- ON WANTING HAPPINESS @ Location 171");
            line = await streamReader.ReadLineAsync();
            line.Should().Be("- spirituality is the most practical thing in the whole wide world. @ Location 179");
        } 
    }
}
