namespace Sample.Modules;

public class OrderServices : Light.AspNetCore.Modularity.Module
{
    public override void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<OrderMiddleware>();
        services.AddSingleton<OrderModuleService>();

        //Serilog.Log.Warning("Module {name} injected", GetType().FullName);
    }
}

public class OrderPipelines : Light.AspNetCore.Modularity.ModulePipeline
{
    public override void Configure(IApplicationBuilder builder)
    {
        builder.UseMiddleware<OrderMiddleware>();

        //Serilog.Log.Warning("Module {name} injected", GetType().FullName);
    }
}

public class ProductServices : Light.AspNetCore.Modularity.Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddSingleton<ProductMiddleware>();
        services.AddTransient<ProductModuleService>();

        //Serilog.Log.Warning("Module Product service injected");
    }
}

public class ProductPipelines : Light.AspNetCore.Modularity.ModulePipeline
{
    public override void Configure(IApplicationBuilder builder)
    {
        builder
            .UseMiddleware<ProductMiddleware>();
    }
}

