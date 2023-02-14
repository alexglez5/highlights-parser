using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using HighlightsParser.Infrastructure.Logging;
using HighlightsParser.Infrastructure.Services;
using HighlightsParser.TestHelpers.AutoFixture;
using NUnit.Framework;

namespace HighlightsParser.Infrastructure.Integration.Test.Services.FileWriter
{
    [TestFixture]
    public class FileWriterServiceTest
    {
        private IFixture _fixture;
        private IAppLogger _logger;

        private IFileWriterService _sut;

        private string _content;

        private const string OutputFileFullPath = "C:\\Dev\\highlights-parser\\Tests\\Integration\\Infrastructure\\HighlightsParser.Infrastructure.Integration.Test\\Services\\FileWriter\\Resources\\output file.md";

        [SetUp]
        public void SetUp()
        {
            _fixture = new AutoFakeItEasyFixture();
            _logger = _fixture.Freeze<IAppLogger>();

            _sut = _fixture.Create<FileWriterService>();

            _content = _fixture.Create<string>();
        }

        [Test]
        public async Task WriteToFile_WhenExceptionOccurs_ShouldLogError()
        {
            // Act
            await _sut.WriteToFile(string.Empty, _content);

            // Assert
            A.CallTo(() => _logger.Error(A<Exception>._, A<string>._, A <object[]>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task WriteToFile_ShouldWriteContentToFile()
        {
            // Act
            await _sut.WriteToFile(OutputFileFullPath, _content);

            // Assert
            var savedContent = await File.ReadAllTextAsync(OutputFileFullPath);
            savedContent.Should().Be(_content);
        }
    }
}
