using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Light.AspNetCore.Modules;

public interface IModuleJobPipeline
{
    /// <summary>
    /// Configure Module Application Builder
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    IApplicationBuilder UseJobPipelines(IApplicationBuilder builder);

    /// <summary>
    /// Configure Module Application Builder with IConfiguration
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    IApplicationBuilder UseJobPipelines(IApplicationBuilder builder, IConfiguration configuration);
}


public abstract class ModuleJobPipeline : IModuleJobPipeline
{
    public virtual IApplicationBuilder UseJobPipelines(IApplicationBuilder builder) => builder;

    public virtual IApplicationBuilder UseJobPipelines(IApplicationBuilder builder, IConfiguration configuration) => builder;
}