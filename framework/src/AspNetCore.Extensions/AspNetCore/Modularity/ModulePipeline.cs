using Microsoft.AspNetCore.Builder;

namespace Light.AspNetCore.Modularity;

internal interface IModulePipeline
{
    /// <summary>
    /// Configure Module Application Builder
    /// </summary>
    IApplicationBuilder Use(IApplicationBuilder builder);

    /// <summary>
    /// For separate job pipelines
    /// </summary>
    IApplicationBuilder UseJob(IApplicationBuilder builder);
}

public abstract class ModulePipeline : IModulePipeline
{
    public virtual IApplicationBuilder Use(IApplicationBuilder builder) => builder;

    public virtual IApplicationBuilder UseJob(IApplicationBuilder builder) => builder;
}