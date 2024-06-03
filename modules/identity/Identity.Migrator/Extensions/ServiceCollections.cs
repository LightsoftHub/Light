using Light.Identity.Migrator.MSSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Light.Identity.Migrator.Extensions
{
    public static class ServiceCollections
    {
        /// <summary>
        ///     Register DI when run console.
        /// </summary>
        public static IServiceCollection AddServiceCollections(this IServiceCollection services, IConfiguration config)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            var connectionStr = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<MigratorDbContext>(options => options.UseSqlServer(connectionStr));

            services.AddIdentity<MigratorDbContext>();

            // manual inject services here
            services.AddScoped<AppDbContextInitialiser>();

            return services;
        }
    }
}
