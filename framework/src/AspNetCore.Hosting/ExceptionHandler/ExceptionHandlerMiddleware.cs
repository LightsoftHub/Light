using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Light.AspNetCore.Hosting.ExceptionHandler;

// must use Microsoft Logger because only Singleton services can be resolved by constructor injection in Middleware
public class ExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await context.HandleExceptionAsync(ex, logger);
        }
    }
}
