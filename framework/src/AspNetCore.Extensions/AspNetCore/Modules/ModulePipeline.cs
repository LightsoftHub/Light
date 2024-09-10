using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Light.AspNetCore.Modules;

public interface IModulePipeline
{
    /// <summary>
    /// Configure Module Application Builder
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder);

    /// <summary>
    /// Configure Module Application Builder with IConfiguration
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder, IConfiguration configuration);
}


public abstract class ModulePipeline : IModulePipeline
{
    public virtual IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder) => builder;

    public virtual IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder, IConfiguration configuration) => builder;
}