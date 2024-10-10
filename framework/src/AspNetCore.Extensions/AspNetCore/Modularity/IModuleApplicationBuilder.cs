using Microsoft.AspNetCore.Builder;

namespace Light.AspNetCore.Modularity;

internal interface IModuleApplicationBuilder
{
    /// <summary>
    /// Configure Module Application Builder
    /// </summary>
    void ConfigurePipelines(IApplicationBuilder builder);
}