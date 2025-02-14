using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Light.AspNetCore.Hosting.Middlewares;

public static class Startup
{
    public const string RequestLoggingSectionName = "RequestLogging";

    public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        var settings = configuration.GetSection(RequestLoggingSectionName).Get<RequestLoggingOptions>();
        if (settings is not null && settings.Enable)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
        }

        return app;
    }
}
