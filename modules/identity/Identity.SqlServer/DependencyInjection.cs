using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.Identity.SqlServer;

public static class DependencyInjection
{
    public static IServiceCollection AddMigrator(this IServiceCollection services, IConfiguration configuration)
    {
        /*
        services.AddDbContext<MigratorDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<MigratorDbContext>();
        */
        // manual inject services here
        services.AddScoped<IdentityDbContextInitialiser>();

        return services;
    }
}
