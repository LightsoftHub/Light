using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity
{
    internal interface IModule
    {
        /// <summary>
        /// Module Service Collection
        /// </summary>
        IServiceCollection Add(IServiceCollection services);

        /// <summary>
        /// Module Service Collection with IConfiguration
        /// </summary>
        IServiceCollection Add(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// For separate job services
        /// </summary>
        IServiceCollection AddJob(IServiceCollection services);

        /// <summary>
        /// For separate job services with IConfiguration
        /// </summary>
        IServiceCollection AddJob(IServiceCollection services, IConfiguration configuration);
    }
}
