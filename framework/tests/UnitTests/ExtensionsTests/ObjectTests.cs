namespace UnitTests.ExtensionsTests
{
    public class ObjectTests
    {
        [Fact]
        public void Should_Check_ExactlyType()
        {
            var list = new List<object>();
            var dictionary = new Dictionary<string, object>();

            list.IsList().Should().BeTrue();
            dictionary.IsList().Should().BeFalse();

            list.IsDictionary().Should().BeFalse();
            dictionary.IsDictionary().Should().BeTrue();
        }
    }
}