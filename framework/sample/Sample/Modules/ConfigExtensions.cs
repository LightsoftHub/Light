using Light.AspNetCore.Modularity;

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
    public override IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder)
    {
        builder.UseMiddleware<OrderMiddleware>();

        Serilog.Log.Warning("Module {name} injected", GetType().FullName);

        return builder;
    }  
}

public class OrderJobs : ModuleJob
{
    public override IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder)
    {
        var scope = builder.ApplicationServices.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderJobs>>();

        logger.LogWarning("Order Jobs injected");

        return builder;
    }
}

public class ProductServices : Module
{
    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ProductMiddleware>();
        services.AddTransient<ProductModuleService>();

        Serilog.Log.Warning("Module Product service injected");

        return services;
    }
}

public class ProductPipelines : ModulePipeline
{
    public override IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder) =>
        builder
            .UseMiddleware<ProductMiddleware>();
}

public class ProductJobs : ModuleJob
{
    public override IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder)
    {
        var scope = builder.ApplicationServices.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProductJobs>>();

        logger.LogInformation("Module Product Jobs injected");

        return builder;
    }
}

