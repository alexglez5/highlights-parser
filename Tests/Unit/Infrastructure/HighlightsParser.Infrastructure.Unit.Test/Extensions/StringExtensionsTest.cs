using FluentAssertions;
using HighlightsParser.Infrastructure.Extensions;
using NUnit.Framework;

namespace HighlightsParser.Infrastructure.Unit.Test.Extensions
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void SplitByLineExcludingEmptyLinesTest()
        {
            // Arrange
            var sut = "line 1" + Environment.NewLine
                               + Environment.NewLine
                               + "line 2" + Environment.NewLine
                               + "line 3";

            // Act
            var result = sut.SplitByLineExcludingEmptyLines();

            // Assert
            result.Count.Should().Be(3);
            result[0].Should().Be("line 1");
            result[1].Should().Be("line 2");
            result[2].Should().Be("line 3");
        }

        [TestCase("ff")]
        [TestCase("f>f")]
        [TestCase("f<f")]
        [TestCase("f:f")]
        [TestCase("f:f")]
        [TestCase("f?f")]
        [TestCase("f/f")]
        [TestCase("f\\f")]
        public void RemoveInvalidCharactersForFileNameTest(string sut)
        {
            // Act
            var result = sut.RemoveInvalidCharactersForFileName();

            // Assert
            result.Should().Be("ff");
        }
    }
}
