﻿using Light.AspNetCore.Mvc;
using Light.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Light.Extensions.DependencyInjection;

public static class MvcBuilderExtensions
{
    /// <summary>
    /// Add controlers with lowercase name
    /// </summary>
    public static IMvcBuilder AddLowercaseControllers(this IServiceCollection services)
    {
        return services.AddControllers(options =>
        {
            options.Conventions.Add(new LowercaseControllerNameConvention());
        });
    }

    /// <summary>
    /// Default Json options settings for controllers
    /// </summary>
    public static IMvcBuilder AddDefaultJsonOptions(this IMvcBuilder builder)
    {
        return builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
    }

    /// <summary>
    /// Custom Invalid Model State Response
    /// </summary>
    public static IMvcBuilder AddInvalidModelStateHandler(this IMvcBuilder builder)
    {
        return builder.ConfigureApiBehaviorOptions(options =>
        {
            // custom Invalid model state response
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .ToDictionary(k => k.Key, v => v.Value!.Errors.Select(s => s.ErrorMessage))
                    .Select(s =>
                    {
                        // convert error from dictionary to model_prop: error1,error2,...
                        var modelState = $"{s.Key}: {string.Join(",", s.Value)}";

                        return modelState;
                    });

                var apiError = new Result
                {
                    Code = ResultCode.BadRequest.ToString(),
                    // convert errors to Model_Erorr1|Model_Error2|....
                    Message = string.Join("|", errors)
                };

                var result = new ObjectResult(apiError)
                {
                    StatusCode = (int)apiError.MapHttpStatusCode()
                };
                result.ContentTypes.Add(MediaTypeNames.Application.Json);

                return result;
            };
        });
    }
}