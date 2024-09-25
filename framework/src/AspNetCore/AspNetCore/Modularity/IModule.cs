using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity
{
    internal interface IModule
    {
        /// <summary>
        /// Module Service Collection
        /// </summary>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Module Service Collection with IConfiguration
        /// </summary>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// For separate job services
        /// </summary>
        void ConfigureJobs(IServiceCollection services);

        /// <summary>
        /// For separate job services with IConfiguration
        /// </summary>
        void ConfigureJobs(IServiceCollection services, IConfiguration configuration);
    }
}