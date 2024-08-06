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
        public void Should_Map_Correct_ResultCode()
        {
            var success = Result.Success();
            var error = Result.Error();
            var unauthorized = Result.Unauthorized();
            var notFound = Result.NotFound();
            var unknown = new Result { Code = "OtherCode" };

            success.MapResultCode().Should().Be(ResultCode.Ok);
            error.MapResultCode().Should().Be(ResultCode.Error);
            unauthorized.MapResultCode().Should().Be(ResultCode.Unauthorized);
            notFound.MapResultCode().Should().Be(ResultCode.NotFound);
            unknown.MapResultCode().Should().Be(ResultCode.Unknown);
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
                Code = code.ToString(),
                Message = message,
            };

            var mappedResultCode = result.MapResultCode();

            mappedResultCode.Should().Be(code);

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