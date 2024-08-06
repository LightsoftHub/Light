using Light.AspNetCore.Modules;

namespace Sample.Modules;

public class OrderConfigServices : ModuleServiceCollection
{
    public override IServiceCollection ConfigureServices(IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<OrderMiddleware>();
        services.AddSingleton<OrderModuleService>();

        return services;
    }
}

public class OrderConfigPipelines : ModuleApplicationBuilder
{
    public override IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder,
        IConfiguration configuration) =>
        builder
            .UseMiddleware<OrderMiddleware>();
}

public class ProductConfigServices :
    IModuleServiceCollection,
    IModuleApplicationBuilder
{
    public IServiceCollection ConfigureServices(IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ProductMiddleware>();
        services.AddTransient<ProductModuleService>();

        return services;
    }

    public IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder,
        IConfiguration configuration) =>
        builder
            .UseMiddleware<ProductMiddleware>();
}

