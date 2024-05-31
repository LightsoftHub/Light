using Light.Contracts;
using System;
using System.Collections.Generic;

namespace Light.Models
{
    public class Result<T> : IResult<T>
    {
        public Result()
        {
        }

        protected internal Result(T data, string message)
        {
            if (data == null)
            {
                Code = ResultCode.NotFound;
                Message = string.IsNullOrEmpty(message) ? $"Null value object {typeof(T).Name}" : message;
            }
            else
            {
                Code = ResultCode.Ok;
                Message = message;
                Data = data;
            }
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

        public bool Succeeded => Code == ResultCode.Ok && Data != null;

        public string Message { get; set; } = "";

        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

        public T Data { get; set; }

        public static Result<T> Success(T data, string message = "") =>
            new Result<T>(data, message);

        public static Result<T> NotFound(string objectName, object queryValue)
        {
            var message = $"Query object {objectName} by {queryValue} not found";

            return new Result<T>(ResultCode.NotFound, new List<string> { message });
        }

        public static Result<T> NotFound<TObject>(object queryValue)
        {
            var message = $"Query object {typeof(TObject).Name} by {queryValue} not found";

            return new Result<T>(ResultCode.NotFound, new List<string> { message });
        }

        public static Result<T> BadRequest(params string[] errors) =>
            new Result<T>(ResultCode.BadRequest, errors);

        public static Result<T> Error(params string[] errors) =>
            new Result<T>(ResultCode.Error, errors);

        public static Result<T> Error(IEnumerable<string> errors) =>
            new Result<T>(ResultCode.Error, errors);
    }
}