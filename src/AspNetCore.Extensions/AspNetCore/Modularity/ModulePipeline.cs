using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Light.AspNetCore.Modularity;

public abstract class ModulePipeline : IModuleBuilder
{
    public virtual void ConfigurePipelines(IApplicationBuilder builder)
    { }

    public virtual void MapHub(IEndpointRouteBuilder builder)
    { }

    public virtual void MapEndpoints(IEndpointRouteBuilder builder)
    { }
}