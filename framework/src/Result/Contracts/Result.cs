namespace Light.Contracts
{
    public class Result : ResultBase
    {
        public Result() { }

        protected internal Result(ResultCode code)
        {
            Code = code.ToString();
            Succeeded = code == ResultCode.success;
        }

        protected internal Result(ResultCode code, string message) : this(code)
        {
            Message = message;
        }

        public static Result Success(string message = "") =>
            new Result(ResultCode.success, message);

        public static Result Forbidden(string message = "") =>
            new Result(ResultCode.forbidden, message);

        public static Result Unauthorized(string message = "") =>
            new Result(ResultCode.unauthorized, message);

        public static Result NotFound(string message = "") =>
            new Result(ResultCode.not_found, message);

        public static Result NotFound(string objectName, object queryValue) =>
            new Result(ResultCode.not_found, $"Query object {objectName} by {queryValue} not found");

        public static Result NotFound<TObject>(object queryValue) =>
            NotFound(typeof(TObject).Name, queryValue);

        public static Result Conflict(string message = "") =>
            new Result(ResultCode.conflict, message);

        public static Result Error(string message = "") =>
            new Result(ResultCode.error, message);
    }
}