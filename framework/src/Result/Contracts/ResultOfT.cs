using System;

namespace Light.Contracts
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
                Code = ResultCode.Unknown.ToString();
            }
            else
            {
                Code = ResultCode.Ok.ToString();
                Succeeded = true;
                Message = message;
                Data = data;
            }
        }

        protected internal Result(ResultCode code, string message)
        {
            Code = code.ToString();
            Message = message;
        }

        public static implicit operator T(Result<T> result) => result.Data;

        public string Code { get; set; }

        public bool Succeeded { get; set; }

        public string Message { get; set; } = "";

        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public T Data { get; set; }

        public static Result<T> Success(T data, string message = "") =>
            new Result<T>(data, message);

        public static Result<T> Forbidden(string message = "") =>
            new Result<T>(ResultCode.Forbidden, message);

        public static Result<T> Unauthorized(string message = "") =>
            new Result<T>(ResultCode.Unauthorized, message);

        public static Result<T> NotFound(string objectName, object queryValue)
        {
            var message = $"Query object {objectName} by {queryValue} not found";
            return new Result<T>(ResultCode.NotFound, message);
        }

        public static Result<T> NotFound<TObject>(object queryValue) =>
            NotFound(typeof(TObject).Name, queryValue);

        public static Result<T> Error(string message = "") =>
            new Result<T>(ResultCode.Error, message);
    }
}