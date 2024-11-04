using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Light.Identity.SqlServer;

public static class DependencyInjection
{
    public static IServiceCollection AddMigrator(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LightIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), o =>
            {
                o.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            }));

        services.AddIdentity<LightIdentityDbContext>();

        services.AddScoped<IdentityDbContextInitialiser>();

        return services;
    }
}
