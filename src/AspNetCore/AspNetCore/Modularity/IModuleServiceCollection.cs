using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity
{
    internal interface IModuleServiceCollection
    {
        /// <summary>
        /// Module Service Collection
        /// </summary>
        void Configure(IServiceCollection services);

        /// <summary>
        /// Module Service Collection with IConfiguration
        /// </summary>
        void Configure(IServiceCollection services, IConfiguration configuration);
    }
}