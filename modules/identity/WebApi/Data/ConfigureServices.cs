using Light.ActiveDirectory;
using Light.Identity;
using Light.Identity.Options;
using Light.Identity.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data;

public static class ConfigureServices
{
    /// <summary>
    /// Config Identity data
    /// </summary>
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStr = configuration.GetConnectionString("DefaultConnection");

        var useMemoryDb = false;
        if (useMemoryDb)
        {
            //services.AddDbContext<LightIdentityDbContext>(options => options.UseInMemoryDatabase("IdentityDb"));
        }
        else
        {
            //services.AddDbContext<LightIdentityDbContext>(options => options.UseSqlServer(connectionStr));
        }

        // custom claims options
        //services.AddTransient<IConfigureOptions<ClaimTypeOptions>, CustomClaims>();

        services.AddMigrator(configuration);

        // Overide by BindConfiguration
        services.AddOptions<JwtOptions>().BindConfiguration("JWT");

        services.AddTokenServices();

        services.AddActiveDirectory();

        return services;
    }

    public static async Task ConfigurePipelines(this WebApplication app)
    {
        await app.InitialiseDatabaseAsync();
        await Task.CompletedTask;
    }
}
