using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Light.AspNetCore.Modules;

public abstract class ModuleApplicationBuilder : IModuleApplicationBuilder
{
    public virtual IApplicationBuilder ConfigurePipelines(
        IApplicationBuilder builder) => builder;

    public virtual IApplicationBuilder ConfigurePipelines(
        IApplicationBuilder builder,
        IConfiguration configuration) => builder;
}