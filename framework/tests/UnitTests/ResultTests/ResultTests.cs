namespace UnitTests.ResultTests
{
    public class ResultTests
    {
        [Fact]
        public void Should_True_When_Success()
        {
            var success = Result.Success();
            var error = Result.Error();

            success.Succeeded.Should().BeTrue();
            error.Succeeded.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_Correct_Code()
        {
            var success = Result.Success();
            var error = Result.Error();
            var badRequest = Result.BadRequest();
            var unauthorized = Result.Unauthorized();
            var notFound = Result.NotFound();

            success.Code.Should().Be(ResultCode.Ok);
            error.Code.Should().Be(ResultCode.Error);
            badRequest.Code.Should().Be(ResultCode.BadRequest);
            unauthorized.Code.Should().Be(ResultCode.Unauthorized);
            notFound.Code.Should().Be(ResultCode.NotFound);
        }

        [Theory]
        [InlineData(ResultCode.Ok, "Success message")]
        [InlineData(ResultCode.Error, "Error message")]
        [InlineData(ResultCode.BadRequest, "BadRequest message")]
        [InlineData(ResultCode.Unauthorized, "Unauthorized message")]
        [InlineData(ResultCode.NotFound, "NotFound message")]
        public void Should_Return_Correct_Result(ResultCode code, string message)
        {
            var result = new Result
            {
                Code = code,
                Message = message,
            };

            result.Code.Should().Be(code);
            result.Succeeded.Should().Be(code == ResultCode.Ok);
            result.Message.Should().Be(message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Correct_Data(int id)
        {
            var intId = Result<int>.Success(id);
            var stringId = Result<string>.Success($"ID-{id}");

            intId.Data.Should().Be(id);
            stringId.Data.Should().Be($"ID-{id}");
        }
    }
}