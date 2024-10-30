using System;

namespace Light.Contracts
{
    public class Result : IResult
    {
        public Result() { }

        protected internal Result(ResultCode code)
        {
            Code = code.ToString();
            Succeeded = code == ResultCode.Ok;
        }

        protected internal Result(ResultCode code, string message) : this(code)
        {
            Message = message;
        }

        public string Code { get; set; }

        public bool Succeeded { get; set; }

        public string Message { get; set; } = "";

        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public static Result Success(string message = "") =>
            new Result(ResultCode.Ok, message);

        public static Result Forbidden(string message = "") =>
            new Result(ResultCode.Forbidden, message);

        public static Result Unauthorized(string message = "") =>
            new Result(ResultCode.Unauthorized, message);

        public static Result NotFound(string message = "") =>
            new Result(ResultCode.NotFound, message);

        public static Result NotFound(string objectName, object queryValue) =>
            new Result(ResultCode.NotFound, $"Query object {objectName} by {queryValue} not found");

        public static Result NotFound<TObject>(object queryValue) =>
            NotFound(typeof(TObject).Name, queryValue);

        public static Result Conflict(string message = "") =>
            new Result(ResultCode.Conflict, message);

        public static Result Error(string message = "") =>
            new Result(ResultCode.Error, message);
    }
}