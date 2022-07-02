using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using HighlightsParser.Infrastructure.Logging;
using HighlightsParser.Infrastructure.Services;
using HighlightsParser.TestHelpers.AutoFixture;
using NUnit.Framework;

namespace HighlightsParser.Infrastructure.Integration.Test.Services.FileReader
{
    [TestFixture]
    public class FileReaderServiceTest
    {
        private IFixture _fixture;
        private IAppLogger _logger;

        private IFileReaderService _sut;

        private const string SourceFileFullPath = "C:\\Projects\\text-parser\\Tests\\Integration\\Infrastructure\\HighlightsParser.Infrastructure.Integration.Test\\Services\\FileReader\\Resources\\clippints.txt";

        [SetUp]
        public void Setup()
        {
            _fixture = new AutoFakeItEasyFixture();
            _logger = _fixture.Freeze<IAppLogger>();

            _sut = _fixture.Create<FileReaderService>();
        }

        [Test]
        public async Task ReadFromFile_ShouldReturnStringListExcludingEmptyLines()
        {
            // Act
            var result = await _sut.ReadFromFile(SourceFileFullPath);

            // Assert
            var tittle = "The Decline and Fall of Practically Everybody (Nonpareil Books) (Will Cuppy)";
            var dividingLine = "==========";
            result[0].Should().Be(tittle);
            result[1].Should().Be("- Your Highlight on Location 528-530 | Added on Sunday, June 26, 2022 9:05:56 AM");
            result[2].Should()
                .Be(
                    "There was also a man named Socrates, who went around barefoot asking people to define their terms. He taught that the good life consists in being good and that virtue is knowledge and knowledge is virtue.");
            result[3].Should().Be(dividingLine);
            result[4].Should().Be(tittle);
            result.Last().Should().Be(dividingLine);

            result.Count.Should().Be(12);
        }

        [Test]
        public async Task ReadFromFile_WhenException_ShouldLogErrorAndReturnEmptyList()
        {
            // Act
            var result = await _sut.ReadFromFile(string.Empty);

            // Assert
            A.CallTo(() => _logger.Error(A<Exception>._, A<string>._, A<object[]>._))
                .MustHaveHappenedOnceExactly();
            result.Any().Should().BeFalse();
        }
    }
}