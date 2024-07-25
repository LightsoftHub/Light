using Light.Application.Common.Exceptions;
using Light.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Light.AspNetCore.Hosting.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next,
    ILogger<ExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger; // must use Microsoft Logger because only Singleton services can be resolved by constructor injection in Middleware

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Not write exception from Hangfire
            bool isHangfire = context.Request.Path.ToString().Contains("hangfire");
            var isHangfireLoginError = ex.Message.Contains("StatusCode cannot be set because the response has already started.");
            if (isHangfire && isHangfireLoginError)
                return;

            var traceId = context.TraceIdentifier;
            var response = context.Response;

            if (ex is not ExceptionBase && ex.InnerException != null)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
            }

            string? message;
            var errors = new List<string>();

            switch (ex)
            {
                case ExceptionBase e:
                    response.StatusCode = (int)e.StatusCode;
                    message = $"{ex.Message.Trim()} with Trace ID: {traceId}";
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    message = $"Not Found with Trace ID: {traceId}";
                    break;

                case Exception e:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = $"Error with Trace ID: {traceId}";
                    errors.Add(e.Message);
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = $"Internal Server Error with Trace ID: {traceId}";
                    break;
            }

            // Write exception log with Trace ID
            var exceptionSource = ex.TargetSite?.DeclaringType?.FullName;
            var log = $"Source: {exceptionSource}\r\nTrace ID: {traceId}\r\nStatus Code: {response.StatusCode}\r\nError: {ex.Message}";
            _logger.LogError("{log}", log);

            // Write exception as Result
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";

                var result = new Result
                {
                    Code = response.StatusCode.ToString(),
                    Message = message,
                    //Errors = errors,
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                await response.WriteAsJsonAsync(result, jsonOptions);
            }
            else
            {
                _logger.LogError("Can't write error response. Response has already started.");
            }
        }
    }
}
