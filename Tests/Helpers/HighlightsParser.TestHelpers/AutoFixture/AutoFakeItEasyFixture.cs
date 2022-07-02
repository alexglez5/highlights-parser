using AutoFixture;
using AutoFixture.AutoFakeItEasy;

namespace HighlightsParser.TestHelpers.AutoFixture
{
    public class AutoFakeItEasyFixture : Fixture
    {
        public AutoFakeItEasyFixture()
        {
            this.Customize(new AutoFakeItEasyCustomization());
        }
    }
}