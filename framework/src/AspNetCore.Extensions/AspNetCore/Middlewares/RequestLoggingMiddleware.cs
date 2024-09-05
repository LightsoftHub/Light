using Light.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace Light.AspNetCore.Middlewares;

public class RequestLoggingMiddleware(
    RequestDelegate next,
    IConfiguration configuration,
    // must use Microsoft Logger because only Singleton services can be resolved by constructor injection in Middleware
    ILogger<RequestLoggingMiddleware> logger)
{
    private readonly RequestLoggingOptions _settings = configuration
        .GetSection(MiddlewareBuilderExtensions.RequestLoggingSectionName)
        .Get<RequestLoggingOptions>()
        ?? throw new ArgumentNullException(nameof(RequestLoggingOptions));

    public async Task InvokeAsync(HttpContext context)
    {
        HttpRequest request = context.Request;

        var traceId = context.TraceIdentifier;
        var ip = request.HttpContext.Connection.RemoteIpAddress;
        var clientIp = ip == null ? "UnknownIP" : ip.ToString();

        var requestMethod = request.Method;
        var requestPath = request.Path;
        var requestQuery = request.QueryString.ToString();
        var requestScheme = request.Scheme;
        //var requestHost = request.Host.ToString();

        // default exclude
        var excludePath = new List<string> { "hangfire", "swagger" };
        if (_settings.ExcludePaths is not null)
            excludePath.AddRange(_settings.ExcludePaths);

        var skipWriteLog = excludePath.Any(c => requestPath.ToString().Contains(c));

        if (skipWriteLog)
        {
            // Continue processing the request
            await next(context);
        }
        else
        {
            var timer = new Stopwatch();
            timer.Start();

            var logContent = $"FromIP: {clientIp} TraceID: {traceId}";

            if (_settings.IncludeRequest)
            {
                var requestBody = await ReadBodyAsync(request);

                if (!string.IsNullOrEmpty(requestBody))
                    logContent += $"\r\nRequest: {requestBody}";
            }

            if (_settings.IncludeResponse)
            {
                // Create a new memory stream to capture the response
                var originalBody = context.Response.Body;

                try
                {
                    using var responseBody = new MemoryStream();
                    context.Response.Body = responseBody;

                    // Continue processing the request
                    await next(context);

                    if (_settings.IncludeResponse) // write response log if enable
                    {
                        // Read the response body
                        responseBody.Seek(0, SeekOrigin.Begin);
                        string responseText = new StreamReader(responseBody).ReadToEnd();

                        if (!string.IsNullOrEmpty(responseText))
                            logContent += $"\r\nResponse: {responseText}";
                    }

                    // Copy the response body back to the original stream
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBody);
                }
                catch (Exception ex)
                {
                    logger.LogError("Unhandler exception when write request log with error {error}.", ex.Message);
                }
                finally
                {
                    context.Response.Body = originalBody;
                }
            }
            else
            {
                // Continue processing the request
                await next(context);
            }

            // Log the response body
            var statusCode = context.Response.StatusCode;
            var contentType = context.Response.Headers.ContentType.ToString();

            timer.Stop();

            var elapsedMilliseconds = timer.ElapsedMilliseconds;

            logger.LogInformation("{scheme} {method} {statusCode} {RequestPath}{RequestQuery} in {elapsedMilliseconds} ms {log}",
                requestScheme, requestMethod, statusCode, requestPath, requestQuery, elapsedMilliseconds, logContent);
        }
    }

    private static async Task<string> ReadBodyAsync(HttpRequest request)
    {
        // Ensure the request's body can be read multiple times 
        // (for the next middlewares in the pipeline).
        request.EnableBuffering();
        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();
        // Reset the request's body stream position for 
        // next middleware in the pipeline.
        request.Body.Position = 0;

        // minify request if is JSON
        if (request.ContentType?.Contains("application/json") is true)
            requestBody = Minify(requestBody);

        return requestBody;
    }

    private static string Minify(string json)
    {
        if (string.IsNullOrEmpty(json))
            return "<null>";

        var obj = JsonSerializer.Deserialize<object>(json);
        return JsonSerializer.Serialize(obj);
    }
}
