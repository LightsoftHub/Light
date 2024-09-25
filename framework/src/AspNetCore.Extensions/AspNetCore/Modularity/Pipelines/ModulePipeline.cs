using Microsoft.AspNetCore.Builder;

namespace Light.AspNetCore.Modularity.Pipelines;

public abstract class LightModule : IModulePipeline
{
    public virtual void ConfigurePipelines(IApplicationBuilder builder)
    { }

    public virtual void InitializeJobs(IApplicationBuilder builder)
    { }
}