using Light.Extensions;

namespace UnitTests.ExtensionsTests
{
    public class StringTests
    {
        [Fact]
        public void Should_Get_Correct_Characters()
        {
            var str = "A1B2C_3D4E5";

            var leftToUnderscore = str.Left("_");
            leftToUnderscore.Should().Be("A1B2C");

            var left2Chars = str.Left(2);
            left2Chars.Should().Be("A1");

            var right4Chars = str.Right(4);
            right4Chars.Should().Be("D4E5");
        }
    }
}