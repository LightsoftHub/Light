using Light.Contracts;
using System.Net;

namespace Light.Extensions
{
    public static class HttpStatusMapper
    {
        public static HttpStatusCode MapHttpStatusCode(this IResult result)
        {
            var code = result.MapResultCode();

            switch (code)
            {
                case ResultCode.Ok:
                    return HttpStatusCode.OK;
                case ResultCode.BadRequest:
                    return HttpStatusCode.BadRequest;
                case ResultCode.Unauthorized:
                    return HttpStatusCode.Unauthorized;
                case ResultCode.Forbidden:
                    return HttpStatusCode.Forbidden;
                case ResultCode.NotFound:
                    return HttpStatusCode.NotFound;
                case ResultCode.Conflict:
                    return HttpStatusCode.Conflict;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}
