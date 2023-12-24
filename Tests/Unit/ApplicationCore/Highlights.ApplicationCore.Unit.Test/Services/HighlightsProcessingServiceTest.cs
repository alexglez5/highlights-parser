using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using HighlightsParser.ApplicationCore.Models;
using HighlightsParser.ApplicationCore.Services;
using HighlightsParser.Infrastructure.Services;
using HighlightsParser.TestHelpers.AutoFixture;
using NUnit.Framework;

namespace Highlights.ApplicationCore.Unit.Test.Services
{
    [TestFixture]
    public class HighlightsProcessingServiceTest
    {
        private IFixture _fixture;
        private IFileReaderService _fileReaderService;
        private IFileWriterService _fileWriterService;
        private IHighlightsParsingService _highlightsParsingService;
        private ITimeProvider _timeProvider;

        private IHighlightsProcessingService _sut;

        private const string InputFileFullPath = "C:\\SomePath\\input.txt";
        private const string OutputFolderFullPath = "C:\\SomeOtherPath\\";

        [SetUp]
        public void SetUp()
        {
            _fixture = new AutoFakeItEasyFixture();
            _fileReaderService = _fixture.Freeze<IFileReaderService>();
            _fileWriterService = _fixture.Freeze<IFileWriterService>();
            _highlightsParsingService = _fixture.Freeze<IHighlightsParsingService>();
            _timeProvider = _fixture.Freeze<ITimeProvider>();

            _sut = _fixture.Create<HighlightsProcessingService>();
        }

        [Test]
        public async Task ParseHighlightsFromInputFileToOutputFiles_HappyPath()
        {
            // Arrange
            var fileContent = _fixture.CreateMany<string>().ToList();
            A.CallTo(() => _fileReaderService.ReadFromFile(InputFileFullPath))
                .Returns(fileContent);

            var parsingResults = new List<ParsingResult>
            {
                new ParsingResult
                {
                    Title = "tittle 1",
                    Highlights = new List<string> { "highlight 1", "highlight 2" }
                },
                new ParsingResult
                {
                    Title = "tittle 2",
                    Highlights = new List<string> { "highlight 3", "highlight 4" }
                },
            };
            A.CallTo(() => _highlightsParsingService.Parse(fileContent))
                .Returns(parsingResults);

            var date = DateTime.Parse("2020-10-15");
            A.CallTo(() => _timeProvider.GetCurrentDate())
                .Returns(date);

            var capturedOutputFileFullPaths = new List<string>();
            var capturedContents = new List<string>();
            A.CallTo(() => _fileWriterService.WriteToFile(A<string>._, A<string>._))
                .Invokes(objCall =>
                {
                    capturedOutputFileFullPaths.Add(objCall.Arguments[0].ToString()!);
                    capturedContents.Add(objCall.Arguments[1].ToString()!);
                });

            // Act
            await _sut.ParseHighlightsFromInputFileToOutputFiles(InputFileFullPath, OutputFolderFullPath);

            // Assert
            capturedOutputFileFullPaths.Count.Should().Be(2);
            capturedOutputFileFullPaths.Any(c => c.Equals("C:\\SomeOtherPath\\2020-10-15 tittle 1.md")).Should().BeTrue();
            capturedOutputFileFullPaths.Any(c => c.Equals("C:\\SomeOtherPath\\2020-10-15 tittle 2.md")).Should().BeTrue();

            capturedContents.Count.Should().Be(2);
            capturedContents.Any(c => c.Equals("# tittle 1\r\n\r\n- highlight 1\r\n- highlight 2")).Should().BeTrue();
            capturedContents.Any(c => c.Equals("# tittle 2\r\n\r\n- highlight 3\r\n- highlight 4")).Should().BeTrue();
        }
    }
}
