using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Light.AspNetCore.Swagger;

public static class Startup
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var sectionName = "Swagger";

        services.Configure<SwaggerSettings>(configuration.GetSection(sectionName));

        var settings = configuration.GetSection(sectionName).Get<SwaggerSettings>();

        ArgumentNullException.ThrowIfNull(settings, nameof(SwaggerSettings));

        if (settings.Enable)
        {
            if (settings.VersionDefinition)
            {
                services.AddTransient<IConfigureOptions<SwaggerGenOptions>, VersionDefinitionSwaggerOptions>();
            }

            services.AddSwaggerGen(opt =>
            {
                switch (settings.AuthMode)
                {
                    case "JWT":
                        opt.AddJwtSecurityScheme();
                        break;
                    case "basic":
                        opt.AddBasicSecurityScheme();
                        break;
                    default:
                        // code block
                        break;
                }

                opt.CustomSchemaIds(x => x.FullName); // fix Swagger when contain multi model, dto has same name

                //opt.DocumentFilter<TitleFilter>();

                opt.UseInlineDefinitionsForEnums();
            });

            services.AddTransient<IConfigureOptions<SwaggerUIOptions>, CustomSwaggerUIOptions>();
        }

        return services;
    }

    public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
    {
        var settings = app.ApplicationServices.GetRequiredService<IOptions<SwaggerSettings>>().Value;

        if (settings.Enable)
        {
            SwaggerBuilderExtensions.UseSwagger(app);

            if (settings.VersionDefinition)
            {
                var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

                app.UseSwaggerUI(options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                    }
                });
            }
            else
            {
                app.UseSwaggerUI();
            }
        }

        return app;
    }
}