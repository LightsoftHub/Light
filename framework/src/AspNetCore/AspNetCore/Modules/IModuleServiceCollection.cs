using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modules
{
    public interface IModuleServiceCollection
    {
        /// <summary>
        /// Add Module Service Collection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        IServiceCollection ConfigureServices(IServiceCollection services) => services;

        /// <summary>
        /// Add Module Service Collection with IConfiguration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration) => services;
    }
}