using Light.AspNetCore.ExceptionHandlers;
using Microsoft.AspNetCore.Builder;

namespace Light.AspNetCore.Builder;

public static class ExceptionHandlerBuilderExtensions
{
    //[Obsolete("please use AddGlobalExceptionHandler() instead")]
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        return app;
    }
}
