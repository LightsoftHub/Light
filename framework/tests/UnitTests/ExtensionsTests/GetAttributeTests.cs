namespace UnitTests.ExtensionsTests
{
    public class GetAttributeTests
    {
        [Fact]
        public void Should_Return_Correct_Attribute()
        {
            var type = typeof(TestObject);

            var displayName = type.GetDisplayName();
            displayName.Should().Be("Test DisplayName");

            var description = type.GetDescription();
            description.Should().Be("Test Description");

            var nameInDisplay = type.GetNameOfDisplay();
            nameInDisplay.Should().Be("Test Name in Display");

            var descInDisplay = type.GetDescriptionOfDisplay();
            descInDisplay.Should().Be("Test Description in Display");

            var nameOfId = ObjectHelper.GetPropertyName<TestObject>(x => x.Id);
            nameOfId.Should().Be("Id");

            var nameOfValue = ObjectHelper.GetPropertyName<TestObject>(x => x.Value);
            nameOfValue.Should().Be("Value");
        }
    }
}