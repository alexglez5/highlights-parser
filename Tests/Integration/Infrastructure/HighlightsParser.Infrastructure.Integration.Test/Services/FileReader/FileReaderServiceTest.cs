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

        private const string SourceFileFullPath = "C:\\Dev\\highlights-parser\\Tests\\Integration\\Infrastructure\\HighlightsParser.Infrastructure.Integration.Test\\Services\\FileReader\\Resources\\clippings.txt";

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
            result.Count.Should().Be(3);
            result[0].Should().Be("The Decline and Fall of Practically Everybody (Nonpareil Books) (Will Cuppy)\r\n- Your Highlight on Location 528-530 | Added on Sunday, June 26, 2022 9:05:56 AM\r\n\r\nThere was also a man named Socrates, who went around barefoot asking people to define their terms. He taught that the good life consists in being good and that virtue is knowledge and knowledge is virtue.\r\n");
            result[1].Should().Be("\r\nThe Decline and Fall of Practically Everybody (Nonpareil Books) (Will Cuppy)\r\n- Your Highlight on Location 617-622 | Added on Sunday, June 26, 2022 9:10:51 AM\r\n");
            result[2].Should().Be("\r\nThe Decline and Fall of Practically Everybody (Nonpareil Books) (Will Cuppy)\r\n- Your Highlight on Location 638-638 | Added on Sunday, June 26, 2022 9:13:01 AM\r\n\r\nAristotle was famous for knowing everything.\r\n");
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