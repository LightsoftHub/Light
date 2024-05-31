using Microsoft.AspNetCore.Mvc;

namespace Sample.Extensions
{
    public static class ApiResultExtensions
    {
        public static ObjectResult ToResponse(this Light.Contracts.IResult result)
        {
            if (result.Succeeded)
            {
                return new ObjectResult(default)
                {
                    StatusCode = (int)result.Code,
                    Value = result.Message
                };
            }

            return new ObjectResult(default)
            {
                StatusCode = (int)result.Code,
                Value = result
            };
        }

        public static ObjectResult ToResponse<T>(this Light.Contracts.IResult<T> result)
        {
            if (result.Succeeded)
            {
                return new ObjectResult(default)
                {
                    StatusCode = (int)result.Code,
                    Value = !string.IsNullOrEmpty(result.Message) ? result : result.Data
                };
            }

            return new ObjectResult(default)
            {
                StatusCode = (int)result.Code,
                Value = result
            };
        }
    }
}
