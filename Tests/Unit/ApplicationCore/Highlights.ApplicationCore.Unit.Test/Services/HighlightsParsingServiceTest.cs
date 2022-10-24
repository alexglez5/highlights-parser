using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using HighlightsParser.ApplicationCore.Services;
using HighlightsParser.Infrastructure.Logging;
using HighlightsParser.TestHelpers.AutoFixture;
using NUnit.Framework;

namespace Highlights.ApplicationCore.Unit.Test.Services
{
    [TestFixture]
    public class HighlightsParsingServiceTest
    {
        private IFixture _fixture;
        private IAppLogger _logger;

        private IHighlightsParsingService _sut;

        [SetUp]
        public void SetUp()
        {
            _fixture = new AutoFakeItEasyFixture();
            _logger = _fixture.Freeze<IAppLogger>();

            _sut = _fixture.Create<HighlightsParsingService>();
        }

        [Test]
        public void Parse_WhenLineListIsEmpty_ShouldReturnEmptyResult()
        {
            // Act
            var result = _sut.Parse(new List<string>());

            // Assert
            result.Any().Should().BeFalse();
        }

        [Test]
        public void Parse_WhenNotAllMultiLineHighlightsHave3Lines_ShouldReturnGroupsOfContentByHeaderForThoseWith3Lines()
        {
            // Assert
            var newLine = Environment.NewLine;
            var multiLineHighlights = new List<string>
            {
                $"header 1{newLine}location 1{newLine}{newLine}content 1",
                $"header 1{newLine}location 2{newLine}{newLine}content 2",
                $"header 2{newLine}location 3{newLine}{newLine}content 3",
                $"header 3{newLine}location 4"
            };

            // Act
            var result = _sut.Parse(multiLineHighlights);

            // Assert
            result.Count.Should().Be(2);
            result.Any(r => r.Title == "header 3").Should().BeFalse();

            A.CallTo(() => this._logger.Warning(
                    "Ignoring multi line highlight with less than 3 lines. Content: {multiLineHighlight}",
                    multiLineHighlights.Last()))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void Parse_When3LinesPerMultiLineHighlight_ShouldReturnGroupsOfContentByHeader()
        {
            // Assert
            var newLine = Environment.NewLine;
            var multiLineHighlights = new List<string>
            {
                $"header 1{newLine}location 1{newLine}{newLine}content 1",
                $"header 1{newLine}location 2{newLine}{newLine}content 2",
                $"header 2{newLine}location 3{newLine}{newLine}content 3"
            };

            // Act
            var result = _sut.Parse(multiLineHighlights);

            // Assert
            result.Count.Should().Be(2);
            
            var contentForHeader1 = result.First(r => r.Title == "header 1");
            contentForHeader1.Highlights.Count.Should().Be(2);
            contentForHeader1.Highlights[0].Should().Be("content 1");
            contentForHeader1.Highlights[1].Should().Be("content 2");

            var contentForHeader2 = result.First(r => r.Title == "header 2");
            contentForHeader2.Highlights.Count.Should().Be(1);
            contentForHeader2.Highlights[0].Should().Be("content 3");
        }

        #region Helpers

        private List<string> GetRandomList(int count)
        {
            var result = new List<string>();

            for (var i = 0; i < count; i++)
            {
                result.Add(_fixture.Create<string>());
            }

            return result;
        }

        #endregion
    }
}
