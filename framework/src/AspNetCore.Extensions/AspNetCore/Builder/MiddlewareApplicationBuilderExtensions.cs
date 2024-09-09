using Light.AspNetCore.ExceptionHandlers;
using Light.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Light.AspNetCore.Builder;

public static class MiddlewareApplicationBuilderExtensions
{
    internal const string RequestLoggingSectionName = "RequestLogging";

    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, IConfiguration configuration)
    {
        var settings = configuration.GetSection(RequestLoggingSectionName).Get<RequestLoggingOptions>();
        if (settings is not null && settings.Enable)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
        }

        return app;
    }

    public static IApplicationBuilder UseGuidTraceId(this IApplicationBuilder app)
    {
        app.UseMiddleware<GuidTraceIdMiddleware>();

        return app;
    }

    //[Obsolete("please use AddGlobalExceptionHandler() instead")]
    public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        return app;
    }
}
