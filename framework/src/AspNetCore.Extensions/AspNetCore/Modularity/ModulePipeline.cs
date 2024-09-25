using Microsoft.AspNetCore.Builder;

namespace Light.AspNetCore.Modularity;

internal interface IModulePipeline
{
    /// <summary>
    /// Configure Module Application Builder
    /// </summary>
    void Use(IApplicationBuilder builder);

    /// <summary>
    /// For separate job pipelines
    /// </summary>
    void UseJob(IApplicationBuilder builder);
}

public abstract class ModulePipeline : IModulePipeline
{
    public virtual void Use(IApplicationBuilder builder)
    { }

    public virtual void UseJob(IApplicationBuilder builder)
    { }
}