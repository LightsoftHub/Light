using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity
{
    internal interface IModuleServiceCollection
    {
        /// <summary>
        /// Module Service Collection
        /// </summary>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Module Service Collection with IConfiguration
        /// </summary>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}