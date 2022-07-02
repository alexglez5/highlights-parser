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

        [TestCase(2)]
        [TestCase(5)]
        [TestCase(13)]
        public void Parse_WhenNumberOfLinesNotAMultipleOf4_ShouldLogErrorAndReturnEmptyResult(int lineCount)
        {
            // Assert
            var lines = GetRandomList(lineCount);
            
            // Act
            var result = _sut.Parse(lines);

            // Assert
            result.Any().Should().BeFalse();

            A.CallTo(() => _logger.Error(A<string>._, A<object[]>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void Parse_WhenNumberOfLinesMultipleOf4_ShouldReturnGroupsOfContentByHeader()
        {
            // Assert
            var lines = new List<string>
            {
                "header 1",
                "location 1",
                "content 1",
                "=====",
                "header 1",
                "location 2",
                "content 2",
                "=====",
                "header 2",
                "location 3",
                "content 3",
                "====="
            };

            // Act
            var result = _sut.Parse(lines);

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
