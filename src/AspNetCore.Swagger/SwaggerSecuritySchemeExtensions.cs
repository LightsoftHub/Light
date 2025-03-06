using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Light.AspNetCore.Swagger;

internal static class SwaggerSecuritySchemeExtensions
{
    internal static SwaggerGenOptions AddJwtSecurityScheme(this SwaggerGenOptions swaggerGenOptions)
    {
        // Include 'SecurityScheme' to use JWT Authentication
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            Scheme = "bearer",
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        };

        swaggerGenOptions.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });

        return swaggerGenOptions;
    }

    internal static SwaggerGenOptions AddBasicSecurityScheme(this SwaggerGenOptions swaggerGenOptions)
    {
        // Include 'SecurityScheme' to use Basic Authentication
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            Scheme = "basic",
            Name = "Basic Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Description = "Put your basic Authentication on textbox below!",

            Reference = new OpenApiReference
            {
                Id = "basic",
                Type = ReferenceType.SecurityScheme
            }
        };

        swaggerGenOptions.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });

        return swaggerGenOptions;
    }
}
