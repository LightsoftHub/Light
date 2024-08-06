using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.CORS;

public static class CorsExtensions
{

    const string CORS_POLICY_NAME = "CORS_Policies";

    static CorsOptions? GetCorsOptions(this IConfiguration configuration) =>
        configuration.GetSection("CORS").Get<CorsOptions>();

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetCorsOptions();

        if (settings != null && settings.Enable)
        {
            var corsPolicyName = CORS_POLICY_NAME;

            if (settings.AllowAll)
            {
                services.AddCors(opts =>
                {
                    opts.AddPolicy(corsPolicyName, builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
                });
            }
            else if (settings.Origins is not null)
            {
                services.AddCors(opts =>
                    opts.AddPolicy(corsPolicyName, policy =>
                        policy
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithOrigins(settings.Origins)));
            }
        }

        return services;
    }

    public static IApplicationBuilder UseCorsPolicies(this IApplicationBuilder app, IConfiguration configuration)
    {
        var settings = configuration.GetCorsOptions();

        if (settings != null && settings.Enable)
        {
            app.UseCors(CORS_POLICY_NAME);
        }

        return app;
    }
}
