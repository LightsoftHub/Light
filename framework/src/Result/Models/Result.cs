using Light.Contracts;
using System.Collections.Generic;

namespace Light.Models
{
    public class Result : IResult
    {
        public Result() { }

        protected internal Result(string message)
        {
            Code = ResultCode.Unknown;
            Message = message;
        }

        protected internal Result(ResultCode code)
        {
            Code = code;
        }

        protected internal Result(ResultCode code, string message)
        {
            Code = code;
            Message = message;
        }

        protected internal Result(ResultCode code, IEnumerable<string> errors)
        {
            Code = code;

            if (errors != null)
            {
                Errors = errors;
            }
        }

        public ResultCode Code { get; set; }

        public bool Succeeded => Code == ResultCode.Ok;

        public bool IsFailed => !Succeeded;

        public string Message { get; set; } = "";

        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public static Result Success(string message = "") => new Result(ResultCode.Ok, message);

        public static Result BadRequest(params string[] errors) =>
            new Result(ResultCode.BadRequest, errors);

        public static Result Forbidden(params string[] errors) =>
            new Result(ResultCode.Forbidden, errors);

        public static Result Unauthorized(params string[] errors) =>
            new Result(ResultCode.Unauthorized, errors);

        public static Result NotFound(params string[] errors) =>
            new Result(ResultCode.NotFound, errors);

        public static Result NotFound(string objectName, object queryValue)
        {
            var message = $"Query object {objectName} by {queryValue} not found";

            return new Result(ResultCode.NotFound, new List<string> { message });
        }

        public static Result NotFound<TObject>(object queryValue)
        {
            var message = $"Query object {typeof(TObject).Name} by {queryValue} not found";

            return new Result(ResultCode.NotFound, new List<string> { message });
        }

        public static Result Conflict(params string[] errors) =>
            new Result(ResultCode.Conflict, errors);

        public static Result Error(params string[] errors) =>
            new Result(ResultCode.Error, errors);

        public static Result Error(IEnumerable<string> errors) =>
            new Result(ResultCode.Error, errors);
    }
}