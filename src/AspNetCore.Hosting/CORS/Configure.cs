using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Hosting.CORS;

public static class Configure
{
    private const string _corsPolicies = "CORS_Policies";

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("CORS").Get<CorsOptions>();

        if (settings != null && settings.Enable)
        {
            if (settings.AllowAll)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(_corsPolicies, builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
                });
            }
            else
            {
                var origins = new List<string>();

                string? apiGateways = settings.ApiGw;
                if (!string.IsNullOrEmpty(apiGateways))
                    origins.AddRange(apiGateways.Split(';', StringSplitOptions.RemoveEmptyEntries));

                string? blazor = settings.Blazor;
                if (!string.IsNullOrEmpty(blazor))
                    origins.AddRange(blazor.Split(';', StringSplitOptions.RemoveEmptyEntries));

                string? mvc = settings.Mvc;
                if (!string.IsNullOrEmpty(mvc))
                    origins.AddRange(mvc.Split(';', StringSplitOptions.RemoveEmptyEntries));

                if (origins.Count > 0)
                {
                    services.AddCors(opt =>
                        opt.AddPolicy(_corsPolicies, policy =>
                            policy
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .WithOrigins(origins.ToArray())));
                }
            }
        }

        return services;
    }

    public static IApplicationBuilder UseCorsPolicies(this IApplicationBuilder app, IConfiguration configuration)
    {
        var settings = configuration.GetSection("CORS").Get<CorsOptions>();

        if (settings != null && settings.Enable)
        {
            app.UseCors(_corsPolicies);
        }

        return app;
    }
}