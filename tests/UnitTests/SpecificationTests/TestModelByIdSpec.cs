using Light.Specification;

namespace UnitTests.SpecificationTests
{
    public class TestModelByIdSpec : Specification<TestModel>
    {
        public TestModelByIdSpec(int id)
        {
            Where(x => x.Id == id);
        }
    }
}
