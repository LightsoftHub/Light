using Light.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Sample.Data.Persistence;
using Sample.Data.Repository;

namespace Sample.Data;

public static class Startup
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services
                .AddDbContext<AlphaDbContext>(opt =>
                    opt.UseInMemoryDatabase("MemoryDb"));
        }
        else
        {
            services
                .AddDbContext<AlphaDbContext>(opt =>
                    opt.UseSqlServer(defaultConnection));
        }

        services.AddScoped(typeof(IRepository<>), typeof(CustomRepository<>));

        services.AddUnitOfWork();

        services.AddScoped(typeof(ICacheRepository<,>), typeof(CacheRepository<,>));
        //services.AddScoped(typeof(ICacheRepository<>), typeof(CustomCacheRepository));

        services.AddUnitOfWork<AlphaDbContext>();

        //services.AddScoped(typeof(IAppRepository<>), typeof(AppRepository<>));
        //services.AddScoped(typeof(IAppUnitOfWork), typeof(AppUnitOfWork));

        return services;
    }

    public static void SeedData(this IApplicationBuilder builder)
    {
        var scope = builder.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AlphaDbContext>();

        var categories = new List<RetailCategory>();

        for (int i = 1; i <= 20; i++)
        {
            categories.Add(new RetailCategory
            {
                Name = $"Category {i}"
            });
        }

        context.RetailCategories.AddRange(categories);
        context.SaveChanges();
    }
}