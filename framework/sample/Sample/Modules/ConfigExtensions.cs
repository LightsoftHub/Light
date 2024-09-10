using Light.AspNetCore.Modules;

namespace Sample.Modules;

public class OrderServices : Module
{
    public override IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<OrderMiddleware>();
        services.AddSingleton<OrderModuleService>();

        Serilog.Log.Warning("Module {name} injected", GetType().FullName);

        return services;
    }
}

public class OrderPipelines : ModulePipeline
{
    public override IApplicationBuilder UseJobPipelines(IApplicationBuilder builder,
        IConfiguration configuration)
    {
        builder.UseMiddleware<OrderMiddleware>();

        Serilog.Log.Warning("Module {name} injected", GetType().FullName);

        return builder;
    }  
}

public class OrderModuleJob : ModuleJobPipeline
{
    public override IApplicationBuilder UseJobPipelines(IApplicationBuilder builder, IConfiguration configuration)
    {
        var scope = builder.ApplicationServices.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderModuleJob>>();

        logger.LogWarning("Order Jobs injected");

        return builder;
    }
}

public class ProductModule :
    IModule,
    IModulePipeline
{
    public IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ProductMiddleware>();
        services.AddTransient<ProductModuleService>();

        Serilog.Log.Warning("Module Product service injected");

        return services;
    }

    public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    public IApplicationBuilder UseJobPipelines(IApplicationBuilder builder, IConfiguration configuration) =>
        builder
            .UseMiddleware<ProductMiddleware>();

    public IApplicationBuilder UseJobPipelines(IApplicationBuilder builder) => builder;
}

public class ProductModuleJob : IModuleJobPipeline
{
    public IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder)
    {
        var scope = builder.ApplicationServices.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProductModuleJob>>();

        logger.LogInformation("Product Jobs injected");

        return builder;
    }

    public IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder, IConfiguration configuration)
    {
        return builder;
    }
}

