using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Light.AspNetCore.Builder;

public abstract class ModuleApplicationBuilder : IModuleApplicationBuilder
{
    public virtual IApplicationBuilder ConfigurePipelines(
        IApplicationBuilder builder) => builder;

    public virtual IApplicationBuilder ConfigurePipelines(
        IApplicationBuilder builder,
        IConfiguration configuration) => builder;
}