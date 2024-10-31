using System.Text.Json;

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

            success.MapResultCode().Should().Be(ResultCode.success);
            error.MapResultCode().Should().Be(ResultCode.error);
            unauthorized.MapResultCode().Should().Be(ResultCode.unauthorized);
            notFound.MapResultCode().Should().Be(ResultCode.not_found);
            unknown.MapResultCode().Should().Be(ResultCode.unknown);
        }

        [Theory]
        [InlineData(ResultCode.success, "Success message")]
        [InlineData(ResultCode.error, "Error message")]
        [InlineData(ResultCode.bad_request, "BadRequest message")]
        [InlineData(ResultCode.unauthorized, "Unauthorized message")]
        [InlineData(ResultCode.not_found, "NotFound message")]
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

        [Fact]
        public void Should_Deserialize_Correct_Result()
        {
            var successJson = JsonSerializer.Serialize(Result.Success());
            var errorJson = JsonSerializer.Serialize(Result.Error());

            var success = JsonSerializer.Deserialize<Result>(successJson);
            var error = JsonSerializer.Deserialize<Result>(errorJson);

            success.MapResultCode().Should().Be(ResultCode.success);
            error.MapResultCode().Should().Be(ResultCode.error);
        }
    }
}