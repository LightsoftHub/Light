using Asp.Versioning;
using Light.AspNetCore.Hosting.Extensions;
using Light.Contracts;
using Light.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Light.AspNetCore.Hosting;

public static class ConfigureExtensions
{
    /// <summary>
    /// add API version
    /// </summary>
    public static IApiVersioningBuilder AddApiVersion(this IServiceCollection services, int version, int minorVersion = 0)
        => services
        .AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(version, minorVersion);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        })
        .AddApiExplorer(o =>
        {
            o.GroupNameFormat = "'v'VVV";
            o.SubstituteApiVersionInUrl = true;
        });

    /// <summary>
    /// Add configuration files *.json from folder
    /// </summary>
    public static WebApplicationBuilder LoadJsonConfigurations(this WebApplicationBuilder builder, string[]? paths = null)
    {
        var configuration = builder.Configuration;

        // If not configure paths
        //      get default configuration paths in "JsonConfigurationPaths" section
        paths ??= builder.Configuration.GetSection("JsonConfigurationPaths").Get<string[]>();

        if (paths is null)
        {
            return builder; // use default config
        }

        var env = builder.Environment.EnvironmentName;

        // combine paths to string
        var path = Path.Combine(paths);

        // get directory info
        var dInfo = new DirectoryInfo(path);
        if (dInfo.Exists)
        {
            var files = dInfo.GetFiles("*.json").Where(x => !x.Name.Contains("appsettings"));

            foreach (var file in files)
            {
                configuration
                    .AddJsonFile(Path.Combine(path, file.Name), optional: false, reloadOnChange: true);
            }
        }

        // load after add json files for can override values at root configurations
        configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

        configuration.AddEnvironmentVariables();

        return builder;
    }

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