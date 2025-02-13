using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Light.AspNetCore.CORS;

public static class CorsExtensions
{
    const string CORS_POLICY_NAME = "CORS_Policy";

    static CorsOptions? GetCorsOptions(this IConfiguration configuration) =>
        configuration.GetSection("CORS").Get<CorsOptions>();

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetCorsOptions();

        if (settings != null && settings.Enable)
        {
            var corsPolicyName = CORS_POLICY_NAME;

            if (settings.Origins is not null)
            {
                services.AddCors(opts =>
                    opts.AddPolicy(corsPolicyName, AddPolicy =>
                        AddPolicy
                            .WithOrigins(settings.Origins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()));
            }
            else
            {
                services.AddCors(opts =>
                    opts.AddPolicy(corsPolicyName, builder =>
                            builder
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()));
            }
        }

        return services;
    }

    public static IApplicationBuilder UseCorsPolicies(this IApplicationBuilder app)
    {
        var settings = app.ApplicationServices.GetRequiredService<IOptions<CorsOptions>>().Value;

        if (settings?.Enable is true)
        {
            app.UseCors(CORS_POLICY_NAME);
        }

        return app;
    }
}
