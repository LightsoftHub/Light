using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Light.AspNetCore.Modularity;

internal interface IModuleBuilder
{
    /// <summary>
    /// Configure Module Application Builder
    /// </summary>
    void ConfigurePipelines(IApplicationBuilder builder);

    /// <summary>
    /// Configure Endpoint Route Builder
    /// </summary>
    void MapEndpoints(IEndpointRouteBuilder builder);
}