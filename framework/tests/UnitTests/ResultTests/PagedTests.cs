namespace UnitTests.ResultTests
{
    public class PagedTests
    {
        private readonly int totalRecords = 10;
        private readonly int pageSize = 5;

        private readonly IEnumerable<int> listValues;

        public PagedTests()
        {
            var list = new List<int>();

            for (int i = 1; i <= totalRecords; i++)
            {
                list.Add(i);
            }

            listValues = list;
        }

        [Fact]
        public void Should_Return_Correct_PageInfo()
        {
            var pagedResult = listValues.ToPagedResult(1, pageSize);

            var pagedInfo = pagedResult.PagedInfo;

            pagedInfo.PageSize.Should().Be(pageSize);
            pagedInfo.TotalRecords.Should().Be(totalRecords);
        }

        [Fact]
        public void Should_Return_Correct_PageData()
        {
            var pagedResult = listValues.ToPagedResult(1, pageSize);

            var recordsPerPage = pagedResult.Data.Count();

            recordsPerPage.Should().Be(pageSize);
        }
    }
}