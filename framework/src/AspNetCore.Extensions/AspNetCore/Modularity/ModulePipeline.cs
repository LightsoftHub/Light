using Microsoft.AspNetCore.Builder;

namespace Light.AspNetCore.Modularity;

public abstract class ModulePipeline : IModuleApplicationBuilder
{
    public virtual void ConfigurePipelines(IApplicationBuilder builder)
    { }

    public virtual void InitializeJobs(IApplicationBuilder builder)
    { }
}