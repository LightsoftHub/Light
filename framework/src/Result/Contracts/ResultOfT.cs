namespace Light.Contracts
{
    public class Result<T> : Result, IResult<T>
    {
        public Result()
        {
        }

        protected internal Result(T data, string message)
        {
            if (data == null)
            {
                Code = ResultCode.unknown.ToString();
            }
            else
            {
                Code = ResultCode.success.ToString();
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

        public T Data { get; set; }

        public static Result<T> Success(T data, string message = "") =>
            new Result<T>(data, message);

        public static new Result<T> Forbidden(string message = "") =>
            new Result<T>(ResultCode.forbidden, message);

        public static new Result<T> Unauthorized(string message = "") =>
            new Result<T>(ResultCode.unauthorized, message);

        public static new Result<T> NotFound(string message = "") =>
            new Result<T>(ResultCode.not_found, message);

        public static new Result<T> NotFound(string objectName, object queryValue) =>
            new Result<T>(ResultCode.not_found, $"Query object {objectName} by {queryValue} not found");

        public static new Result<T> NotFound<TObject>(object queryValue) =>
            NotFound(typeof(TObject).Name, queryValue);

        public static new Result<T> Error(string message = "") =>
            new Result<T>(ResultCode.error, message);
    }
}