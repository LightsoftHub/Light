using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Light.AspNetCore.Modularity;

public interface IModuleBuilder
{
    /// <summary>
    /// Configure Module Application Builder
    /// </summary>
    void Configure(IApplicationBuilder builder);

    /// <summary>
    /// Configure Module Hub Endpoint Route Builder
    /// </summary>
    void MapHub(IEndpointRouteBuilder builder);

    /// <summary>
    /// Configure Module Endpoint Route Builder
    /// </summary>
    void MapEndpoints(IEndpointRouteBuilder builder);

    /// <summary>
    /// Configure Module WebApplication
    /// </summary>
    void Configure(WebApplication app);
}