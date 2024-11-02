using Light.Identity.EntityFrameworkCore;
using Light.Identity.EntityFrameworkCore.Options;
using Light.Identity.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
            services.AddDbContext<AppIdentityDbContext>(options => options.UseInMemoryDatabase("IdentityDb"));
        }
        else
        {
            services.AddDbContext<AppIdentityDbContext>(options => 
                options.UseSqlServer(connectionStr, o => o.MigrationsAssembly("Lightsoft.Identity.SqlServer")));
        }

        services.AddIdentity<AppIdentityDbContext>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;

            // Password settings
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 3;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

            // Lockout settings
            //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
            //options.Lockout.MaxFailedAccessAttempts = 10;

            // User settings
            options.User.RequireUniqueEmail = false;
        });

        // custom claims options
        services.AddTransient<IConfigureOptions<ClaimTypeOptions>, CustomClaims>();

        services.AddMigrator(configuration);

        return services;
    }

    public static async Task ConfigurePipelines(this WebApplication app)
    {
        await app.InitialiseDatabaseAsync();
    }
}
