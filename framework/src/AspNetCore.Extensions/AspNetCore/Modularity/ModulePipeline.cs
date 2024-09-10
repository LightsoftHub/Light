using Microsoft.AspNetCore.Builder;

namespace Light.AspNetCore.Modularity;

internal interface IModulePipeline
{
    /// <summary>
    /// Configure Module Application Builder
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder);
}

public abstract class ModulePipeline : IModulePipeline
{
    public virtual IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder) => builder;
}

public abstract class ModuleJob : IModulePipeline
{
    public virtual IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder) => builder;
}