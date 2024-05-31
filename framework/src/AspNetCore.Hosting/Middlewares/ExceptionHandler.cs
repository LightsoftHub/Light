using Light.Application.Common.Exceptions;
using Light.Contracts;
using Light.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Light.AspNetCore.Hosting.Middlewares;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Not write exception from Hangfire
        bool isHangfire = httpContext.Request.Path.ToString().Contains("hangfire");
        var isHangfireLoginError = exception.Message.Contains("StatusCode cannot be set because the response has already started.");
        if (isHangfire && isHangfireLoginError)
            return true;

        var traceId = httpContext.TraceIdentifier;
        var response = httpContext.Response;

        if (exception is not ExceptionBase && exception.InnerException != null)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
        }

        string? message;
        var errors = new List<string>();

        switch (exception)
        {
            case ExceptionBase e:
                response.StatusCode = (int)e.StatusCode;
                message = $"{e.Message.Trim()} with Trace ID: {traceId}";
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
        var exceptionSource = exception.TargetSite?.DeclaringType?.FullName;
        var log = $"Source: {exceptionSource}\r\nTrace ID: {traceId}\r\nStatus Code: {response.StatusCode}\r\nError: {exception.Message}";
        logger.LogError("{log}", log);

        // Write exception as Result
        if (!response.HasStarted)
        {
            response.ContentType = "application/json";

            var result = new Result
            {
                Code = (ResultCode)response.StatusCode,
                Message = message,
                Errors = errors,
            };

            var jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            await response.WriteAsJsonAsync(result, jsonOptions, cancellationToken: cancellationToken);
        }
        else
        {
            logger.LogError("Can't write error response. Response has already started.");
        }

        return true;
    }
}
