using Light.Specification;

namespace UnitTests.SpecificationTests
{
    public class SpecificationTests
    {
        private readonly List<TestModel> _models;

        public SpecificationTests()
        {
            var models = new List<TestModel>();

            for (var i = 1; i <= 10; i++)
            {
                models.Add(new TestModel
                {
                    Id = i,
                    Name = $"Name Of {i}"
                });
            }

            _models = models;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Should_Find_Correct_Data(int id)
        {
            var spec = new TestModelByIdSpec(id);

            var single = _models.Where(spec).Single();

            single.Id.ShouldBe(id);
            single.Name.ShouldBe($"Name Of {id}");
        }
    }
}