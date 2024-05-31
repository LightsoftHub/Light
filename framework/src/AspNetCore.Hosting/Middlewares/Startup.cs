using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Hosting.Middlewares;

public static class Startup
{
    public const string RequestLoggingSectionName = "RequestLogging";

    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        var settings = configuration.GetSection(RequestLoggingSectionName).Get<RequestLoggingOptions>();
        if (settings is not null && settings.Enable)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
        }

        return app;
    }

    [Obsolete("please use AddGlobalExceptionHandler() instead")]
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        return app;
    }
}
