using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity;

public class ModuleJob
{
    /// <summary>
    /// For separate job services
    /// </summary>
    public virtual void Add(IServiceCollection services)
    { }

    /// <summary>
    /// For separate job services with IConfiguration
    /// </summary>
    public virtual void Add(IServiceCollection services, IConfiguration configuration)
    { }

    /// <summary>
    /// For separate job pipelines
    /// </summary>
    public virtual void Initialize(IApplicationBuilder builder)
    { }
}
