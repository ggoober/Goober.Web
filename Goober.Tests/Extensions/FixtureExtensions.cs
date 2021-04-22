using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace Goober.Tests.Extensions
{
    public static class FixtureExtensions
    {
        public static IFixture AutoMock(this IFixture fixture)
        {
            return fixture.Customize(new AutoNSubstituteCustomization());
        }
    }
}
