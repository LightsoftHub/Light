using Light.AspNetCore.Modularity;
using Light.AspNetCore.Modularity.Pipelines;

namespace Sample.Modules;

public class OrderServices : LightModule
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<OrderMiddleware>();
        services.AddSingleton<OrderModuleService>();

        //Serilog.Log.Warning("Module {name} injected", GetType().FullName);
    }
}

public class OrderPipelines : ModulePipeline
{
    public override void Use(IApplicationBuilder builder)
    {
        builder.UseMiddleware<OrderMiddleware>();

        //Serilog.Log.Warning("Module {name} injected", GetType().FullName);
    }  
}

public class OrderJobs : ModulePipeline
{
    public override void Use(IApplicationBuilder builder)
    {
        //var scope = builder.ApplicationServices.CreateScope();

        //var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderJobs>>();

        //logger.LogWarning("Order Jobs injected");
    }
}

public class ProductServices : LightModule
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ProductMiddleware>();
        services.AddTransient<ProductModuleService>();

        //Serilog.Log.Warning("Module Product service injected");
    }
}

public class ProductPipelines : ModulePipeline
{
    public override void Use(IApplicationBuilder builder)
    {
        builder
            .UseMiddleware<ProductMiddleware>();
    }

    public override void UseJob(IApplicationBuilder builder)
    {
        //var scope = builder.ApplicationServices.CreateScope();

        //var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProductJobs>>();

        //logger.LogInformation("Module Product Jobs injected");
    }
}

