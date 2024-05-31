using Microsoft.Extensions.Options;

namespace Sample.TestOption;

public static class Startup
{
    public static IServiceCollection AddTestOptions(this IServiceCollection services, IConfiguration configuration)
    {
        // Overide by action
        services.AddTest(opt =>
        {
            opt.Name = "Test from Action";
            opt.Description = "Test description from Action";
        });

        // Overide by IConfigureOptions
        services.AddTransient<IConfigureOptions<TestOptions>, CustomTestOptions>();

        // Overide by read appsettings        
        services.Configure<TestOptions>(configuration.GetSection("Test"));

        // Overide by BindConfiguration
        services.AddOptions<TestOptions>().BindConfiguration("Test1");

        return services;
    }

    public static IServiceCollection AddTest(this IServiceCollection services, Action<TestOptions> action)
    {
        // this will overide action manual set to default options
        services.Configure(action);

        return services;
    }
}